using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaffleGenerator
{
    internal class Waffle
    {
        //Solution to puzzle
        public Dictionary<POS, string> Words = new Dictionary<POS, string>();

        Grid grid;

        public Waffle()
        {

        }

        public bool Play()
        {
            bool keepPlaying = true;

            //Play Loop 
            while (keepPlaying)
            {
                Console.Clear();

                //Keep track of score
                int moveCount = 1;

                //Generate grid if necessary
                if (grid == null) grid = new Grid(Words);

                //Scramble the grid
                grid.Scramble();

                //DEBUG: Show answer
                if (Main.debug) ShowWaffle(true);

                //Show the scrambled waffle
                ShowWaffle(false, true);

                //Prompt Loop
                while (true)
                {
                    //Prompt for move and show current score
                    Console.Write($"[{moveCount.ToString("d2")}/15]: ");

                    //Get move 
                    string move = Console.ReadLine();
                    move = move.Trim();

                    //Option to rescramble this waffle
                    if (move.ToUpper() == "R") { break; }

                    //Option to play a new game (and generate new Waffle)
                    if (move.ToUpper() == "N") { Main.count++; return true; }

                    //Option to toggle debug
                    if (move.ToUpper() == "D") { Main.debug = Main.debug ? false : true; break; }

                    if (String.IsNullOrEmpty(move) || !MakeMove(move)) continue;

                    Console.Clear();

                    //DEBUG: Show answer
                    if (Main.debug) ShowWaffle(true, true);

                    ShowWaffle(false, true);

                    //Check for win
                    if ((grid.GetUnsolved(grid.tiles)).Count == 0)
                    {
                        Console.Write("Solved! ");

                        int stars = 15 - moveCount;

                        for (int i = 0; i < stars; i++) Console.Write("*");

                        Console.WriteLine();

                        keepPlaying = false;
                        break;
                    }

                    //Check for loss
                    if (moveCount >= 15)
                    {
                        Console.Write("\nNo more waffles.\n");

                        //Show answer
                        ShowWaffle(true);

                        keepPlaying = false;
                        break;
                    }

                    moveCount++;
                }
            }

            Console.WriteLine("\nPress <ENTER> to play another game.");
            Console.ReadLine();

            //Increment counter to get a new game
            Main.count++;

            return true;
        }

        bool MakeMove(string move)
        {
            //Validate overall entry
            string pattern = @"[TLVHRBtlvhrb][1-5]:[TLVHRBtlvhrb][1-5]";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);

            if (!regex.IsMatch(move)) return false;

            //Split left and right letters
            string[] input = move.Split(":");
            string first = input[0];
            string second = input[1];

            //TLVHRB
            char firstWordLetter = first[0].ToString().ToUpper()[0];
            char secondWordLetter = second[0].ToString().ToUpper()[0];

            //1-5
            int firstWordNum = int.Parse(first[1].ToString());
            int secondWordNum = int.Parse(second[1].ToString());

            Coord firstCoord = grid.GetCoord(firstWordLetter, firstWordNum);
            Coord secondCoord = grid.GetCoord(secondWordLetter, secondWordNum);

            //Don't allow swapping solved tiles
            if (grid.Get(firstCoord).isSolved() || grid.Get(secondCoord).isSolved()) return false;

            grid.Swap(firstCoord, secondCoord);

            return true;
        }

        public void ShowWaffle(bool showAnswer, bool showHeader = false)
        {
            if (showHeader) Console.WriteLine($"Waffle #{Main.count.ToString("d3")} for {System.DateTime.Today.ToString("dd-MMM-yy")} ");

            //Generate the grid if necessary
            if (grid == null) grid = new Grid(Words);

            //Show the grid
            grid.showGrid(showAnswer);

            Console.WriteLine();
        }

        public Waffle Copy()
        {
            Waffle copy = new Waffle
            {
                Words = new Dictionary<POS, string>(Words)
            };

            return copy;
        }

        public int WordCount()
        {
            return Words.Keys.Count;
        }

        public void Clear()
        {
            Words.Clear();
        }
    }
}
