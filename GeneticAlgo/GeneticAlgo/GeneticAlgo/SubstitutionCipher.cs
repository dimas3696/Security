using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgo
{
    class SubstitutionCipher
    {
        //public const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        //public string GenerateRandomKey()
        //{
        //    Random rand = new Random();
        //    char[] key = ALPHABET.ToCharArray();
        //    for (int i = 0; i < key.Length; i++)
        //    {
        //        char c = key[i];
        //        int shuffleIndex = rand.Next(key.Length - i) + i;
        //        key[i] = key[shuffleIndex];
        //        key[shuffleIndex] = c;
        //    }
        //    return new String(key);
        //}

        //public string Encrypt(string text, string key)
        //{
        //    string normalizedText = text.Trim().ToUpper();
        //    string encryptedText = "";

        //    foreach (char c in normalizedText)
        //    {
        //        if (Char.IsLetter(c))
        //            encryptedText += key[(int)(c - 'A')];
        //        else
        //            encryptedText += c;
        //    }

        //    return encryptedText;
        //}

        public string Decrypt(string text, string key)
        {
            string normalizedText = text.Trim().ToUpper();
            string decryptedText = "";

            Dictionary<char, int> indexInKey = new Dictionary<char, int>();
            for (int i = 0; i < key.Length; i++)
                indexInKey[key[i]] = i;

            foreach (char c in normalizedText)
            {
                decryptedText += (char)('A' + indexInKey[c]);
            }

            return decryptedText;
        }
    }
}
