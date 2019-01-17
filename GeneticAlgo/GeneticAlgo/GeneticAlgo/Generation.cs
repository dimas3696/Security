using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgo
{
    class Generation
    {
        public List<Pool> GenerationSample { get; set; }
        public string EncryptedText { get; set; }
        public int PoolSize { get; set; }

        public Generation(string encryptedText, int poolSize)
        {
            GenerationSample = new List<Pool>();
            //GenerationSample.Add(new Pool(poolSize, encryptedText, freqs));
            //GenerationSample.Add(new Pool(poolSize, encryptedText, freqs));
            //GenerationSample[1].PossibleKeys = new List<PossibleKey>();
            EncryptedText = encryptedText;
        }

        public Generation(Pool pool1, Pool pool2)
        {
            GenerationSample = new List<Pool>(2) { pool1, pool2 };
        }

        public void CreateFirstGeneration(Pool pool1, Pool pool2)
        {
            GenerationSample.Add(pool1);
            GenerationSample.Add(pool2);
        }

        public void EnsureDistinctPools()
        {
            var isCommonKeys = true;
            do
            {
                var pool1ForRand = GenerationSample[0];
                var pool2ForRand = GenerationSample[1];
                var pool1 = GenerationSample[0].PossibleKeys.Select(x => x.Key).ToList();
                var pool2 = GenerationSample[1].PossibleKeys.Select(x => x.Key).ToList();

                var intersection = pool1.Intersect(pool2);
                if (intersection.Count() != 0)
                {
                    foreach (var key in intersection)
                    {
                        var newKey = pool1ForRand.CreateRandKey();
                        pool1ForRand.PossibleKeys[pool1ForRand.PossibleKeys.FindIndex(ind => ind.Key.Equals(key))] = new PossibleKey(newKey, EncryptedText);
                    }
                    GenerationSample[0] = pool1ForRand;
                }
                else isCommonKeys = false;
            }
            while (isCommonKeys);
        }

        public PossibleKey GetBestKey()
        {
            return GenerationSample[0].PossibleKeys[0].Fitness > GenerationSample[1].PossibleKeys[0].Fitness ? GenerationSample[0].PossibleKeys[0] : GenerationSample[1].PossibleKeys[0];
        }

        public override string ToString()
        {
            int i = 1;
            StringBuilder res = new StringBuilder();
            foreach(var pool in GenerationSample)
            {
                res.Append($"Pool {i}:\n{pool.ToString()}").AppendLine();
                i++;
            }
            return res.ToString();
        }

    }
}
