using System;
using System.Collections.Generic;
using SpaceSim;

class Astronomy {
	public static void Main() {
		//TODO: legge inn tall på måner
		Star sun = new Star("Sun", "Yellow", 1390000, 0, 0, (27 * 24));
		sun.position[0] = 0;
		sun.position[1] = 0;

		List<SpaceObject> solarSystem = new List<SpaceObject>
		{
			//new Star("Sun", "Yellow", 1390000, 0, 0, (27 * 24)),
			sun,
			new Planet("Mercury", "Darkgrey", 2439, 57910, 88 * 24, 1416, 47, null),
			new Planet("Venus", "Beige", 6051, 108200, 225 * 24, 243, 35, null),
			new Planet("Earth", "Blue", 6371, 149600, 365 * 24, 24, 30,
				new List<Moon>(){ new Moon("Moon", "White",  3475 / 2, 384400 / 1000, 30 * 24, 27 * 24 , 1, null)}),
			new Planet("Mars", "Red", 3389, 227940, 687 * 24, 25, 24,
				new List<Moon>()
				{
					new Moon("Phobos", "Grey", 22 / 2, 9378 / 1000, 8, (int)(0.3 * 24),  2),
					new Moon("Deimos", "Off-white", 23 / 2, 9376 / 1000, 30, (int)(1.2 * 24), 1)
				}),
			new Planet("Jupiter", "Beige Red", 69911, 778330, 4333, 10, 13,
				new List<Moon>()
				{
					new Moon("Metris", "Grey", 22, 127969 / 1000, (int)(0.29 * 24), 0, 32),
					new Moon("Callisto", "Grey", 2400, 01883000 / 1000, (int)(16.7 * 24), (int)(16.7 * 24), 8),

				}),
			new Planet("Saturn", "Eggshell", 58232, 1429400, 10760, 11, 10,
				new List<Moon>()
				{
					new Moon("Pan", "Grey", 9655, 133583 / 1000, (int)(0.57 * 24), 0, 17)
				}),
			new Planet("Uranus", "Pale blue", 25362, 2870990, 30685, 17, 7,
				new List<Moon>()
				{
					new Moon("Ophelia", "Grey", 16, 53760 / 1000, (int)(0.376 * 24), 0, 10)
				}),
			new Planet("Neptune", "Blue", 24622, 4504300, 60190, 16, 5,
				new List<Moon>()
				{
					new Moon("Naiad", "Grey", 29, 48000 / 1000, (int)(0.29 * 24), 0, 12)
				}),
			new DwarfPlanet("Pluto", "Steel Red", 1188, 5913520, 90550, 6387 * 24, 4743, null),
			new AsteroidBelt("The asteroid belt", "Grey Rock", 69420),
			new Asteroid("243 Ida", "Grey", 16, 4280, 1767, 5, 420, null)
		};

		Console.Write("Please enter time: ");
		String string_time = Console.ReadLine();
		Console.Write("Please enter planet name: ");
		String planet_name = Console.ReadLine();
		int time = convert_time(string_time);


		bool found = false;
		SpaceObject space_object = sun;
		foreach(SpaceObject obj in solarSystem)
		{
			if(obj.name == planet_name)
			{
				space_object = obj;
				found = true;
			}
		}
		if(found == true)
		{
			Console.WriteLine(space_object.toString());
			space_object.updatePosition(time, solarSystem[0]);
			Console.WriteLine(String.Format("x: {0}, y: {1}", space_object.position[0], space_object.position[1]));
		}

		Console.ReadLine();
	}

	public static int convert_time(String time)
	{
		try
		{
			return Int32.Parse(time);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);			
		}
		return 0;

	}
}