using System;
using System.Collections.Generic;
using System.Linq;

namespace dna_sim
{
    public class dna {

        public dna(string origin) {
            Origin = origin;
        }

        public int value { get; set; }
        public string Origin { get; set; }

        public List<string> holderHistory { get; set; }

    }

    class Chromo {

        private const int recombinationCount = 3;

        public List<dna> Paternal { get; set; }
        public List<dna> Maternal { get; set; }

        public void Make(string origin) {

            Paternal = new List<dna>();
            Maternal = new List<dna>();
            var rg = new Random();
            int size = 100;

            int idx = 0;
            while (idx < size)
            {
                Paternal.Add(new dna(origin) { value = rg.Next(1, 100) });
                idx++;
            }

            idx = 0;
            while (idx < size)
            {
                Maternal.Add(new dna(origin) { value = rg.Next(1, 100) });
                idx++;
            }


        }

        public List<dna> Combined() {

            var r = new Random();

            int position = 0;

            int maxSize = 40;
            int minSize = 20;

            List<List<dna>> results = new List<List<dna>>();
            List<int> positionList = new List<int>();

            while (position < Paternal.Count)
            {
                int first = minSize;
                //recombination initially can start from position zero
                
                int availableSpace = (Paternal.Count - position);

                //problem is we dont want to end up with tiny chunks!!! 

                if (availableSpace >= minSize)
                {
                    //

                    if(availableSpace > maxSize)
                        first = r.Next(minSize, maxSize);
                    else
                        first = r.Next(minSize, availableSpace);



                    if (first > maxSize)
                    {
                        
                        first = maxSize;
                    }

                    //  if (minSize > rand) rand = minSize;



                    List<dna> result = new List<dna>();
                    //tracker = position.

                    if (position >= (Paternal.Count - minSize))
                    {
                        result = Paternal.Skip(position).ToList();
                    }
                    else
                    {

                        result = Paternal.Skip(position).Take(first).ToList();
                    }

                    

                    results.Add(result);

                    positionList.Add(result.Count);

                    //if (positionList.Count > 2)
                   //     Console.WriteLine("normal distribution: " + positionList[0] + " " + positionList[1]);

                    position += first;
                }
                else
                {
                    //if the available space left is less than the minimum size
                    //then redistribute it equally between the chunkS!!!!!!!!!!

                    int sum = positionList.Sum();
                    int partsCount = positionList.Count;

                    int offset = Paternal.Count - sum;

                    int rem = offset % partsCount;



                    offset = offset - rem;

                    int chunkAddition = offset / partsCount;

                    positionList = positionList.Select(s => s + chunkAddition).ToList();

                    positionList[0] += rem;

                       


                    break;
                }


                
            }

            Console.WriteLine(positionList.Count + " " + string.Join(' ', positionList));
            return new List<dna>();
        }
    }

    class Person {
        public Person(string origin) {
           One.Make(origin);
        }
        public Person()
        {
        }

        public string Name { get; set; }
        public Chromo One { get; set; } = new Chromo();

        public Person CreateChild(Person father){

            var child = new Person();

            child.One.Maternal = One.Combined();
            child.One.Paternal = father.One.Combined();

            return child;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var john = new Person("John");

            //var sarah = new Person("Sarah");

            //var child = sarah.CreateChild(john);

            int idx = 0;

            while (idx < 2000)
            {
                john.One.Combined();
                idx++;
            }
           

            Console.WriteLine("Hello World!");
        }
    }
}
