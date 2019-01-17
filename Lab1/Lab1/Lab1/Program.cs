using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections.Generic;
using Accord.Math;
using ConsoleTables;

namespace Lab1
{
    class Program
    {
        public static void GetSymbolFrequency(string s)
        {
            var freqs = s.GroupBy(c => c).Select(g => new { Symbol = g.Key, Count = g.Count() }).OrderByDescending(b => b.Count);

            foreach(var item in freqs)
            {
                Console.WriteLine($"Symbol: {item.Symbol}, Count: {item.Count}");
            }
        }

        public static Dictionary<byte, int> GetByteFrequency(List<byte> bytearr)
        {
            var freqs = bytearr.GroupBy(c => c)
                        .Select(g => new { Symbol = g.Key, Count = g.Count() })
                        .OrderByDescending(b => b.Count)
                        .ToDictionary(c => c.Symbol, c => c.Count);
            //foreach (var item in freqs)
            //{
            //    Console.WriteLine($"Symbol: {item.Key}, Count: {item.Value}");
            //}
            return freqs;
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private static byte[] lookup =
        {
            0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4,
            1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5,
            1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5,
            2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6,
            1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5,
            2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6,
            2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6,
            3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7,
            1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5,
            2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6,
            2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6,
            3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7,
            2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6,
            3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7,
            3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7,
            4, 5, 5, 6, 5, 6, 6, 7, 5, 6, 6, 7, 6, 7, 7, 8,
        };

        public static double BitwiseHamming(List<byte> x, List<byte> y)
        {
            int d = 0;
            for(int i = 0; i < x.Count; i++)
            {
                byte xor = (byte)(x[i] ^ y[i]);
                d += lookup[xor];
            }
            return d;
        }

        public static List<byte> BytePartition(List<byte> bytearr, int index, int keyLength)
        {
            var resArr = new List<byte>();
            for (int i = index; i < bytearr.Count; i += keyLength)
            {
                resArr.Add(bytearr[i]);
            }
            return resArr;
        }

        public static byte DecodeRepeatingXOR(List<byte> bytearr)
        {
            var topSymbols = bytearr.Take(6).ToList();
            var letters = new char[] { 'e', 't', 'a', 'o', 'i', 'n' };
            var letterBytes = letters.Select(c => (byte)c).ToList();
            
            var listCounts = new List<byte>();
            for (int i = 0; i < letterBytes.Count; i++)
            {
                Console.WriteLine($"letter: {(char)letterBytes[i]}");
                for(int j = 0; j < topSymbols.Count; j++)
                {
                    var xor = topSymbols[j] ^ letterBytes[i];
                    listCounts.Add(Convert.ToByte(xor));
                    Console.Write($"\nSymbol: {topSymbols[j]}\t\tXOR: {xor}");
                }
                Console.WriteLine();
            }
            var probableKeySymbols = GetByteFrequency(listCounts);
            var table = new ConsoleTable("Symbol", "Count");
            foreach (var dict in probableKeySymbols)
                table.AddRow(Convert.ToChar(dict.Key), dict.Value);
            table.Write();
            return probableKeySymbols.First().Key;
        }

        //Task 0 function for decription
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        static void Main(string[] args)
        {
            //Task 0 (Decode task lab1)
            string encodedString = "VDJzc0lHRjBJR3hsWVhOMElHNXZkeUI1YjNVZ2MyVmxJSFJvWVhRZ2FYUW5jeUJoWW5OdmJIVjBaV3g1SUhCdmFXNTBiR1Z6Y3lCMGJ5QjBjbmtnZEc4Z2FXNTJaVzUwSUhsdmRYSWdiM2R1SUNKamNubHdkRzhpSUhkcGRHZ2daWGh3WldOMFlYUnBiMjV6SUhSb1lYUWdhV1lnYzI5dFpXOXVaU0JrYjJWemJpZDBJR3R1YjNjZ2RHaGxJR0ZzWjI5eWFYUm9iU0FvWlhabGJpQjBkMmxqWlNCMGFHVWdZbUZ6WlRZMExDQjNhR2xqYUNCcGN5QndjbVYwZEhrZ2MzUjFjR2xrS1NCcGRDZHpJR2x0Y0c5emMybGliR1VnWm05eUlHaHBiU0IwYnlCa1pXTnBjR2hsY2k0Z1dXOTFKM0psSUhKbFlXUnBibWNnZEdocGN5QnViM2NnYVc0Z2NHeGhhVzRnZEdWNGRDQnpieUIwYUdGMElHRnlaM1Z0Wlc1MElHTnNaV0Z5YkhrZ1ptRnNiSE1nYzJodmNuUXVDazV2ZHlCMGFHVWdZV04wZFdGc0lIUmhjMnR6T2dveExpQlhjbWwwWlNCaElIQnBaV05sSUc5bUlITnZablIzWVhKbElIUnZJR0YwZEdGamF5QnphVzVuYkdVdFlubDBaU0JZVDFJZ1kybHdhR1Z5SUhkb2FXTm9JR2x6SUhSb1pTQnpZVzFsSUdGeklFTmhaWE5oY2lCaWRYUWdkMmwwYUNCNGIzSWdiM0F1Q2pJdUlGMThaRE5uWVdvemNqTmhkbU4yY21kNmZYUStlSFpxTTB0Y1FUTndlbU43ZG1FOU0xWTlkRDB6ZW1jellIdDhaaTUzTTJkeWVIWXpjak5nWjJGNmZYUXpNWHQyTGk1OE0yUjhZUzUzTVROeWZYYy9NM1I2WlhaOU0yZDdkak40ZG1vemVtQXpNWGgyYWpFL00ydDhZVE5uZTNZemRYcGhZR2N6TG5ablozWmhNekY3TVROa2VtZDdNekY0TVQ4elozdDJmVE5yZkdFek1YWXhNMlI2WjNzek1YWXhQek5uZTNaOU16RXVNVE5rZW1kN016RnFNVDh6Y24xM00yZDdkbjB6YTN4aE0zMTJhMmN6Y0h0eVlUTXhMakV6WkhwbmV6TXhlREV6Y25SeWVuMC9NMmQ3ZG4wek1Yd3hNMlI2WjNzek1YWXhNM0o5ZHpOZ2ZETjhmVDB6U254bU0zNXlhak5tWUhZemVuMTNkbXN6ZkhVemNIeDZmWEI2ZDNaOWNIWS9NMXR5Zm41NmZYUXpkM3BnWjNKOWNIWS9NMWh5WUhwZ2VIb3pkbXR5Zm5wOWNtZDZmSDAvTTJCbmNtZDZZR2Q2Y0hJdU0yZDJZR2RnTTN4aE0yUjdjbWQyWlhaaE0zNTJaM3Q4ZHpOcWZHWXpkWFoyTGpOa2ZHWXVkek5nZTN4a00yZDdkak54ZG1Cbk0yRjJZR1l1WnowS015NGdNV00wTVRBeU0yWTFOalJpTW1FeE16QTRNalExTnpCbE5tSTBOekEwTm1JMU1qRm1NMlkxTWpBNE1qQXhNekU0TWpRMVpUQmxObUkwTURBeU1qWTBNekEzTW1VeE16RTRNMlUxTVRFNE0yWTFZVEZtTTJVME56QXlNalExWkRSaU1qZzFZVEZpTWpNMU5qRTVOalV4TXpObU1qUXhNekU1TW1VMU56RmxNamcxTmpSaU0yWTFZakJsTm1JMU1EQTBNalkwTXpBM01tVTBZakF5TTJZMFlUUmlNalExTlRSaU0yWTFZakF5TXpneE16QTBNalUxTmpSaU0yTTFOalJpTTJNMVlUQTNNamN4TXpGbE16ZzFOalJpTWpRMVpEQTNNekl4TXpGbE0ySTBNekJsTXprMU1EQmhNemcxTmpSaU1qYzFOakZtTTJZMU5qRTVNemd4WmpSaU16ZzFZelJpTTJZMVlqQmxObUkxT0RCbE16STBNREZpTW1FMU1EQmxObUkxWVRFNE5tSTFZekExTWpjMFlUUmlOemt3TlRSaE5tSTJOekEwTm1JMU5EQmxNMll4TXpGbU1qTTFZVEU0Tm1JMVl6QTFNbVV4TXpFNU1qSTFOREF6TTJZeE16QmhNMlUwTnpBME1qWTFNakZtTWpJMU1EQmhNamMxWmpFeU5tSTBZVEEwTTJVeE16RmpNakkxWmpBM05tSTBNekU1TWpRMU1UQmhNamsxWmpFeU5tSTFaREJsTW1VMU56UmlNMlkxWXpSaU0yVTBNREJsTm1JME1EQTBNalkxTmpSaU16ZzFZekU1TTJZeE16QTBNbVF4TXpCak1tVTFaREJsTTJZMVlUQTRObUkxTWpBM01tTTFZekU1TWpJME56QXpNall4TXpRek0yTTFZakF5TWpnMVlqUmlNMk0xWXpFNU1qQTFOakJtTm1JME56QXpNbVV4TXpBNU1tVTBNREZtTm1JMVpqQmhNemcwTnpSaU16STFOakJoTXpreFlUUTNObUkwTURBeU1qWTBOakEzTW1FME56QmxNbVl4TXpCaE1qVTFaREJsTW1FMVpqQXlNalUxTkRSaU1qUTBNVFJpTW1NME1UQmhNbVkxWVRCbE1qVTBOelJpTW1ZMU5qRTRNamcxTmpBMU0yWXhaRFJpTVRnMU5qRTVNakkxWXpGbE16ZzFaakV5TmpjeE16RmpNemsxWVRGbU1tVXhNekF5TTJZeE16RTVNakkxTkRBek0yWXhNekExTWpRME5EUTNObUkwWVRBME0yVXhNekZqTWpJMVpqQTNObUkxWkRCbE1tVTFOelJpTWpJME56UmlNMlkxWXpSaU1tWTFOakE0TWpJME16QXpNbVUwTVRSaU0yWTFZakJsTm1JMVpEQmxNek0wTnpSaU1qUTFaREJsTm1JMU1qRTRObUkwTkRCbE1qYzFaalExTm1JM01UQmxNbUUwTVRSaU1qSTFaRFJpTWpZMVlUQTFNbVl4WmpSaU0yWTFZakJsTXprMU5qZzVZMkpoWVRFNE5tSTFaREEwTm1JME1ERmlNbUUxTURCbE16Z3haRFl4Q2pRdUlFVkdSbEJSVEVWTFZsUldVRU5RV1VaTVRWWklVVXhWUlZkRFRsWlhSbGxIU0ZsVVEwVlVTRkZGUzB4UVZrMVRRVXRUVUZaUVFWQldXVmROVmtoUlRGVlRVRkZNV1ZkTVFWTk1SbFpYVUZGTVRWWklVVXhWVUV4U1VGTlJURlZNVVVWVFVFSk1WMUJEVTFaU1ZsZEdURWhNVjBaTVYxQlZSVmRHV1U5VVEwMVJXVk5NVjA5WlYxbEZWRWhSUlV0TVVGWk5VMEZMVTFCV1VFRlFWbGxYU0VWUVVFeFZWMU5IV1ZWTVJVMVJWRXhRVUV4VlIxVlpUMHhYUkZSV1UxRkZWRWhSUlV0TVVGWlFWbE5OVkV4RlZWQlJSVkJEV1VGTlJWZFhXVTlaVlV4VlRGUkRXVmRRVVV4VFJVOU1VMVpQU0ZSTVZWbEJVRlpYVEZsSFJFRk1VMU5XVjBSUVVVeE9URU5MUTB4U1VVVkJVMUJXU1V4VFRFVlZUVkZDVVZaTlVVTlpRVWhWV1V0RlMxUkRRVk5NUmxCWlJreE5Wa2hSVEZWSVZVeEpWbGxCVTBoRlZVVkVWVVZJVVVKV1ZGUlFVVXhXVjBaTVVsbEhUVmxXVjAxV1JreFhUVXhUVUZaVVZFSlpWVTVGVTBWVFFVUkVURk5RVmxsWFExbEJUVVZYVUZWRFVGbEdWa2xXUmt4UVVVeFBURk5UUlVSTVZsZElSVlZRVTB0RFVGRk1WMEZQUzB4VldVZE5VVVZWUlUxUVRGVlRWbGRGVGt4RFJWZEdSVWhJVkVOSFZVeFlRVXhYVFVORlYwVlVRMU5XVTFCWlRFVk5VVmxIVUZGTVQwMUZWME5aUVVkV1YwWkZRa1ZEVUZsQlUweFJWa1JSVEZWWlZVWk1WVWRWVEZoQlRGZE5RMU5RUlZCV1UxQldUVk5DVmxCUlVGRldVMUJEU0V4WlIwMVdTRkZNVlZCUlRGZE1VbEJJUlZWRlJGVkZTRkZOV1ZkUVJWWlhVMU5aVDB4SVZVeFFVRU5XVjFCTVZVeFRVRlpYUkZaWFIxbFZUMFZRVmxsWFJVdFpRVkJUV1U5TVJVWkdWbEJXV1ZkRlZGVk1Ra1ZWUmdvMUxpQk5WVXhFUTB4YVYwdE1XbFJYVkZoSVNrTllSVVJSVEVoTVdFbElRa3hNU2tSUlYwaEVVVlZhVTA5Q1EwdE5SVXRYV0ZKQlFsVkhRVVJSV2s5Q1RGRk9UbGhOU2xOWldFUlhXVnBHVGt0SlRrVlZTVU5PU2s1SVUxVlVWRTVSV2xSUlRrZEdRMWxaU2tOT1JVRk9UbGhJVGt0VFFVUlJXbFpFVTBWRVEweFNVMWRTV2xOQlVVMUdUVmhQU2xOWlEwWlRXVVZVVTFGYVUwSlBVMDVOVTBsV1RrTkNRMWRTV0ZaQ1IwbENRVWRMUmsxTVdsUlhWRUpDUmxWUFNrVlRTVWxDUjBWUVFsSk9RVXROV1ZSWlQxTkRWMVJhVTBKWlUxQkRRMXBHUTAxQ1FsaFVTVlZDVTFoVVRrMUZUbEpPUzBGRVVWSkdRMWxSV2xaTVMwWlZXbGxhVGs1S1drMVpTMDlPVlV0R1YwRkhSa05aV1VwVlExRk9UbGxUUTBWT1FsWkxUbFJUVjA5VFMwTk5Va2hHV1ZsS1MweFlTRVZEVTB0Q1drbEJTMHBUU0VwT1MxZE5RVmRHVTA1WVNGZFNTVU5aV1ZsS1JWTkpTVUpaV1ZSR1NFVk9UazVNUzBOTlVraENUMXBLV2xsSlRrUklUMFpHV1ZwVFFrdEZVRmxPV0VoV1ZVNUxSRlJFU2s1YVNVaEtVVWxIUlVoVFZsZElWMXBMUlZsYVNVNWFWRTFIU1ZaS1RsRk9RVmxPU0VGSVNVbENVVmhHVVU1YVNrMUxURlJNV0ZOT1RsVk5VRUZYVTBoS1RrTlFWa1ZhV2xOTVMwOU9RME5hVWt4WFFrTlZUMGhLVGxGVVZFdGFVVUZUVTB0T1EwSkRVVU5ZVGtWT1MwRkNUMVJHUTA5WVIwUlVUbHBMU0ZkTFNreE1SRUpQUTFORVUwVkxUazVSVGtsT1IwWkRUbE5EVFZOWFZrNURRa05SVGtWRFFWUkVRVUpSV0V0WFJFWlVVMHhhVkZkVVdFWlZTRk5JVGs1UlRrRlVRMEZZVDFOUlZsVk9SMFJOVkZwVFdWcEdSazVGVUUxTFIwWkRURlJhVmxsQ1YwWk9WVU5YV2t4V1MweEpRMDVPU2toRVdWcGFUVUZGVGxaVlRrdEVWRVJYUWxGWVRsZEVVMFZYVTAxT1YxTlRWVUpVVWxORFRGaEtSbEpSUmxGWldVcExURmhJUlV4UldsUlRSVlpLUlZGR1IwTk5WRk5RV2xOQ1JWRkdWRk5GUjFKRFdsTkZWVmhPUVVwTFNrVklSVlJUV1ZOWlRrVlpVRVJSVTA1WlZWTk9VMVZhUjBSQlUxUk9WRUpJUkZoYVJsRk1XbFJOVEV4UlJsVlBTa1ZUVTFaRFZVTktSbEZKUTFSYVFrTk9TMDVRVGt4WVJVSklTd28yTGlCRVUwMWFUVUZEV0ZwTVNFZFdVa1JXVWtGYVRVcFNRVWRGVmtKRlExUlBTMXBKVFVGU1ZsWkRTRlphVWxSQlJFVkRUVmxGV0ZCWlNrZFBTRUZaVkVwRFVrcFdSMUpKVmt4UFIxUkZWVUZLVEVaU1ZGVlBWVkpIV2toT1VrNVpUVU5NV0VkVFNGWkhRa3BaVVUxTlEwcElSRVJVUVZoRlNFNVVVbFJLUzBwTlZWbEtVa1ZGVWxSTVUwZElUVUZTVlZOUlZrMVdSMWxaVFZaU1IwTkZWVWxPU0VkRlJWVktTMDlTVVVwRFJGbE9RMEphVTBGVFZWVkNRMDlGVVZOWlNGSlpTRk5EVkV0RFUwaEhWRkZTU2tOWVYwNUtTVUZNV1VSVFIwaE5RVkpaUjBGV1QxcFJURXBGVGxkUVZrSlZTVTVMUjBOV1RFRk1WVU5ZU2xoSFNFaFNVbE5TVkVWTVVrTlRXVk5CVWtkV1dWVldRMU5JV0ZWTldWUlZSMDFOUlU1U1NVcE5SRXBMUVV4T1NWVlVVMVZEVWtwSFRVaEhVazlTU0U1S1RVOURSVWhKVms1RldVUk9XVVZKVFZaV1RsSlVXRTFOVWxoQlNFNVRWVVZHVEU5RldFUk5VRWxNUTFkRFRVVldURlZKVFZSR1VFTmFWMDFEVEZGU1RsaFRXbEJOVFVsUlNGTkVWVlpXVFVGVFFVWllVMXBLVjFKRVZGVk5WVWxHUkZwSVNsUllTRTlCVWtGSFJGWk1UazFSVFZoQ1JreFBURk5JVFVwSFdrMVFWa2RWUVZSV1RGcFFXVXhWUlUxSFNWWktUMWhCUjA5SVVFbE1SVk5GUVVOWVYwNUtWa2REU2tOUlRVMVBVbFJXVFZOV1FWaFpTbFpDU0ZKWlEwOVdTRk5EVEZoSFVrcEhUMGhCVmt4VlMxRlBWVXhJU0ZKT1dFMUJUVlZNU1V0V1ZFaFRVVUZNVGtWQlNrOWFWMDFEVEVaQ1JWSlBWVVpWVlV4WVNFMUJRMFJGVlZSVFZWSlJTbEpCVFVSV1JGcFZVRVpKVmxOSVEwZEVRMFJJVWtGS1RrRkhSRlZRUVVoRVJWVlVVMVZYUTBWRFZsSlNTa3hhVTFOTlFscEtVMVJCVlVST1dGVlJWazVHVTBwSlMxbE5Ua1JaVWtGT1NVTllTVTFhVFVwSFQwaFVXVWRGVFZWVVZrOUlTRTlNV2xCTlNsUlZSMGRNVlZoWVRsZFRRVWxOUTFSS1RrdFdURmxIVmxsQlZGbFJXRVJIU0UxU1ZGaE5SMUpSVFVSTlEwUlBVazVYVTBOU1dVcEhUMUpSVDBsSlFsVkdXVlZEVkZaTVdVTlRWa2RQVWxSRlIxbElVMVpOVlVkTldFUktTRTFTV0ZWQ1EwOUZRVWhPVTA5TlIxbFdRbFZSV0VoSFIwaEVTRUZKU0ZOQlExaERUa3hUUzAxUFdVdEVTVkZLU0ZoYVdVTk1SVWhYVmxSVlExUldURnBLVGtwUlIwSlhVa3hTUTFaQlFscEtUVlJWUVVoS1VrcFdWMVZCUjFSRVRVTlNRMDFDU1UxV1RWTkRWRlZEVjBoSFZGQkpTa2RhV1V4UVUwcElSMWxXVDBoVFdWUkdTRU5JVlZoWFNWWk1RbFpWVWxGWVRWbFNVVVpEVFVGU1ZVbExWa3haUTFWVVJsaEdTMUpTUlVoTlFWSkdRMDFCVWxCV1RrNVNXbFZEVkV4V1ZrTklURlpJUjBOSVdFMU1WVWhWUlZWQlExaE5WVUZEV0ZkTlFVTkdWVkJJVEVGRFdVZEtRa2RZU0ZOVlNVdFdURlZIVTFaTVdFZFJTbEZVU1VwV1JFNVRVVlpIVDBoTlNrbEhTRTFNV1VkYVRVZFRRa2RZU0ZOV1ZreEJTMXBIU0VwR1JGcElWazFZVjFkUVRGWkRWVVpMVGxkRFFsTk9WVTlPUVZoSFVVbENXRTFaVEZOSlNGRkJVbEpOVlVsTFdGVk9Sa3RFVFZGRFZFOU5VVkJWVFZwUlNsRkNSa3RKVFZoTldVeEpWazFIVEVsTldsUlVTMWhOVlVoU1FVaE5RVWxPU1ZwV1RFNVhUa1JWVFZwRFZFdFdRMDFEUkVKUVJVRlNRVWhOUkZOT1YwNUtXRUZDUTA5RlJGVlFRVkpWUVZWSVIxRkRWRlpNVGxWUFRsSkJTRTFFUTBSR1MwSlRSRlZaVmxSS1NWVkVWVUZOVEVSSVZsWkpVa0pQU0ZORVRGWlhWUT09";
            string resultString = Base64Decode(encodedString);
            resultString = Base64Decode(resultString);

            Console.WriteLine(resultString);
            //Task 0 finish
            string encoded = "]|d3gaj3r3avcvrgz}t>xvj3K\\A3pzc{va=3V=t=3zg3`{|f.w3grxv3r3`gaz}t31{v..|3d|a.w13r}w?3tzev}3g{v3xvj3z`31xvj1?3k|a3g{v3uza`g3.vggva31{13dzg{31x1?3g{v}3k|a31v13dzg{31v1?3g{v}31.13dzg{31j1?3r}w3g{v}3k|a3}vkg3p{ra31.13dzg{31x13rtrz}?3g{v}31|13dzg{31v13r}w3`|3|}=3J|f3~rj3f`v3z}wvk3|u3p|z}pzwv}pv?3[r~~z}t3wz`gr}pv?3Xr`z`xz3vkr~z}rgz|}?3`grgz`gzpr.3gv`g`3|a3d{rgveva3~vg{|w3j|f3uvv.3d|f.w3`{|d3g{v3qv`g3av`f.g=";
            GetSymbolFrequency(encoded);
            char a = '3';
            char b = ' ';
            string decoded = "";
            var key = Convert.ToInt32(Convert.ToInt32(a) ^ Convert.ToInt32(b));
            
            for (int i = 0; i < encoded.Length; i++)
            {
                decoded += Convert.ToChar(Convert.ToInt32(encoded[i]) ^ Convert.ToInt32(key));
            }
            //var keytemp = Convert.ToInt32(key);
            Console.WriteLine(key.ToString("X"));

            decoded = decoded.Replace('=', 'l');
            Console.WriteLine(decoded);

            string encodeTask2 = "1c41023f564b2a130824570e6b47046b521f3f5208201318245e0e6b40022643072e13183e51183f5a1f3e4702245d4b285a1b23561965133f2413192e571e28564b3f5b0e6b50042643072e4b023f4a4b24554b3f5b0238130425564b3c564b3c5a0727131e38564b245d0732131e3b430e39500a38564b27561f3f5619381f4b385c4b3f5b0e6b580e32401b2a500e6b5a186b5c05274a4b79054a6b67046b540e3f131f235a186b5c052e13192254033f130a3e470426521f22500a275f126b4a043e131c225f076b431924510a295f126b5d0e2e574b3f5c4b3e400e6b400426564b385c193f13042d130c2e5d0e3f5a086b52072c5c192247032613433c5b02285b4b3c5c1920560f6b47032e13092e401f6b5f0a38474b32560a391a476b40022646072a470e2f130a255d0e2a5f0225544b24414b2c410a2f5a0e25474b2f56182856053f1d4b185619225c1e385f1267131c395a1f2e13023f13192254033f13052444476b4a043e131c225f076b5d0e2e574b22474b3f5c4b2f56082243032e414b3f5b0e6b5d0e33474b245d0e6b52186b440e275f456b710e2a414b225d4b265a052f1f4b3f5b0e395689cbaa186b5d046b401b2a500e381d61";
            var bytearr = StringToByteArray(encodeTask2);
            //foreach (var byt in bytearr)
            //{
            //    Console.Write(byt + " ");
            //}

            //var b1 = new List<byte> { 69, 17 };
            //var b2 = new List<byte> { 19, 0 };

            //Console.WriteLine("-----------" + BitwiseHamming(b1, b2));

            //var bytearr = new byte[] { 28, 65, 2, 63, 86, 75, 42, 19 };

            const int MAX_KEY_SIZE = 20;
            int currKeySize = 2;
            var hammingDistances = new Dictionary<int, Tuple<double, double>>();
            do
            {
                int bytesCount = 0;
                var distancesArr = new List<double>();
                for (int i = 0; i < bytearr.Length; i += currKeySize)
                {
                    if (i + (2 * currKeySize) > bytearr.Length)
                        continue;
                    var bytes1 = new List<byte>();
                    var bytes2 = new List<byte>();
                    //int j = i;
                    for (int j = i; j < i + currKeySize; j++)
                    {
                        bytes1.Add(bytearr[j]);
                    }
                    for (int n = i + currKeySize; n < i + currKeySize + currKeySize; n++)
                    {
                        bytes2.Add(bytearr[n]);
                    }
                    distancesArr.Add(BitwiseHamming(bytes1, bytes2));
                    bytesCount++;
                }
                hammingDistances.Add(currKeySize, new Tuple<double, double>(distancesArr.Sum() / bytesCount, (distancesArr.Sum() / bytesCount) / currKeySize));
                currKeySize++;
            }
            while (currKeySize < MAX_KEY_SIZE);

            Console.WriteLine();
            var table = new ConsoleTable("Key Length", "AverHD", "NAverHD");
            foreach (var dict in hammingDistances.OrderBy(x => x.Value.Item2))
                table.AddRow(dict.Key, dict.Value.Item1, dict.Value.Item2);
            table.Write();

            const int keyLength = 3;

            //var bytearr = new byte[] { 28, 65, 2, 28, 65, 65, 42, 2 };

            List<byte> column = new List<byte>();
            var firstPart = BytePartition(bytearr.ToList(), 0, keyLength);
            var secondPart = BytePartition(bytearr.ToList(), 1, keyLength);
            var thirdPart = BytePartition(bytearr.ToList(), 2, keyLength);

            var firstSymbols = GetByteFrequency(firstPart).Keys.ToList();
            var secondSymbols = GetByteFrequency(secondPart).Keys.ToList();
            var thirdSymbols = GetByteFrequency(thirdPart).Keys.ToList();

            Console.WriteLine("first:------------------");
            var firstByte = DecodeRepeatingXOR(firstSymbols);
            Console.WriteLine("second:------------------");
            var secondByte = DecodeRepeatingXOR(secondSymbols);
            Console.WriteLine("third:------------------");
            var thirdByte = DecodeRepeatingXOR(thirdSymbols);

            var keyTask2 = string.Format($"{(char)firstByte}{(char)secondByte}{(char)thirdByte}");
            Console.WriteLine(keyTask2);

            var decodedTask2 = new int[bytearr.Length];
            for(int i = 0; i < bytearr.Length; i+= keyTask2.Length)
            {
                int k = 0;
                for(int j = i; j < keyTask2.Length + i; j++)
                {
                    decodedTask2[j] = (int)(bytearr[j] ^ keyTask2[k]);
                    Console.Write($"{(char)decodedTask2[j]}");
                    k++;
                }
            }

            string encodeTask3 = "EFFPQLEKVTVPCPYFLMVHQLUEWCNVWFYGHYTCETHQEKLPVMSAKSPVPAPVYWMVHQLUSPQLYWLASLFVWPQLMVHQLUPLRPSQLULQESPBLWPCSVRVWFLHLWFLWPUEWFYOTCMQYSLWOYWYETHQEKLPVMSAKSPVPAPVYWHEPPLUWSGYULEMQTLPPLUGUYOLWDTVSQETHQEKLPVPVSMTLEUPQEPCYAMEWWYOYULULTCYWPQLSEOLSVOHTLUYAPVWLYGDALSSVWDPQLNLCKCLRQEASPVILSLEUMQBQVMQCYAHUYKEKTCASLFPYFLMVHQLUHULIVYASHEUEDUEHQBVTTPQLVWFLRYGMYVWMVFLWMLSPVTTBYUNESESADDLSPVYWCYAMEWPUCPYFVIVFLPQLOLSSEDLVWHEUPSKCPQLWAOKLUYGMQEUEMPLUSVWENLCEWFEHHTCGULXALWMCEWETCSVSPYLEMQYGPQLOMEWCYAGVWFEBECPYASLQVDQLUYUFLUGULXALWMCSPEPVSPVMSBVPQPQVSPCHLYGMVHQLUPQLWLRPHEUEDUEHQMYWPEVWSSYOLHULPPCVWPLULSPVWDVWGYUOEPVYWEKYAPSYOLEFFVPVYWETULBEUF";



            Console.ReadLine();
        }
    }
}
