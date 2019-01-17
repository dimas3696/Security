using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgo
{
    class Pool
    {
        public List<PossibleKey> PossibleKeys { get; set; }
        public int PoolSize { get; set; }
        const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static Random rand = new Random();
        public string EncryptedText { get; set; }

        public Pool(){ }

        public Pool(int poolSize, string encryptedText)
        {
            PoolSize = poolSize;
            EncryptedText = encryptedText;
            PossibleKeys = new List<PossibleKey>();
        }

        public string CreateRandKey()
        {
            char[] key = ALPHABET.ToCharArray();
            for (int i = 0; i < key.Length; i++)
            {
                char c = key[i];
                int shuffleIndex = rand.Next(key.Length - i) + i;
                key[i] = key[shuffleIndex];
                key[shuffleIndex] = c;
            }
            return new String(key);
        }

        public void CreateInitialPool()
        {
            for(int i = 0; i < PoolSize; i++)
            {
                PossibleKeys.Add(new PossibleKey(CreateRandKey(), EncryptedText));
            }
        }

        public void EnsureDistinctPool()
        {
            var isDistinct = false;
            do
            {
                var listOfKeys = PossibleKeys;
                var duplicateKeys = listOfKeys.GroupBy(x => x.Key)
                                                .Where(group => group.Count() > 1)
                                                .Select(group => group.Key);
                if (duplicateKeys.Count() != 0)
                {
                    foreach (var key in duplicateKeys)
                    {
                        var newKey = CreateRandKey();
                        listOfKeys[listOfKeys.FindIndex(ind => ind.Key.Equals(key))] = new PossibleKey(newKey, EncryptedText); 
                    }
                    PossibleKeys = listOfKeys;
                }
                else isDistinct = true;
            }
            while (!isDistinct);
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            foreach(var key in PossibleKeys)
            {
                res.Append(key.Key).AppendLine();
            }
            return res.ToString();
        }
    }
}
