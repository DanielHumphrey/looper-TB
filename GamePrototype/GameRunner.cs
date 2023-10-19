using System;

namespace GamePrototype
{
    public class GameRunner
    {
        public GameData currentdata;
        public LocationManager locationManager;
        Player player;
        public static GameRunner instance;
        public GameRunner(bool newgame, GameData dataset = null)
        {
            if (newgame)
            {
                if (!Program.debug)
                {
                    Program.WriteOut("Every time somebody does something to us, or we do something to somebody, we open or close a loop.", new CinematicType("MonolougeSkippable"));
                    Program.WriteOut("Think of loops as a way of keeping score. If someone buys you dinner, it goes that at some point you'll have the chance to buy someone dinner.", new CinematicType("MonolougeSkippable"));
                    Program.WriteOut("Of course nothing is perfect, but in general this is how it works.", new CinematicType("MonolougeSkippable"));
                    Program.WriteOut("This is a story driven and powered by loops. Everything you do will open or close one.", new CinematicType("MonolougeSkippable"));
                    Program.WriteOut("Don't worry about it for now though, all we're doing now is talking before the fact.", new CinematicType("MonolougeSkippable"));
                    Program.WriteOut("To begin, I need to find out some things about you. There are no right or wrong answers to the following questions.", new CinematicType("MonolougeSkippable"));
                    Program.WriteOut("Just answer with your heart. For now, don't worry about loops.", new CinematicType("MonolougeSkippable"));
                }
                currentdata = BeginTest();

                StartGame(true);

            }
            else
            {
                currentdata = dataset;
                StartGame(true);
            }

        }

        void StartGame(bool needsBoot)
        {
            bool boot = needsBoot;
            if (boot)
            {
                Console.Clear();
                // boot the location manager
                locationManager = currentdata.InitalizeLocationManager(new MultipleChoicePrompt("Which of the following would you consider to be more of a home to you, at this point in your life?", new string[4] { "A college dorm room", "Your parent(s) house", "Your apartment (if you have one)", "Your house (if you have one)" }));
                // create the Player
                player = new Player(true, this);
                boot = false;
            }
            instance = this;
            // Begin events with the wakeup loop. We call the method from here for this first time, just to get things started.
            player.WakeUp();

        }

        public GameData BeginTest()
        {
            GameData data = new GameData(this);

            // at some point review these
            if(Program.debug)
            {
                data.socialIQ.BaseValue = 6;
                data.iq.BaseValue = 6;
                data.trauma.BaseValue = 6;
                data.physicalIQ.BaseValue = 6;
                data.stability.BaseValue = 6;
            }
            else
            {
                data.socialIQ.SetWithPolarQ(new PolarAttributeQuestion() { question = "Do you like to go out on Saturday nights?", baseWorth = 2, multiplierWorth = 0.5f, positive = true, regressive = false, skippable = false });
                data.SetMultiple(new MultiTargetMCQ("What did you used to get for your birthday as a child?",
                  new string[3]
                  {
                    "Brain games like puzzles or math sets.", "Sports equipment like baseball bats or footballs.", "I usually didn't get anything for my birthday as a child."
                  },
                  new int[3] { 1, 1, 1 },
                  false, new CoreAttribute[3] { data.iq, data.physicalIQ, data.trauma })
                {

                });
                data.socialIQ.SetWithPolarQ(new PolarAttributeQuestion() { question = "If someone offered you a pill they said would make you feel great, would you take it on the spot?", baseWorth = -1, multiplierWorth = 0.0f, positive = true, regressive = true, skippable = false });
                data.iq.SetWithPolarQ(new PolarAttributeQuestion() { question = "Do you enjoy challenging homework or work assignments?", baseWorth = 2, multiplierWorth = 0.1f, positive = true, regressive = false, skippable = false });
                data.stability.SetWithMCQ(new MultiplieChoiceAttributeQuestion("You're minding your buisness walking down the street, and all of a sudden a prank YouTuber sneaks up on you and screams in your ear. What do you do?",
                    new string[5]
                    {
                    "Scream for help and run.", "Flip them off", "Turn around and punch them in the face", "The previous response, but also make sure they're knocked out", "Kill them"
                    }, new int[5] { 1, 1, 0, -1, -2 }, false, new float[5] { 0.1f, 0.0f, 0.0f, 0.0f, 0.5f }));
                data.socialIQ.SetWithPolarQ(new PolarAttributeQuestion() { question = "Is it a social faliure to sit alone at lunch or dinner?", baseWorth = -1, multiplierWorth = 0.1f, positive = true, regressive = false, skippable = false });
                data.trauma.SetWithPolarQ(new PolarAttributeQuestion() { question = "Were you ever physically attacked by your parents as a child? Corperal punishment such as spanking counts.", baseWorth = 1, multiplierWorth = 0.3f, positive = true, regressive = false, skippable = false });
                data.SetMultiple(new MultiTargetMCQ("A stranger offers you a bite of their food. You are very hungry, and probably won't eat for a few more hours. What do you do?", new string[5]
                {
                "Take the food and thank them.", "Call the police and report a strange person attempting to poison bystanders", "Politly decline", "Take the food and throw it in their face, telling them it's obviously contaminated with fentanyl.", "Ask them why they are trying to help you"
                }, new int[5] { -1, -1, 2, -2, 2 }, false, new CoreAttribute[5] { data.iq, data.stability, data.stability, data.stability, data.trauma }));
                data.socialIQ.SetWithPolarQ(new PolarAttributeQuestion() { question = "Would you drop working on an important assignment to hang out with your friend if they had a bad day?", baseWorth = 1, multiplierWorth = 0.0f, positive = true, regressive = true, skippable = false });
                data.physicalIQ.SetWithPolarQ(new PolarAttributeQuestion() { question = "Do you like to play sports?", baseWorth = 3, multiplierWorth = 0.5f, positive = true, regressive = false, skippable = false });
                data.socialIQ.SetWithMCQ(new MultiplieChoiceAttributeQuestion("What's a worse fate to you?",
                    new string[3]
                    {
                    "Dying alone.", "Dying saving a stranger's life.", "Dying and not having an obituary."
                    },
                    new int[3] { 1, -2, -1 },
                    false, new float[3] { 0.5f, -0.5f, 0.0f })
                {

                });
            }
            Program.WriteOut("Okay, one last question.");
                       
            return data;
        }


    }
}