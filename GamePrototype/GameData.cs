namespace GamePrototype
{
    public class GameData
    {
        GameRunner boundrunner;
        public CoreAttribute iq, socialIQ, physicalIQ, stability, trauma;
        LocationManager boundLocationManager;
        public static int counter;

        public GameData(GameRunner runner)
        {
            boundrunner = runner;

            iq = new CoreAttribute("intelligence");
            socialIQ = new CoreAttribute("social intelligence");
            physicalIQ = new CoreAttribute("strength");
            stability = new CoreAttribute("emotional stability", -1);
            trauma = new Trauma("trauma", -1);
        }

        public LocationManager InitalizeLocationManager(MultipleChoicePrompt question)
        {
            MCQResponse response = Program.GetMCQResponse(question, true);
            Location chosenHome;

            switch(response.index)
            {
                case 0:
                    chosenHome = new Location("dorm room", true);
                    break;
                case 1:
                    chosenHome = new Location("parent's house", true);
                    break;
                case 2:
                    chosenHome = new Location("apartment", true);
                    break;
                case 3:
                    chosenHome = new Location("house", true);
                    break;
                default:
                    // this should never happen
                    chosenHome = new Location("Error Room", true);
                    break;
            }

            LocationManager newManager = new LocationManager(true, chosenHome, chosenHome);
            boundLocationManager = newManager;
            return newManager;
                
        }

        public void SetMultiple(MultiTargetMCQ question)
        {
            Console.Clear();
            MCQResponse ans = Program.GetMCQResponse(question);

            if (question.skippable && question.answers[ans.index] == "Skip.")
            {
                ans.skipped = true;
                Program.WriteOut("Question skipped.");
            }
            else
            {
                question.targets[ans.index].BaseValue += question.baseWorths[ans.index];
                if(question.multiplierWorths != null)
                {
                    question.targets[ans.index].multiplier += question.multiplierWorths[ans.index];
                }
                ans.skipped = false;
            }
        }

    }

    public class Health : CoreAttribute
    {
        public Health (string name, int critical = 0) : base(name, critical)
        {

        }
        public void Damage(bool gradual, int amount, float interval = 0.0f)
        {
            if(!gradual)
            {
                BaseValue -= amount;

            }
            else
            {
                // implement interval damage here
            }
        }
    }

    public class CoreAttribute
    {
        protected string name;
        protected int baseValue = 0;
        public virtual int BaseValue
        {
            get { if (baseValue < criticalThreshold) { Program.WriteOut("Warning! Your " + name + " is very low! If you continue like this, you're sure to have trouble closing loops... and god knows what else.");} return baseValue;}
            set => baseValue = value;
        }
        public float multiplier = 1;
        protected int criticalThreshold;

        public CoreAttribute(string name, int critical = 0)
        {
            this.name = name;
            criticalThreshold = critical;
        }
        public void SetWithPolarQ(PolarAttributeQuestion question)
        {
            Console.Clear();
            PolarStatement ans = Program.GetPolarResponse(question.question);

            if (ans.valid)
            {
                if (ans.yes)
                {
                    if (question.positive)
                    {
                        baseValue += question.baseWorth;
                        multiplier += question.multiplierWorth;
                    }
                    else
                    {
                        baseValue -= question.baseWorth;
                        multiplier -= question.multiplierWorth;
                    }
                }
                else if (question.regressive)
                {
                    baseValue -= question.baseWorth;
                    multiplier -= question.multiplierWorth;
                }

                ans.skipped = false;
            }
            else if (ans.rawresponse == "skip" || ans.rawresponse == "Skip")
            {
                Program.WriteOut("Question skipped.");
                ans.skipped = true;
            }
        }

        public void SetWithMCQ(MultiplieChoiceAttributeQuestion question)
        {
            Console.Clear();
            MCQResponse ans = Program.GetMCQResponse(question, true);

            if(question.skippable && question.answers[ans.index] == "Skip.")
            {
                ans.skipped = true;
                Program.WriteOut("Question skipped.");
            }
            else
            {
                baseValue += question.baseWorths[ans.index];
                if(question.multiplierWorths != null)
                {
                    multiplier += question.multiplierWorths[ans.index];

                }
                ans.skipped = false;
            }
            


        }
    }

    public class Trauma : CoreAttribute
    {
        public override int BaseValue
        {
            get { if (baseValue < criticalThreshold) { Program.WriteOut("Watch out, you're extremely traumatized. If you continue like this, you're sure to have trouble closing loops... and god knows what else."); } return baseValue; }
            set => baseValue = value;
        }
        public Trauma(string name, int critical = 0) : base(name, critical) 
        {
            
        }
    }
}