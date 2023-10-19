using System;
using Syn.WordNet;

namespace GamePrototype
{

	public abstract class Character
    {
		public string name;
		public readonly CharacterType type;
		public static Character currentlyTalking;
		public enum CharacterType
        {
			Player,
			NPC,
			God
        }

		public Character(string name)
        {
            this.name = name;
        }

		public string GetNounSubstitution(string phrase) 
        {
            string[] newPhrase = phrase.Split(' ');
			List<string> words = new List<string>();
			foreach (string word in newPhrase)
            {
				words.Add(word);
            }
			switch(type)
            {
				case CharacterType.Player:
					if(currentlyTalking.type != CharacterType.Player)
                    {
						words.Insert(0, "your");
					}
					else
                    {
						words.Insert(0, "my");
                    }
					break;
				case CharacterType.NPC:
					words.Insert(0, "your");
					break;
				case CharacterType.God:
					words.Insert(0, "your");
					break;
			}
			string rebuilt = "";
			int i = 0;
			foreach(string word in words)
            {
				if(i != words.Count-1)
                {
					rebuilt += word + " ";
				}
				else
                {
					rebuilt += word;
                }
				i++;
            }
			return rebuilt; 
        }

    }

	public class Player : Character
	{
		GameRunner game;
		public PlayerStats stats;
		public static Player player;
		public new readonly CharacterType type = CharacterType.Player;

		public Player(bool firstBoot, GameRunner runner, string name = "Player") : base (name)
		{
			game = runner;

			if(firstBoot)
            {
				stats = new PlayerStats() {health = new Health("health"), luckCounter = 0.1f, stress = 0, isHigh = false, awake = false};
            }

			player = this;
		}

		public void WakeUp()
        {
			Loop? wakeUp;
			if (!LoopManager.GetLoopByName("WakeUp", out wakeUp))
			{
				wakeUp = (NeglectableLoop)LoopManager.CreateLoop(game.locationManager.current, new Event(0), new Event(1), true, new NeglectData(true, true, new Event(2)), false);
			}

			stats.awake = true;
			Program.WriteOut("I woke up. The first thing I saw was the ceiling of " + GetNounSubstitution(game.locationManager.current.name) + ".~ I stayed in bed for a bit before I got up.", new CinematicType("Monolouge"));
			if(game.currentdata.stability.BaseValue > 1)
            {
				PolarStatement response = Program.GetPolarResponse("Do you want to brush your teeth?", true);
				if (response.yes)
				{
					wakeUp.Close(false);
				}
				else
				{
					wakeUp.Close(true);
				}
			}
			else
            {
				Program.WriteOut("Rather than think of brushing my teeth, I punched the mirror. It hurt. I liked it.");
				stats.health.Damage(false, 1);
				wakeUp.Close(true);
			}
			
        }

		public void Sleep()
        {

        }
	}

	public class NPC : Character
    {
		string gender;
		bool romanceable;
		public new readonly CharacterType type = CharacterType.NPC;
		//implement romance

		public NPC (string givenName, string gender, bool romanceable) : base (givenName)
        {
			this.gender = gender;
			this.romanceable = romanceable;
        }

	}

	public class God : Character
    {
		public new readonly CharacterType type = CharacterType.God;
		public God() : base("God")
		{
		}
	}

	public struct PlayerStats
    {
		public Health health;
		public float luckCounter, stress;
		public bool isHigh, awake;
    }
}