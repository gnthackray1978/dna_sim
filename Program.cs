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

        public string Sex { get; set; }

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
                Paternal.Add(new dna(origin) { value = rg.Next(1, 100) , Sex = "P"});
                idx++;
            }

            idx = 0;
            while (idx < size)
            {
                Maternal.Add(new dna(origin) { value = rg.Next(1, 100), Sex = "M" });
                idx++;
            }


        }

        public List<dna> Combined() {

            var r = new Random();
            int chrLength = Paternal.Count;

            int positionInChr = 0;

            int maxSize = 40;
            int minSize = 20;

            List<int> chunkList = new List<int>();

            while (positionInChr < chrLength)
            {
                
                //recombination initially can start from position zero
                
                int availableSpace = (chrLength - positionInChr);

                
                if (availableSpace >= minSize)
                {
                    int chrIndex = 0;

                    if (availableSpace > maxSize)
                        chrIndex = r.Next(minSize, maxSize);
                    else
                        chrIndex = r.Next(minSize, availableSpace);
                   
                    if (positionInChr >= (chrLength - minSize))
                    {
                        chrIndex = chrLength - positionInChr;
                    }
                  
                    chunkList.Add(chrIndex);

                    positionInChr += chrIndex;
                }
                else
                {
                    //if the available space left is less than the minimum size
                    //then redistribute it equally between the chunkS!!!!!!!!!!
                     
                    int partsCount = chunkList.Count;

                    int offset = chrLength - chunkList.Sum();

                    int rem = offset % partsCount;

                    int chunkAddition = (offset - rem) / partsCount;

                    chunkList = chunkList.Select(s => s + chunkAddition).ToList();

                    chunkList[0] += rem;

                    break;
                }


                
            }


            var newChromosome = new List<dna>();

            positionInChr = 0;
            bool isPaternal = false;
            foreach (var chunk in chunkList) {
                List<dna> newPart = new List<dna>();
                
                if(isPaternal)
                    newPart = Paternal.Skip(positionInChr).Take(chunk).ToList();
                else
                    newPart = Maternal.Skip(positionInChr).Take(chunk).ToList();

                isPaternal = !isPaternal;

                newChromosome.AddRange(newPart);
                positionInChr = chunk;
            }


            //Console.WriteLine(chunkList.Count + " " + string.Join(' ', chunkList));
            return newChromosome;
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

            var sarah = new Person("Sarah");

            var js_child = sarah.CreateChild(john);



            var albert = new Person("Albert");

            var victoria = new Person("Victoria");
            
            var av_child = victoria.CreateChild(albert);



            var js_av_gchild = av_child.CreateChild(js_child);

            foreach (var dna in js_av_gchild.One.Maternal)
            {
                Console.WriteLine(dna.Origin);
            }

            Console.WriteLine("Hello World!");
        }
    }
}
