using System;

namespace GamePrototype
{

    public struct CinematicType
    {
        public float delaytime, pauseinterval;
        public  bool requireconf, skippable;
        public CinematicType(string spec)
        {
            switch (spec)
            {
                case "Start":
                    delaytime = 0.07f; pauseinterval = 2f; requireconf = false; skippable = false;
                    break;
                case "Monolouge":
                    delaytime = 0.08f; pauseinterval = 1f; requireconf = true; skippable = false;
                    break;
                case "MonolougeSkippable":
                    delaytime = 0.08f; pauseinterval = 1f; requireconf = true; skippable = true;
                    break;
                case "DialougeDramatic":
                    delaytime = 0.5f; pauseinterval = 1f; requireconf = true; skippable = false;
                    break;
                case "DialougeSlow":
                    delaytime = 0.2f; pauseinterval = 0.7f; requireconf = true; skippable = true;
                    break;
                case "DialougeMedium":
                    delaytime = 0.1f; pauseinterval = 0.7f; requireconf = true; skippable = true;
                    break;
                case "DialougeFast":
                    delaytime = 0.05f; pauseinterval = 0.5f; requireconf = true; skippable = true;
                    break;
                case "MCQ":
                    delaytime = 0.05f; pauseinterval = 0.5f; requireconf = false; skippable = true;
                    break;
                default:
                    delaytime = 0.05f;
                    pauseinterval = 0.5f;
                    requireconf = true;
                    skippable = true;
                    break;
            }
           
        }
       
    }


    public static class Program
    {
        public static readonly bool debug = true;
        public static God god = new God();
        public static void Main(string[] args)
        {
            if(!debug)
            {
                Console.Clear();
                WriteOut("~A game by Danny Humphrey", new CinematicType("Start"));
                Console.WriteLine();
                WriteOut("~With illustrations by Lilith Humphrey", new CinematicType("Start"));
                Console.WriteLine();
                WriteOut("~Misadventure Presents:~", new CinematicType("Start"));
                Console.Clear();
                WriteOut("~Looper. A game of life.~", new CinematicType("Start"));
                Console.WriteLine();
            }
                     
            // make profile storage here.

            GameRunner main = new GameRunner(true);

        }

        public static void WriteOut(string towrite)
        {
            WriteOut(towrite, new CinematicType("default"), god);
        }

        public static void WriteOut(string towrite, CinematicType cinedata)
        {
            WriteOut(towrite, cinedata, god);
        }
        public static void WriteOut(string towrite, CinematicType cinedata, Character talking)
        {
            Character.currentlyTalking = talking;
            // checks my punctuation so I don't have to while writing.
            if (towrite.Last<char>() != '.' && towrite.Last<char>() != '!' && towrite.Last<char>() != '?')
            {
                towrite += '.';
            }

            char[] chars = towrite.ToCharArray();
            int waittimems = Convert.ToInt32(cinedata.delaytime * 1000);
            int pauseintms = Convert.ToInt32(cinedata.pauseinterval * 1000);

            for (int i = 0; i < towrite.Length; i++)
            {
                Thread.Sleep(waittimems);
                if (chars[i] == '~')
                {
                    Thread.Sleep(pauseintms);
                }
                else
                {
                    Console.Write(chars[i]);
                    if (Console.KeyAvailable)
                    {
                       ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter && cinedata.skippable)
                        {
                            string rest = "";
                            for (int x = i + 1; x < chars.Length; x++)
                            {
                                if (chars[x] != '~')
                                {
                                    rest += chars[x];

                                }
                            }
                            Console.Write(rest);
                            cinedata.requireconf = false;
                            break;
                        }
                    }
                }
            }

            if (cinedata.requireconf)
            {
                HoldForConf();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
            }

        }

        static void HoldForConf()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
            {
                return;
            }
            else
            {
                HoldForConf();
            }

        }


        public static PolarStatement GetPolarResponse(string question, bool firsttime = true)
        {
            if (firsttime)
            {
                WriteOut(question);
            }
            string response = Console.ReadLine();

            PolarStatement fullresponse = new PolarStatement(response);
            if (fullresponse.valid)
            {
                return fullresponse;
            }
            else if(!fullresponse.valid && response != "skip" && response != "Skip") 
            {
                WriteOut("Invalid answer!");
                return GetPolarResponse(question, false);
            }
            else
            {
                return fullresponse;
            }
        }

        public static MCQResponse GetMCQResponse(MultipleChoicePrompt question, bool firsttime = true)
        {
            if(firsttime)
            {
                WriteOut(question.question, new CinematicType("MCQ"));
                Console.WriteLine();
                for(int i = 0; i < question.answers.Length; i++)
                {
                    WriteOut((i+1) + " " + question.answers[i], new CinematicType("MCQ"));
                    Console.WriteLine();
                }

                WriteOut("Please input a number between 1 and " + question.answers.Length);
            }

            string response = Console.ReadLine();
            int index;

            if (!int.TryParse(response, out index))
            {
                WriteOut("Invalid response, please input a number!");
                return GetMCQResponse(question, false);
            }
            else if (index - 1 >= question.answers.Length || index - 1 < 0)
            {
                WriteOut("Invalid response, number is too high or low. Please input a number between 1 and " + question.answers.Length);
                return GetMCQResponse(question, false);
            }
            else
            {
                return new MCQResponse {index = index - 1};
            }
        }
    }


}