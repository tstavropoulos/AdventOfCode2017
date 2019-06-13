using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input20.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 20");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            int id = 0;

            List<Particle> particles = File.ReadAllLines(inputFile)
                .Select(x => new Particle(id++, x))
                .ToList();

            long minAcceleration = particles.Select(x => x.a.Magnitude).Min();

            IEnumerable<Particle> lowAParticles = particles.Where(x => x.a.Magnitude == minAcceleration);

            long minVelocity = lowAParticles.Select(x => x.v.Magnitude).Min();

            IEnumerable<Particle> lowVParticles = lowAParticles.Where(x => x.v.Magnitude == minVelocity);

            if (lowVParticles.Count() == 1)
            {
                Console.WriteLine(lowVParticles.First());
                Console.WriteLine($"The long-term closes particle is: {lowVParticles.First().id}");
            }
            else
            {
                Console.WriteLine($"After two searches we are down to {lowVParticles.Count()} Particles");

                Console.WriteLine("Printing:");

                foreach (Particle particle in lowVParticles.Take(100))
                {
                    Console.WriteLine(particle);
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            //Now we begin Advancing
            Dictionary<Vector3, Particle> positions = new Dictionary<Vector3, Particle>();
            HashSet<Particle> destroyedParticles = new HashSet<Particle>();

            int destroyedCount = 0;
            int stepMax = 100;
            int step = 0;

            while (true)
            {
                foreach (Particle particle in particles)
                {
                    if (positions.ContainsKey(particle.p))
                    {
                        destroyedParticles.Add(particle);
                        destroyedParticles.Add(positions[particle.p]);
                    }
                    else
                    {
                        positions.Add(particle.p, particle);
                    }
                }

                destroyedCount += destroyedParticles.Count;

                //Kill collided particles
                particles.RemoveAll(x => destroyedParticles.Contains(x));

                destroyedParticles.Clear();
                positions.Clear();

                if (step++ > stepMax)
                {
                    break;
                }

                //Advance
                particles.ForEach(x => x.Advance());

            }

            Console.WriteLine($"Destroyed {destroyedCount} particles after {stepMax} steps");
            //Damn, I am disappointed that this was sufficient to solve it...
            Console.WriteLine($"Reamining particles: {particles.Count}");

            Console.WriteLine("");
            Console.ReadKey();
        }
    }

    public class Particle
    {
        public Vector3 p;
        public Vector3 v;
        public readonly Vector3 a;

        public readonly int id;

        public Particle(int id, string particle)
        {
            this.id = id;

            string[] splitString = particle.Split(", ");

            p = new Vector3(splitString[0].Substring(2));
            v = new Vector3(splitString[1].Substring(2));
            a = new Vector3(splitString[2].Substring(2));
        }

        public Particle(int id, in Vector3 p, in Vector3 v, in Vector3 a)
        {
            this.id = id;

            this.p = p;
            this.v = v;
            this.a = a;
        }

        public void Advance()
        {
            v += a;
            p += v;
        }

        public long Distance => p.Magnitude;
        public override string ToString() => $"{id,5}) p={p}, v={v}, a={a}";
    }

    public readonly struct Vector3
    {
        public readonly long x;
        public readonly long y;
        public readonly long z;

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 XAxis = new Vector3(1, 0, 0);
        public static readonly Vector3 YAxis = new Vector3(0, 1, 0);
        public static readonly Vector3 ZAxis = new Vector3(0, 0, 1);

        private static readonly char[] SplitCharacters = new char[] { '<', '>', ',' };

        /// <summary>
        /// Manhattan Distance
        /// </summary>
        public long Magnitude => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

        public Vector3(long x, long y, long z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Of the form "<#,#,#>"
        /// </summary>
        public Vector3(string vector)
        {
            string[] splitString = vector.Split(SplitCharacters, StringSplitOptions.RemoveEmptyEntries);

            x = long.Parse(splitString[0]);
            y = long.Parse(splitString[1]);
            z = long.Parse(splitString[2]);
        }

        public override string ToString() => $"<{x},{y},{z}>";

        public override bool Equals(object obj) => (obj is Vector3 vector) && Equals(vector);

        public bool Equals(in Vector3 value) =>
            x == value.x && y == value.y && z == value.z;
        public override int GetHashCode() => HashCode.Combine(x, y, z);

        public static Vector3 operator +(in Vector3 left, in Vector3 right) =>
            new Vector3(left.x + right.x, left.y + right.y, left.z + right.z);

        public static Vector3 operator -(in Vector3 left, in Vector3 right) =>
            new Vector3(left.x - right.x, left.y - right.y, left.z - right.z);

        public static Vector3 operator *(in Vector3 left, in Vector3 right) =>
            new Vector3(left.x * right.x, left.y * right.y, left.z * right.z);

        public static Vector3 operator *(in Vector3 left, long right) =>
            new Vector3(left.x * right, left.y * right, left.z * right);

        public static Vector3 operator *(long left, in Vector3 right) =>
            new Vector3(left * right.x, left * right.y, left * right.z);

        public static bool operator ==(in Vector3 left, in Vector3 right) =>
            left.Equals(right);

        public static bool operator !=(in Vector3 left, in Vector3 right) =>
            !left.Equals(right);
    }
}
