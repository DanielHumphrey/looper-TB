using System;

namespace GamePrototype
{
	public static class EventManager
	{
		public static Event current;
	}

    public static class ScriptManager
    {
        public static event EventHandler<int> BroadcastScript;

        public static void ExecuteScript(int id)
        {

            switch (id) { 
                case 0:
                    // waking up
                    Console.WriteLine("Point 0 reached");
                    Player.player.WakeUp();
                    break;
                case 1:
                    // doing hygine
                    Console.WriteLine("hygine done");
                    ExecuteScript(3);
                    break;
                case 2:
                    // not doing hygine
                    Console.WriteLine("hygine not done");
                    ExecuteScript(3);
                    break;
                case 3:
                      Program.WriteOut("You walk out of your " + )
                default:
                    Program.WriteOut("Error: script not found!");
                    break;
            }

            Console.WriteLine("Script ran and id was " + id);
            BroadcastScript(null, id);

        }
    }

	public class Event
    {
		public static int counter;
		int id;
        public int scriptID;

		public Event(int script)
        {
            id = counter;
            counter++;
            this.scriptID = script;
        }

        public void Execute()
        {
            ScriptManager.ExecuteScript(scriptID);
        }
    }

}
