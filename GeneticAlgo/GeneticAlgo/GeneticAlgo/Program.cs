using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GeneticAlgo
{
    class Program
    {
        public static int MyComparator(double a, double b)
        {
            return -a.CompareTo(b);
        }

        static void Main(string[] args)
        {
            Timer timer = new Timer(state =>
            {
                Console.WriteLine("Genetic Algorithm working...");
            }, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));

            string encodedStr = "EFFPQLEKVTVPCPYFLMVHQLUEWCNVWFYGHYTCETHQEKLPVMSAKSPVPAPVYWMVHQLUSPQLYWLASLFVWPQLMVHQLUPLRPSQLULQESPBLWPCSVRVWFLHLWFLWPUEWFYOTCMQYSLWOYWYETHQEKLPVMSAKSPVPAPVYWHEPPLUWSGYULEMQTLPPLUGUYOLWDTVSQETHQEKLPVPVSMTLEUPQEPCYAMEWWYOYULULTCYWPQLSEOLSVOHTLUYAPVWLYGDALSSVWDPQLNLCKCLRQEASPVILSLEUMQBQVMQCYAHUYKEKTCASLFPYFLMVHQLUHULIVYASHEUEDUEHQBVTTPQLVWFLRYGMYVWMVFLWMLSPVTTBYUNESESADDLSPVYWCYAMEWPUCPYFVIVFLPQLOLSSEDLVWHEUPSKCPQLWAOKLUYGMQEUEMPLUSVWENLCEWFEHHTCGULXALWMCEWETCSVSPYLEMQYGPQLOMEWCYAGVWFEBECPYASLQVDQLUYUFLUGULXALWMCSPEPVSPVMSBVPQPQVSPCHLYGMVHQLUPQLWLRPHEUEDUEHQMYWPEVWSSYOLHULPPCVWPLULSPVWDVWGYUOEPVYWEKYAPSYOLEFFVPVYWETULBEUF";

            //SubstitutionCipher cipher = new SubstitutionCipher();
            //Console.WriteLine(cipher.Decrypt(encodedStr, "EKMFLGDQVZJTOWYHXUSPCANRBI"));
            
            Simulation simulation = new Simulation(20, 0.65, 5, 0.2, 15, encodedStr);

            var dtStart = DateTime.Now; 
            simulation.AutoSimulation();
            var dtEnd = DateTime.Now;
            Console.WriteLine($"Time - {dtEnd - dtStart}");
            simulation.GetLastGenInfo();

            timer.Dispose();

            Console.ReadLine();
        }
    }
}
