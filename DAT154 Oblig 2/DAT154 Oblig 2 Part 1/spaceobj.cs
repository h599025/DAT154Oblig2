using System;
using System.Collections.Generic;

namespace SpaceSim
{
    public class SpaceObject
    {
        public String name { get; set; }
        public String color { get; set; }
        public int radius { get; set; } //represented i km
        public double[] position { get; set; } = new double[2];


        public SpaceObject(String name, String color, int radius)
        {
            this.name = name;
            this.color = color;
            this.radius = radius;
        }
        public virtual void Draw()
        {
            Console.WriteLine(name);
        }
        public virtual String toString()
        {
            return String.Format("Name: {0}, Color: {1}, Radius {2} ", this.name, this.color, this.radius);
        }
        public virtual double[] updatePosition(int time, SpaceObject spaceObject)
        {
            return this.position;
        }
    }

    public class Star : SpaceObject
    {
        public int orbitalRadius { get; set; } //represented in (000 km)
        public int orbitalPeriod { get; set; } //represented in hours
        public int rotationalPeriod { get; set; } // represented in hours

        public Star(String name, String color, int radius, int orbitalRadius, int orbitalPeriod, int rotationalPeriod) : base(name, color, radius)
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
            return base.toString();
        }
    }

    public class Planet : SpaceObject
    {
        public int orbitalRadius { get; set; } // represented in (000 km)
        public int orbitalPeriod { get; set; } //represented in hours
        public int orbitalSpeed { get; set; } // represented in km/s
        public int rotationalPeriod { get; set; } // represented in hours
        public List<Moon> moonList { get; private set; }


        public Planet(String name, String color, int radius, int orbitalRadius, int orbitalPeriod,
            int rotationalPeriod, int orbitalSpeed, List<Moon> moonList) : base(name, color, radius)
        {
            this.orbitalRadius = orbitalRadius;
            this.orbitalPeriod = orbitalPeriod;
            this.orbitalSpeed = orbitalSpeed;
            this.rotationalPeriod = rotationalPeriod;
            this.moonList = moonList;
        }
        public override void Draw()
        {
            Console.Write("Planet: ");
            base.Draw();
        }
        public override double[] updatePosition(int time, SpaceObject spaceObject)
        {
            double offsetX = 0;
            double offsetY = 0;

            if(spaceObject != null)
            {
                offsetX = spaceObject.position[0];
                offsetY = spaceObject.position[1];
            }

            this.position[0] = offsetX + Math.Cos(time * orbitalSpeed * 3.1416 / 180) * orbitalRadius;
            this.position[1] = offsetY + Math.Sin(time * orbitalSpeed * 3.1416 / 180) * orbitalRadius;

            if(moonList != null)
            {
                foreach (Moon moon in this.moonList)
                {
                    moon.updatePosition(time, this);
                }
            }
            return this.position;

        }

    }

    public class Moon : Planet
    {
        public Moon(String name, String color, int radius, int orbitalRadius, int orbitalPeriod, int rotationalPeriod, int orbitalSpeed, List<Moon> moonList=null)
            : base(name, color, radius, orbitalRadius, orbitalPeriod, rotationalPeriod, orbitalSpeed, moonList) { }
        public override void Draw()
        {
            Console.Write("Moon : ");
            base.Draw();
        }

    }
    public class DwarfPlanet : Planet
    {
        public DwarfPlanet(String name, String color, int radius, int orbitalRadius, int orbitalPeriod, int rotationalPeriod, int orbitalSpeed, List<Moon> moonList)
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

        public Comet(String name, String color, int radius) : base(name, color, radius)
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
        public AsteroidBelt(String name, String color, int radius) : base(name, color, radius) { }
        public override void Draw()
        {
            Console.Write("Asteroid belt : ");
            base.Draw();
        }
    }
    public class Asteroid : Planet
    {
        public Asteroid(String name, String color, int radius, int orbitalRadius, int orbitalPeriod, int rotationalPeriod, int orbitalSpeed, List<Moon> moonList)
            : base(name, color, radius, orbitalRadius, orbitalPeriod, rotationalPeriod, orbitalSpeed, moonList) { }
        public override void Draw()
        {
            Console.Write("Asteroid : ");
            base.Draw();
        }
    }
}

