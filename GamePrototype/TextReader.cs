using System;
using Syn.WordNet;


namespace GamePrototype
{
    public class UserInput
    {
        protected string rawstr;
        protected Word[] words;
        public UserInput(string raw)
        {
            rawstr = raw;
            char[] seperators = new char[4] { ' ', ',', '.', '?'};
            string[] rawwords = rawstr.Split(seperators, StringSplitOptions.TrimEntries);

            words = new Word[rawwords.Length];

            for(int i = 0; i < rawwords.Length; i++)
            {
                words[i] = new Word() {word = rawwords[i], sentence = rawstr, position = i, type = TextReader.GetWordType(rawwords[i])};
            }

        }
    }

    public struct PolarAttributeQuestion
    {
        public string question;
        public int baseWorth;
        public float multiplierWorth;
        public bool skippable, positive, regressive;
    }


    public class MultipleChoicePrompt
    {
        public string question;
        public string[] answers;

        public MultipleChoicePrompt(string question, string[] answers)        {
            this.question = question;
            this.answers = answers;
        }   

    }

    public class MultiplieChoiceAttributeQuestion : MultipleChoicePrompt
    {
        public int[] baseWorths;
        public float[]? multiplierWorths;
        public bool skippable;

        public MultiplieChoiceAttributeQuestion(string question, string[] answers, int[] baseWorths, bool skippable, float[]? multiplierWorths = null) : base(question, answers)
        {
            this.baseWorths = baseWorths;
            this.skippable = skippable;
            this.multiplierWorths = multiplierWorths;
        }   
    }

    public class MultiTargetMCQ : MultiplieChoiceAttributeQuestion
    {
        public Dictionary<int, CoreAttribute> targets = new Dictionary<int, CoreAttribute>();

        public MultiTargetMCQ(string question, string[] answers, int[] baseWorths, bool skippable, CoreAttribute[] inputTargets, float[]? multiplierWorths = null) : base(question, answers, baseWorths, skippable, multiplierWorths)
        {
            for (int i = 0; i < inputTargets.Length; i++)
            {
                targets.Add(i, inputTargets[i]);
            }
        }
    }

    public struct MCQResponse
    {
        public int index;
        public bool skipped;
    }
 
    public class PolarStatement : UserInput
    {
        public bool yes, valid, skipped;
        public string rawresponse;
        public PolarStatement(string raw) : base(raw)
        {
            bool found = false;
            rawresponse = raw;
            foreach (Word word in words)
            {
                if (word.type == TextReader.WordTypes.PolarA && !found)
                {
                    // checks if the polar answer is yes or no, using the PolarIndex system, which has access to a dictionary of polar values
                    PolarIndex values = GetPolarValue(word);
                    
                    if (values.valid)
                    {
                        found = true;
                        yes = values.yes;
                        valid = true;
                    }

                }
                else if (word.type == TextReader.WordTypes.PolarA && found)
                {
                    valid = false;
                    break;
                }
            }
        }

        PolarIndex GetPolarValue(Word word)
        {
            bool affirm = false, valid = false;
            
            foreach (KeyValuePair<string,bool> association in TextReader.polarValues)
            {
                if (word.word == association.Key || word.word == TextReader.SwapCase(association.Key))
                {
                    affirm = association.Value;
                    valid = true;
                    break;
                }
            }

            return new PolarIndex() { valid = valid, yes = affirm };
        }
    }

    public struct PolarIndex
    {
        public bool yes, valid;
    }

    public struct Word
    {
        public string word, sentence;
        public int position;
        public TextReader.WordTypes type;
    }
    public static class TextReader
    {

        public static WordNetEngine wordNet;
        public enum WordTypes
        {
            PolarA,
            WhA,
            PolarQ,
            WhQ,
            Particle,
            Noun,
            Adjective,
            Verb,
            Adverb,
            Other
            // continue this
        }
        public static Dictionary<WordTypes, string[]> potentials = new Dictionary<WordTypes, string[]>()
        {
            {WordTypes.PolarA, new string[34]{"yes", "no","yeah", "sure", "alright", "certainly", "absolutely","indeed", "affirmative", "agreed", "roger", "aye", "yah", "yep", "yup", "uh-huh", "okay", "OK", "ok", "okey-dokey", "okey-doke", "achcha", "righto", "righty-ho", "surely", "yea","no", "negative", "never", "nae", "nope", "nah", "naw", "nay"}},
            {WordTypes.WhQ, new string[6] {"who", "what", "when", "where", "why", "how"} }
            // continue initalizing
        };

        public static Dictionary<string, bool> polarValues = new Dictionary<string, bool>()
        {
            {"yes", true }, {"yeah", true}, {"sure", true}, {"alright", true}, {"certainly", true}, {"absolutely", true}, {"indeed", true}, {"affirmative", true},
            {"agreed", true}, {"roger", true}, {"aye", true}, {"yah", true}, {"yep", true}, {"yup", true}, {"uh-huh", true}, {"okay", true}, {"OK", true}, {"ok", true},
            {"okey-dokey", true}, {"okey-doke", true}, {"achcha", true}, {"righto", true}, {"righty-ho", true}, {"surely", true}, {"yea", true}, {"no", false}, {"negative", false},
            {"never", false}, {"nae", false}, {"nope", false}, {"nah", false}, {"naw", false}, {"nay", false}
        };

        public static WordTypes GetWordType(string word)
        {

            WordTypes output = WordTypes.Other;
            bool found = false;

            foreach (KeyValuePair<WordTypes, string[]> pair in potentials)
            {
                foreach (string potential in pair.Value)
                {
                    if (potential == word || SwapCase(potential) == word)
                    {
                        output = pair.Key;
                        found = true;
                        break;
                    }
                }
                if(found) { break; }
            }

            return output;
        }

        public static string SwapCase(string word)
        {
            char letter = word[0];
            string output;
            if(Char.IsUpper(letter))
            {
               output = letter.ToString().ToLower() + word.Substring(1);
            }
            else
            {
               output = letter.ToString().ToUpper() + word.Substring(1);
            }
            return output;

        }

        public static void LoadWordNet()
        {
            WordNetEngine WNInstance = new WordNetEngine();

            WNInstance.LoadFromDirectory(Directory.GetCurrentDirectory());
            wordNet = WNInstance;
        }

    }

}