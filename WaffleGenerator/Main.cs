using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using System.Timers;

namespace WaffleGenerator
{
    internal static class Main
    {
        public static bool debug = false;

        //Store all possible words
        static List<string> words = new List<string>();

        //Random Seed Incrementer
        public static string seedBase = System.DateTime.Now.ToString("yyMMdd");
        public static int count = 1;

        public static void Run()
        {
            //File containing possible answer words
            const string fileName = "..\\..\\..\\WordList.txt";

            //Read contents of file
            using (StreamReader reader = new StreamReader(fileName))
            {
                do
                {
                    string? line = reader.ReadLine();
                    if (line != null) words.Add(line);
                } while (!reader.EndOfStream);
            }

            //Waffle waffle = generateWaffles(20, true);

            //Play the waffle
            bool newGame = true;

            while (newGame)
            {
                Waffle waffle = generateWaffles(1, false);

                newGame = waffle.Play();
            }
        }

        public static Waffle generateWaffles(int count, bool debug)
        {
            //Time the operation
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            Waffle waffle = new Waffle();

            //Generate the waffles
            for (int i = 0; i < count; i++)
            {
                waffle.Clear();

                waffle = GenerateWaffle(waffle);

                if (waffle == null) break;

                if (debug) waffle.ShowWaffle(true);
            }

            stopwatch.Stop();

            if (debug)
            {
                //Report final data
                double avgTime = (double)stopwatch.ElapsedMilliseconds / count;
                Console.WriteLine($"Generated {count} Waffles. Avg time: {avgTime} ms.");
            }

            return waffle;
        }

        public static Waffle? GenerateWaffle(Waffle waffle)
        {
            Waffle thisWaffle = waffle.Copy();

            switch (thisWaffle.WordCount()) {
                case 0: return getValidWordsAndRecurse(thisWaffle, POS.Top);
                case 1: return getValidWordsAndRecurse(thisWaffle, POS.Left, thisWaffle.Words[POS.Top][0]);
                case 2: return getValidWordsAndRecurse(thisWaffle, POS.Vertical, thisWaffle.Words[POS.Top][2]);
                case 3: return getValidWordsAndRecurse(thisWaffle, POS.Horizontal, thisWaffle.Words[POS.Left][2], thisWaffle.Words[POS.Vertical][2]);
                case 4: return getValidWordsAndRecurse(thisWaffle, POS.Right, thisWaffle.Words[POS.Top][4], thisWaffle.Words[POS.Horizontal][4]);
                case 5: return getValidWordsAndRecurse(thisWaffle, POS.Bottom, thisWaffle.Words[POS.Left][4], thisWaffle.Words[POS.Vertical][4], thisWaffle.Words[POS.Right][4]);
                case 6:
                default:
                    //Return the first waffle
                    return thisWaffle;

                    ////Find all possible waffles
                    //count++;
                    //if (count % 10000 == 0) { Console.WriteLine(count.ToString()); }
                    //return null;
            }
        }

        public static Waffle? getValidWordsAndRecurse(Waffle waffle, POS pos, char? first = null, char? third = null, char? last = null)
        {
            List<string> validOptions = new List<string>();

            //List of possible words given previous conditions
            validOptions = words.Where(w => first != null? w[0] == first : true)
                .Where(w => third != null ? w[2] == third : true)
                .Where(w => last != null ? w[4] == last : true)
                .Where(w => !waffle.Words.ContainsValue(w))
                .ToList();

            //If no option is available, return
            if (validOptions.Count == 0) return null;

            //Otherwise, loop through all available options
            while (validOptions.Count > 0)
            {
                //remove a random option and move to next word
                Random random = new Random(getSeed());
                int index = random.Next(validOptions.Count);

                waffle.Words[pos] = validOptions[index];

                validOptions.RemoveAt(index);

                //Choose the next word and make recursive call
                Waffle? result = GenerateWaffle(waffle);

                if (result != null)
                {
                    //Found a complete waffle, return it 
                    return result;
                }
            }

            //There are no possible words left. Return null so that a previous word can be changed
            return null;
        }

        public static int getSeed()
        {
            return int.Parse($"{seedBase}{count.ToString("d3")}");
        }
    }
}
