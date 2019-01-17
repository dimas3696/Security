using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgo
{
    class PossibleKey
    {
        public string Key { get; set; }
        public double Fitness { get; set; }

        public PossibleKey(string key, string encryptedText)
        {
            Key = key;
            Fitness = CalculateFitness(encryptedText);
        }

        public double CalculateFitness(string cipherText)
        {
            double fitness = 0;
            SubstitutionCipher cipher = new SubstitutionCipher();
            string decrypted = cipher.Decrypt(cipherText, Key);
            //var trigramCounts = new FrequenciesCalculation();
            
            var decrTrigramCounts = FrequenciesCalculation.getTrigramCounts(decrypted);
            foreach (var trigram in decrTrigramCounts.Keys)
            {
                double defTrigramCount = FrequenciesCalculation.getCount(trigram);
                if (defTrigramCount != 0)
                    fitness += decrTrigramCounts[trigram] * Math.Log(defTrigramCount, 2.0);
            }

            return fitness;
        }

        public string DecodeByKey(string encriptedText)
        {
            SubstitutionCipher decoder = new SubstitutionCipher();
            return decoder.Decrypt(encriptedText, Key);
        }
    }
}
