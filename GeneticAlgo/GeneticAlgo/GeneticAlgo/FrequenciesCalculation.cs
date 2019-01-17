using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgo
{
    class FrequenciesCalculation
    {
        static public Dictionary<string, int> DefTrigramsCounts;

        public FrequenciesCalculation()
        {
            DefTrigramsCounts = new Dictionary<string, int>();
            //DefTrigramsCounts = getDefTrigramCounts();
        }

        static public void PopulateDefTrigramCounts()
        {
            //var resDict = new Dictionary<string, int>();
            string singleFreq = "";
            var trigramsCounts = Properties.Resources.english_trigrams;
            var trigramsReader = new StringReader(trigramsCounts);
            while (true)
            {
                singleFreq = trigramsReader.ReadLine();
                if (singleFreq != null)
                {
                    var line = singleFreq.Split(' ');
                    string trigram = line[0].ToUpper();
                    int count = Convert.ToInt32(line[1]);
                    DefTrigramsCounts.Add(trigram, count);
                }
                else
                    break;
            }

            //return DefTrigramsCounts;
        }

        static public Dictionary<string, int> getTrigramCounts(string decryptedText)
        {
            int gram = 3;
            var resDict = new Dictionary<string, int>();
            for(int i = 0; i < decryptedText.Length; i++)
            {
                if (i + gram >= decryptedText.Length)
                    break;
                int currentCount;
                var singleGram = decryptedText.Substring(i, gram);
                if (singleGram.Length == gram)
                {
                    if (resDict.TryGetValue(singleGram, out currentCount))
                        resDict[singleGram] = currentCount + 1;
                    else
                        resDict[singleGram] = 1;
                }
                else break;
            }

            return resDict;
        }

        static public int getCount(string trigram)
        {
            int count;
            if (FrequenciesCalculation.DefTrigramsCounts.TryGetValue(trigram, out count))
                return count;
            else
                return 0;
        }
    }
}
