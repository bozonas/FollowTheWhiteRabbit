using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowTheWhiteRabbit
{
    public class Program
    {
        static void Main(string[] args)
        {
            string hash = "e4820b45d2277f3844eac66c903e84be"; // OK    printout stout yawls
            //string hash = "23170acc097c24edb98fc5488ab033fe"; // OK    ty outlaws printouts
            //string hash = "665e5bcb0c20062fe8abaaf4628bb154"; // OK    wu lisp not statutory

            string anagram = "poultry outwits ants";
            int anagramWordCount = anagram.Count(x => x == ' ') + 1;
            string anagramNoSpaces = string.Concat(anagram.Replace(" ", "").OrderBy(x => x));
            var anagramFreq = GetCharFrequencyDic(anagramNoSpaces);

            var anagramEntity = new AnagramEntity
            {
                Anagram = anagramNoSpaces,
                AnagramLen = anagramNoSpaces.Count(),
                AnagramFreq = anagramFreq
            };

            var lines = File.ReadLines("../../wordlist");

            var words = new List<string>();
            foreach (string word in lines)
            {
                var newWord = word.Replace("'", "");
                if (IsValidWord(anagramNoSpaces, anagramFreq, word))
                    words.Add(newWord);
            }
            words = words.Distinct().OrderByDescending(x => x.Count()).ToList();

            IEnumerable<IEnumerable<string>> permutations = GetPermutationsNotRep(words, anagramEntity, 4);

            foreach (var permutation in permutations)
            {
                var item = permutation.ToList();
                var comb = GetPermutations(item, item.Count()).ToList();
                var combinations = comb.Select(x => string.Join(" ", x));
                foreach (var combination in combinations)
                {
                    var firstHash = CreateMD5(combination);
                    if (hash == firstHash)
                    {
                        Console.WriteLine("Success!");
                        Console.WriteLine(combination);
                        Console.ReadLine();
                    }
                }
            }
        }


        public static bool IsValidWord(string anagram, Dictionary<char, int> anagramFreq, string word)
        {
            if (anagram.Count() < word.Count())
                return false;

            Dictionary<char, int> wordFreq = GetCharFrequencyDic(word);

            foreach(var item in wordFreq)
            {
                if (!anagramFreq.ContainsKey(item.Key)
                    || anagramFreq[item.Key] < wordFreq[item.Key])
                    return false;
            }

            return true;
        }

        public static Dictionary<char, int> GetCharFrequencyDic(string text)
        {
            var charDic = text.GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
            return charDic;
        }

        public static IEnumerable<IEnumerable<string>> GetPermutationsNotRep(IEnumerable<string> words,
            AnagramEntity anagramEntity, int depth, IEnumerable<string> prevWords = null)
        {
            int offset = 0;

            foreach (var word in words)
            {
                offset++;
                IEnumerable<string> concat;
                string concatString;

                if (prevWords == null)
                {
                    concat = new List<string> { word };
                    concatString = word;
                    Console.WriteLine($"Word - {concatString}");
                }
                else
                {
                    concat = prevWords.Concat(new List<string> { word });
                    concatString = string.Join("", concat);

                    if (!IsValidWord(anagramEntity.Anagram, anagramEntity.AnagramFreq, concatString))
                        continue;

                    var concatStringCount = concatString.Count();
                    if (concatStringCount == anagramEntity.AnagramLen)
                        yield return concat;

                    if (concatStringCount > anagramEntity.AnagramLen)
                        continue;

                    if (depth <= 1)
                        continue;
                }

                foreach (var item in GetPermutationsNotRep(words.Skip(offset), anagramEntity,
                    depth - 1, concat))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<IEnumerable<string>> GetPermutations(IEnumerable<string> items, int count)
        {
            int i = 0;
            foreach (var item in items)
            {
                i++;

                if (count == 1)
                    yield return new List<string> { item };

                foreach (var result in GetPermutations(items.Where(x => x != item), count - 1))
                {
                    yield return new List<string> { item }.Concat(result);
                }
            }
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }

        public class AnagramEntity
        {
            public string Anagram { get; set; }
            public int AnagramLen { get; set; }
            public Dictionary<char, int> AnagramFreq { get; set; }
        }
    }
}
