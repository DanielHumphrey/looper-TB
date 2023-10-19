using System;

namespace GamePrototype
{
	public class LocationManager
	{
		public Location current, home;
		public List<Character> charactersInRoom = new List<Character>();
		public enum Locations
        {
			Home,
			SchoolMain
			//add more
        }

		public Dictionary<Locations, Location> locationList = new Dictionary<Locations, Location>();
		
		public LocationManager(bool firstboot, Location current, Location home)
        {
			if(firstboot)
            {
				this.home = home;
            }
			this.current = current;
        }

		public void Move(Location loc, Character?[] extras = null)
        {
			
        }

	}

	public class Location
    {
		public string name, description;
		public bool isHome, hasPlayer;
		public Location(string name, bool isHome = false)
        {
			this.name = name;
			this.isHome = isHome;
        }

    }

}
