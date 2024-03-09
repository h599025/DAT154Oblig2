using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceSim
{
    struct ObjectColor
    {
        int RED;
        int GREEN;
        int BLUE;
    }
    public class SpaceObject
    {
        public String name { get; set; }
        public String color { get; set; }
        public int radius { get; set; } //represented i km
        public double[] position { get; set; } = new double[2];
        public List<SpaceObject> moonList { get; private set; }


        public SpaceObject(String name, String color, int radius, List<SpaceObject> moonList)
        {
            this.name = name;
            this.color = color;
            this.radius = radius;
            this.moonList = moonList;
        }
        public virtual void Draw()
        {
            Console.WriteLine(name);
        }
        public virtual String toString()
        {
            return String.Format("Name: {0}, Color: {1}, Radius {2} ", this.name, this.color, this.radius);
        }
        public virtual void updatePosition(double time, SpaceObject spaceObject)
        {
        }
    }

    public class Star : SpaceObject
    {
        public int orbitalRadius { get; set; } //represented in (000 km)
        public int orbitalPeriod { get; set; } //represented in hours
        public int rotationalPeriod { get; set; } // represented in hours

        public Star(String name, String color, int radius, int orbitalRadius,
            int orbitalPeriod, int rotationalPeriod, List<SpaceObject> moonList) : base(name, color, radius, moonList)
        {
            this.orbitalRadius = orbitalRadius;
            this.orbitalPeriod = orbitalPeriod;
            this.rotationalPeriod = rotationalPeriod;
        }
        public override void Draw()
        {
            Console.Write("Star : ");
            base.Draw();
        }
        public override string toString()
        {
            return String.Format(
                                "Stjerne Informasjon: \n Navn: {0} \n Radius: {1} \n Rotasjonsperiode: \n {2} ",
                                this.name,
                                this.radius,
                                this.rotationalPeriod);
                              
        }
    }

    public class Planet : SpaceObject
    {
        public int orbitalRadius { get; set; } // represented in (000 km)
        public int orbitalPeriod { get; set; } //represented in hours
        public int orbitalSpeed { get; set; } // represented in km/s
        public int rotationalPeriod { get; set; } // represented in hours


        public Planet(String name, String color, int radius, int orbitalRadius, int orbitalPeriod,
            int rotationalPeriod, int orbitalSpeed, List<SpaceObject> moonList) : base(name, color, radius, moonList)
        {
            this.orbitalRadius = orbitalRadius;
            this.orbitalPeriod = orbitalPeriod;
            this.orbitalSpeed = orbitalSpeed;
            this.rotationalPeriod = rotationalPeriod;
        }
        public override void Draw()
        {
            Console.Write("Planet: ");
            base.Draw();
        }
        public override void updatePosition(double time, SpaceObject spaceObject)
        {
            double offsetX = 0;
            double offsetY = 0;
            
            if (spaceObject != null)
            {
                offsetX = spaceObject.position[0] + spaceObject.radius;
                offsetY = spaceObject.position[1] + spaceObject.radius;
            }
            

            this.position[0] =  Math.Cos(time * orbitalSpeed * 3.1416 / 180) * (offsetX + orbitalRadius);
            this.position[1] =  Math.Sin(time * orbitalSpeed * 3.1416 / 180) * (offsetY + orbitalRadius);
            
            if (moonList != null)
            {
                foreach (SpaceObject moon in this.moonList)
                {
                    moon.updatePosition(time, this);
                }
            }
        }

        public override string toString()
        {
            return String.Format(
                                "Planet Informasjon: \n Navn: {0} " +
                                "\n Radius: {1} " +
                                "\n Orbitalradius: {2} " +
                                "\n Orbitalfart: {3} " +
                                "\n Orbitalperiode: {4}" +
                                "\n Måner: \n {5} ",
                                this.name,
                                this.radius,
                                this.orbitalRadius,
                                this.orbitalSpeed,
                                this.orbitalPeriod,
                                String.Join("\n ", this.moonList.Select(x => x.name.ToString())));
        }


    }

    public class Moon : Planet
    {
        public Moon(String name, String color, int radius, int orbitalRadius, int orbitalPeriod, int rotationalPeriod, int orbitalSpeed, List<SpaceObject> moonList = null)
            : base(name, color, radius, orbitalRadius, orbitalPeriod, rotationalPeriod, orbitalSpeed, moonList) { }
        public override void Draw()
        {
            Console.Write("Moon : ");
            base.Draw();
        }
        public override void updatePosition(double time, SpaceObject spaceObject)
        {
            double offsetX = 0;
            double offsetY = 0;
            
            if (spaceObject != null)
            {
                offsetX = spaceObject.radius * 10;
                offsetY = spaceObject.radius * 10;
            }
            

            this.position[0] = Math.Cos(time * orbitalSpeed * 3.1416 / 180) * (offsetX + orbitalRadius);
            this.position[1] = Math.Sin(time * orbitalSpeed * 3.1416 / 180) * (offsetY + orbitalRadius);

            if (moonList != null)
            {
                foreach (SpaceObject moon in this.moonList)
                {
                    moon.updatePosition(time, this);
                }
            }
        }

    }
    public class DwarfPlanet : Planet
    {
        public DwarfPlanet(String name, String color, int radius, int orbitalRadius, int orbitalPeriod, int rotationalPeriod, int orbitalSpeed, List<SpaceObject> moonList)
            : base(name, color, radius, orbitalRadius, orbitalPeriod, rotationalPeriod, orbitalSpeed, moonList) { }
        public override void Draw()
        {
            Console.Write("Dwarf planet : ");
            base.Draw();
        }
    }


    public class Comet : SpaceObject
    {
        public int orbitalRadius { get; set; }
        public int orbitalPeriod { get; set; }
        public int rotationalPeriod { get; set; }

        public Comet(String name, String color, int radius, List<SpaceObject> moonList = null) : base(name, color, radius, moonList)
        {
            this.orbitalRadius = orbitalRadius;
            this.orbitalPeriod = orbitalPeriod;
            this.rotationalPeriod = rotationalPeriod;
        }
        public override void Draw()
        {
            Console.Write("Comet: ");
            base.Draw();
        }

    }
    public class AsteroidBelt : SpaceObject
    {
        public AsteroidBelt(String name, String color, int radius, List<SpaceObject> moonList = null) : base(name, color, radius, moonList) { }
        public override void Draw()
        {
            Console.Write("Asteroid belt : ");
            base.Draw();
        }

    }
    public class Asteroid : Planet
    {
        public Asteroid(String name, String color, int radius, int orbitalRadius, int orbitalPeriod, int rotationalPeriod, int orbitalSpeed, List<SpaceObject> moonList)
            : base(name, color, radius, orbitalRadius, orbitalPeriod, rotationalPeriod, orbitalSpeed, moonList) { }
        public override void Draw()
        {
            Console.Write("Asteroid : ");
            base.Draw();
        }
    }
}