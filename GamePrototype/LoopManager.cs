using System;

namespace GamePrototype
{
    public static class LoopManager
    {
        public static List<Loop> loops = new List<Loop>();
        public static List<NeglectableLoop> neglected = new List<NeglectableLoop>();
        public static List<int> oldIds = new List<int>();
        public static Loop? current;

        public static Loop CreateLoop(Location loc, Event starter, Event closer, bool geographic, NeglectData neglect, bool execute = true)
        {
            Loop newLoop;
            if(neglect.neglectable)
            {
                newLoop = new NeglectableLoop(CreateUniqueID(), loc, EventManager.current, starter, closer, geographic, neglect);

            }
            else
            {
                newLoop = new Loop(CreateUniqueID(), loc, EventManager.current, starter, closer, geographic);

            }
            loops.Add(newLoop);

            if (execute)
            {
                newLoop.Start();
            }

            return newLoop;
        }

        public static bool GetLoopByName(string name, out Loop? foundLoop)
        {
            Loop? indexLoop = null;
            bool wasFound = false;
            foreach(Loop loop in loops)
            {
                if (loop.name == name)
                {
                    indexLoop = loop;
                    wasFound = true;
                }
            }
            foundLoop = indexLoop;
            return wasFound;
        }

        public static void CloseLoop(Loop loop, bool neglected = false)
        {
            foreach (Loop foundLoop in loops)
            {
                if (foundLoop.id == loop.id)
                {
                    if(!neglected)
                    {
                        oldIds.Add(loop.id);

                    }
                    loops.Remove(foundLoop);
                    break;
                }
            }
        }

        static int CreateUniqueID()
        {
            int id = GameData.counter;
            GameData.counter++;
            return id;
        }

    }

    public class Loop
    {
        public int id;
        public string name;
        public Location createdAt;
        public bool geographic;
        public Event closer, start;

        public Loop(int id, Location createdAt, Event creator, Event starter, Event closer, bool geographic, string name = "N/A")
        {
            this.id = id;
            this.createdAt = createdAt;
            this.geographic = geographic;
            start = starter;
            this.closer = closer;
            this.name = name;

        }

        public void Start()
        {
            LoopManager.current = this;
            start.Execute();
        }

        public virtual void Close(bool neglect = true)
        {
            closer.Execute();
            LoopManager.CloseLoop(this);
        }

    }


    public class NeglectableLoop : Loop
    {
        NeglectData neglectData;
        public NeglectableLoop(int id, Location createdAt, Event creator, Event starter, Event closer, bool geographic, NeglectData negData, string name = "N/A") : base(id, createdAt, creator, starter, closer, geographic, name)
        {
            neglectData = negData;
            ScriptManager.BroadcastScript += Reinstate;
        }

        // if a loop can return to it's unnegleted state, it should be reinstated.
        public void Reinstate(object sender, int scriptID)
        {
            if(neglectData.reinstatable && scriptID == neglectData.reinstateID)
            {
                Wipe();
                LoopManager.CloseLoop(this);
                LoopManager.CreateLoop(GameRunner.instance.locationManager.current, start, closer, geographic, neglectData);
            }
        }

        void EndPoorly()
        {
            neglectData.badEnding.Execute();
            Wipe();
            LoopManager.CloseLoop(this);
        }

        public override void Close(bool neglect = true)
        {
            // if you close a neglectable loop it gets neglected, unless otherwise specified
            if(neglect)
            {
                if (neglectData.timesNeglected == 0)
                {
                    LoopManager.neglected.Add(this);

                }

                neglectData.timesNeglected++;
                if (neglectData.canEndPoorly && neglectData.timesNeglected > neglectData.threshold) 
                {
                    EndPoorly();
                }
                else
                {
                    neglectData.neglect.Execute();
                }
                LoopManager.CloseLoop(this, true);
            }
            else
            {
                Wipe();
                base.Close();
            }
        }

        void Wipe()
        {
            if (LoopManager.neglected.Contains(this))
            {
                LoopManager.neglected.Remove(this);
            }
            ScriptManager.BroadcastScript -= Reinstate;
        }
    }


    public struct NeglectData
    {
        public bool reinstatable, neglectable, canEndPoorly;
        public Event neglect;
        public Event? badEnding;
        public int timesNeglected, reinstateID, threshold;

        public NeglectData(bool neglectable, bool reinstatable, Event neglect, int reinstateid = -1)
        {
            timesNeglected = 0;
            this.neglect = neglect;
            this.reinstatable = reinstatable;
            this.neglectable = neglectable;
            reinstateID = reinstateid;
            canEndPoorly = false;
            badEnding = null;
            threshold = 0;
        }

        public NeglectData(Event neglect, Event badEnding, int threshold, bool reinstatable, int reinstateid = -1)
        {
            timesNeglected = 0;
            this.neglect = neglect;
            this.badEnding = badEnding;
            this.reinstatable = reinstatable;
            this.threshold = threshold;
            reinstateID = reinstateid;
            neglectable = true;
            canEndPoorly = true;
        }

    }

}

