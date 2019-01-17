using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgo
{
    class Simulation
    {
        public Random rand;
        public int PoolSize { get; set; }
        public int GenerationNum { get; set; }
        public double ProbabilityOfCrossover { get; set; }
        public int CrossoverPoints { get; set; }
        public double ProbabilityOfMutation { get; set; }
        public int ElitePercentage { get; set; }
        public Generation gen;
        public string EncryptedText { get; set; }
        public FrequenciesCalculation DefTrigramCounts { get; set; }

        public const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public Simulation(int poolSize, double crossover, int crossoverPoints, double mutation, int elitePercentage, string encryptedText)
        {
            GenerationNum = 1;
            rand = new Random();
            
            PoolSize = poolSize;
            ProbabilityOfCrossover = crossover;
            CrossoverPoints = crossoverPoints;
            ProbabilityOfMutation = mutation;
            ElitePercentage = elitePercentage;
            EncryptedText = encryptedText;

            gen = new Generation(EncryptedText, PoolSize);
        }

        public void SimulationStart()
        {
            FrequenciesCalculation frequenciesCalculation = new FrequenciesCalculation();
            FrequenciesCalculation.PopulateDefTrigramCounts();

            Pool pool1 = new Pool(PoolSize, EncryptedText);
            pool1.CreateInitialPool();
            Pool pool2 = new Pool(PoolSize, EncryptedText);
            pool2.CreateInitialPool();

            pool1.EnsureDistinctPool();
            pool2.EnsureDistinctPool();

            gen.CreateFirstGeneration(pool1, pool2);
            gen.EnsureDistinctPools();
            gen.GenerationSample[0].PossibleKeys.Sort(ComparatorForSorting);
            gen.GenerationSample[1].PossibleKeys.Sort(ComparatorForSorting);
        }

        public List<PossibleKey> Breed(PossibleKey key1, PossibleKey key2)
        {
            var childrens = new List<PossibleKey>();            
            var probOfCrossover = rand.NextDouble();
            var keyLength = key1.Key.Length;
            var childKey1 = new char[keyLength]; var childKey2 = new char[keyLength];
            // Crossover
            for (int i = 0; i < keyLength; i++)
            {
                childKey1[i] = key1.Key[i];
                childKey2[i] = key2.Key[i];
            }
            //if (probOfCrossover < ProbabilityOfCrossover)
            //{
            //    int randIndex = rand.Next(1, keyLength);
            //    for (int i = 0; i < randIndex; i++)
            //    {
            //        childKey1[i] = key1.Key[i];
            //        childKey2[i] = key2.Key[i];
            //    }
            //    for (int j = randIndex; j < keyLength; j++)
            //    {
            //        childKey1[j] = key2.Key[j];
            //        childKey2[j] = key1.Key[j];
            //    }

            //    var key1String = new String(childKey1);
            //    var key2String = new String(childKey2);
            //    var dupCharsKey1 = key1String.GroupBy(x => x)
            //                                .Where(x => x.Count() > 1)
            //                                .Select(x => x.Key);
            //    var dupCharsKey2 = key2String.GroupBy(x => x)
            //                                .Where(x => x.Count() > 1)
            //                                .Select(x => x.Key);
            //    var notInChild1 = ALPHABET.Except(childKey1).ToList();
            //    int cnt = 0;
            //    foreach (var dupChar in dupCharsKey1)
            //    {
            //        childKey1[key1String.IndexOf(dupChar)] = notInChild1[cnt];
            //        cnt++;
            //    }

            //    var notInChild2 = ALPHABET.Except(childKey2).ToList();
            //    cnt = 0;
            //    foreach (var dupChar in dupCharsKey2)
            //    {
            //        childKey2[key2String.IndexOf(dupChar)] = notInChild2[cnt];
            //        cnt++;
            //    }


            //    //var randPositions = new List<int>();
            //    //for(int i = 0; i < CrossoverPoints; i++)
            //    //{
            //    //    int index = -1;
            //    //    do
            //    //    {
            //    //        index = rand.Next(0, keyLength);
            //    //    }
            //    //    while (randPositions.Contains(index));
            //    //    randPositions.Add(index);
            //    //}

            //    //List<char> remainingCharacters1 = new List<char>(key1.Key.ToCharArray());
            //    //List<char> remainingCharacters2 = new List<char>(key2.Key.ToCharArray());

            //    //foreach (int point in randPositions)
            //    //{
            //    //    char c1 = key1.Key[point];
            //    //    char c2 = key2.Key[point];
            //    //    childKey1[point] = c1;
            //    //    childKey2[point] = c2;
            //    //    remainingCharacters1.Remove(c1);
            //    //    remainingCharacters2.Remove(c2);
            //    //}
            //    //int j = 0;
            //    //for (int i = 0; i < keyLength; i++)
            //    //{
            //    //    if (!randPositions.Contains(i))
            //    //    {
            //    //        childKey1[i] = remainingCharacters1[j];
            //    //        childKey2[i] = remainingCharacters2[j];
            //    //        j++;
            //    //    }
            //    //}
            //}
            //else
            //{
            //    for (int i = 0; i < keyLength; i++)
            //    {
            //        childKey1[i] = key1.Key[i];
            //        childKey2[i] = key2.Key[i];
            //    }
            //}

            // Mutation
            var probOfKey1Mutate = rand.NextDouble();
            var probOfKey2Mutate = rand.NextDouble();
            if (probOfKey1Mutate < ProbabilityOfMutation)
            {
                int oldPosition = rand.Next(0, keyLength);
                int newPosition = -1;
                do
                {
                    newPosition = rand.Next(0, keyLength);
                } while (oldPosition == newPosition);
                char oldChar = childKey1[oldPosition];
                childKey1[oldPosition] = childKey1[newPosition];
                childKey1[newPosition] = oldChar;
            }

            if(probOfKey2Mutate < ProbabilityOfMutation)
            {
                int oldPosition = rand.Next(0, keyLength);
                int newPosition = -1;
                do
                {
                    newPosition = rand.Next(0, keyLength);
                } while (oldPosition == newPosition);
                char oldChar = childKey2[oldPosition];
                childKey2[oldPosition] = childKey2[newPosition];
                childKey2[newPosition] = oldChar;
            }

            childrens.Add(new PossibleKey(new String(childKey1), EncryptedText));
            childrens.Add(new PossibleKey(new String(childKey2), EncryptedText));

            return childrens;
        }

        private int ComparatorForSorting(PossibleKey key1, PossibleKey key2)
        {
            return -key1.Fitness.CompareTo(key2.Fitness);
        }

        public void SimulationStep()
        {
            int eliteAmount = (int)Math.Ceiling((2 * PoolSize * ((double)ElitePercentage / 100)));
            int childrenToMake = (2 * PoolSize) - eliteAmount;

            var keysOfPool1 = gen.GenerationSample[0].PossibleKeys;
            var keysOfPool2 = gen.GenerationSample[1].PossibleKeys;

            //gen.GenerationSample[0].PossibleKeys.Clear();
            //gen.GenerationSample[1].PossibleKeys.Clear();
            int j = 0, k = 0;
            Generation nextGen = new Generation(EncryptedText, PoolSize);
            nextGen.CreateFirstGeneration(new Pool(PoolSize, EncryptedText), new Pool(PoolSize, EncryptedText));
            if (eliteAmount > 0)
            {
                for(int i = 0; i < eliteAmount; i++)
                {
                    if (i % 2 == 0)
                    {
                        nextGen.GenerationSample[0].PossibleKeys.Add(keysOfPool1[j]);
                        j++;
                    }
                    else
                    {
                        nextGen.GenerationSample[1].PossibleKeys.Add(keysOfPool2[k]);
                        k++;
                    }
                }
            }

            int elite1 = j;
            int elite2 = k;

            if(childrenToMake > 0)
            {
                var allChildren = new List<PossibleKey>();

                for(int m = 0; m < elite1; m++)
                {
                    for (int n = 0; n < elite2; n++)
                    {
                        allChildren.AddRange(Breed(keysOfPool1[m], keysOfPool2[n]));
                    }
                }

                for (int n = 0; n < elite2; n++)
                {
                    for (int m = 0; m < elite1; m++)
                    {
                        allChildren.AddRange(Breed(keysOfPool1[n], keysOfPool2[m]));
                    }
                }

                for(int i = 0; i < allChildren.Count; i++)
                {
                    if (nextGen.GenerationSample[0].PossibleKeys.Count + nextGen.GenerationSample[1].PossibleKeys.Count < 2 * PoolSize)
                    {
                        if (i % 2 == 0)
                            nextGen.GenerationSample[0].PossibleKeys.Add(allChildren[i]);
                        else
                            nextGen.GenerationSample[1].PossibleKeys.Add(allChildren[i]);
                    }
                }

                int moreToAdd = (2 * PoolSize) - (nextGen.GenerationSample[0].PossibleKeys.Count + nextGen.GenerationSample[1].PossibleKeys.Count);
                allChildren.Clear();
                
                for(int i = j; i < PoolSize; i++)
                {
                    if (moreToAdd <= allChildren.Count)
                        break;
                    allChildren.AddRange(Breed(keysOfPool1[i], keysOfPool2[i]));
                }
                int counter = 0;
                while(moreToAdd > 0)
                {
                    if (nextGen.GenerationSample[0].PossibleKeys.Count > nextGen.GenerationSample[1].PossibleKeys.Count)
                    {
                        nextGen.GenerationSample[1].PossibleKeys.Add(allChildren[counter]);
                        counter++;
                    }
                    else
                    {
                        nextGen.GenerationSample[0].PossibleKeys.Add(allChildren[counter]);
                        counter++;
                    }
                    moreToAdd--;
                }
            }
            nextGen.GenerationSample[0].EnsureDistinctPool();
            nextGen.GenerationSample[1].EnsureDistinctPool();
            nextGen.EnsureDistinctPools();
            nextGen.GenerationSample[0].PossibleKeys.Sort(ComparatorForSorting);
            nextGen.GenerationSample[1].PossibleKeys.Sort(ComparatorForSorting);
            gen = nextGen;
            GenerationNum++;
        }

        public void AutoSimulation()
        {
            SimulationStart();

            do
            {
                int nextGens = (int)(0.3 * GenerationNum);
                if (nextGens > 120) {
                    double currentBestFitness = gen.GetBestKey().Fitness;
                    double tempBestFitness = 0;
                    for (int i = 0; i < nextGens; i++)
                    {
                        tempBestFitness = gen.GetBestKey().Fitness;
                        SimulationStep();
                    }

                    if (tempBestFitness <= currentBestFitness)
                    {
                        break;
                    }
                }
                SimulationStep();

            } while (true);
        }

        public void GetLastGenInfo()
        {
            Console.WriteLine($"Generation - {GenerationNum}\n" +
                $"Best Key - {gen.GetBestKey().Key}\n" +
                $"Best Fitness - {gen.GetBestKey().Fitness}\n" +
                $"Decoded text:\n{gen.GetBestKey().DecodeByKey(EncryptedText)}");
        }
    }
}
