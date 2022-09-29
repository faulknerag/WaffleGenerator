using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaffleGenerator
{
    internal class Grid
    {
        public Dictionary<POS, string> Answers = new Dictionary<POS, string>();
        public Tile[,] tiles = new Tile[5, 5];

        public Grid(Dictionary<POS, string> answers)
        {
            Answers = answers;

            //Set tiles, unscrambled
            updateTiles(Answers);
        }

        public Grid(Dictionary<POS, string> answers, Tile[,] _tiles)
        {
            Answers = new Dictionary<POS, string>(answers);
            //tiles = _tiles.Clone() as Tile[,];

            //Generate a copy with all new tiles
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (_tiles[i, j] == null) continue;

                    tiles[i, j] = new Tile(_tiles[i, j].letter, _tiles[i, j].ansLetter);
                }
            }
        }

        void updateTiles(Dictionary<POS, string> curWords)
        {
            //Top Row
            for (int i = 0; i < 5; i++)
            {
                updateOrCreateTile(0, i, curWords[POS.Top][i], Answers[POS.Top][i]);
            }

            //Second Row
            updateOrCreateTile(1, 0, curWords[POS.Left][1], Answers[POS.Left][1]);
            updateOrCreateTile(1, 2, curWords[POS.Vertical][1], Answers[POS.Vertical][1]);
            updateOrCreateTile(1, 4, curWords[POS.Right][1], Answers[POS.Right][1]);

            //Middle Row
            for (int i = 0; i < 5; i++)
            {
                updateOrCreateTile(2, i, curWords[POS.Horizontal][i], Answers[POS.Horizontal][i]);
            }

            //Fourth Row
            updateOrCreateTile(3, 0, curWords[POS.Left][3], Answers[POS.Left][3]);
            updateOrCreateTile(3, 2, curWords[POS.Vertical][3], Answers[POS.Vertical][3]);
            updateOrCreateTile(3, 4, curWords[POS.Right][3], Answers[POS.Right][3]);

            //Bottom Row
            for (int i = 0; i < 5; i++)
            {
                updateOrCreateTile(4, i, curWords[POS.Bottom][i], Answers[POS.Bottom][i]);
            }

            //Set colors based on newly updated tiles
            updateColors();
        }

        void updateOrCreateTile(int row, int col, char curChar, char ansChar)
        {
            if (tiles[row, col] == null)
            {
                tiles[row, col] = new Tile(ansChar, curChar);
            }
            else
            {
                tiles[row, col].ansLetter = ansChar;
                tiles[row, col].letter = curChar;
            }
        }

        public void showGrid(bool showAnswer)
        {
            updateColors();

            char spacer = ' ';

            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    //Write a space for empty letters
                    if (tiles[row, col] == null) { Console.Write($"{spacer} {spacer}"); continue; }

                    //Start new line at beginning of each row
                    if (col == 0) Console.WriteLine();

                    if (!showAnswer)
                    {
                        Console.BackgroundColor = tiles[row, col].Color();
                        Console.ForegroundColor = ConsoleColor.Black;

                        //Write the letter
                        Console.Write($"{spacer}{tiles[row, col].letter}{spacer}");
                    }
                    else
                    {
                        Console.BackgroundColor = tiles[0, 0].Color();
                        Console.ForegroundColor = ConsoleColor.Black;

                        //Write the letter
                        Console.Write($"{spacer}{tiles[row, col].ansLetter}{spacer}");
                    }

                    Console.ResetColor();
                }
            }

            Console.WriteLine();
        }

        void updateColors()
        {
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    //Tile whose color is being decided
                    Tile curTile = tiles[row, col];

                    //Skip the 4 missing tiles
                    if (curTile == null) continue;

                    //GREEN if the current letter matches the answer letter
                    if (curTile.isSolved())
                    {
                        curTile.state = STATE.Green;
                    }
                    else
                    {
                        bool foundYellow = false;

                        //Even rows need to check their own rows for matches
                        if (row % 2 == 0)
                        {
                            int unresolved = 0;
                            int matchesBefore = 0;

                            //loop through the columns
                            for (int c = 0; c < 5; c++)
                            {
                                Tile tempTile = tiles[row, c];

                                //Skip this column, solved columns, and null columns
                                if (c == col || tempTile == null || tempTile.isSolved()) continue;

                                //How many non-GREEN answer instances are there in this row for my letter 
                                if (tempTile.ansLetter == curTile.letter) unresolved++;

                                //How many instances of my letter are unresolved yellows in this row before my
                                if (c < col && tempTile.letter == curTile.letter) matchesBefore++;
                            }

                            //If the number of my letter before me is less than the number of unresolved => YELLOW
                            if (matchesBefore < unresolved) foundYellow = true;
                        }

                        //Even cols need to check their own cols for matches
                        if (col % 2 == 0)
                        {
                            int unresolved = 0;
                            int matchesBefore = 0;

                            //loop through the rows
                            for (int r = 0; r < 5; r++)
                            {
                                Tile tempTile = tiles[r, col];

                                //Skip this column, solved columns, and null columns
                                if (r == row || tempTile == null || tempTile.isSolved()) continue;

                                //How many non-GREEN answer instances are there in this row for my letter 
                                if (tempTile.ansLetter == curTile.letter) unresolved++;

                                //How many instances of my letter are unresolved yellows in this row before my
                                if (r < row && tempTile.letter == curTile.letter) matchesBefore++;
                            }

                            //If the number of my letter before me is less than the number of unresolved => YELLOW
                            if (matchesBefore < unresolved) foundYellow = true;
                        }

                        //Choose between YELLOW and GRAY
                        curTile.state = foundYellow ? STATE.Yellow : STATE.Gray;
                    }
                }
            }
        }

        public void Swap(Coord first, Coord second)
        {
            Tile firstTile = tiles[first.row, first.col];
            Tile secondTile = tiles[second.row, second.col];

            char temp = firstTile.letter;

            firstTile.letter = secondTile.letter;
            secondTile.letter = temp;
        }

        public void Swap(Tile first, Tile second)
        {
            char temp = first.letter;

            first.letter = second.letter;
            second.letter = temp;
        }

        public void Scramble()
        {
            //Reset grid to solved state
            updateTiles(Answers);

            //List of possible coordinates to swap
            List<Coord> coords = new List<Coord>();

            int[] corners = new int[] { 0, 4 };
            int[] nullCells = new int[] { 1, 3 };

            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    //Skip corners
                    if (corners.Contains(row) && corners.Contains(col)) continue;

                    //Skip null cells
                    if (nullCells.Contains(row) && nullCells.Contains(col)) continue;

                    //Skip center
                    if (row == 2 && col == 2) continue;

                    coords.Add(new Coord(row, col));
                }
            }

            int[] zeroGreens = new int[] { 3, 3, 3, 3, 2, 2 };
            int[] zeroGreens2 = new int[] { 4, 4, 2, 2, 2, 2 };
            int[] oneGreen = new int[] { 4, 4, 3, 2, 2 };
            int[] twoGreens = new int[] { 4, 4, 4, 2 };

            Random rand = new Random(Main.getSeed());

            int[] limits = rand.Next(4) switch
            {
                0 => zeroGreens,
                1 => zeroGreens2,
                2 => oneGreen,
                3 => twoGreens,
            };

            bool success;
            int rndSeed = 0;
            do
            {
                success = partitionGroups(coords, limits, rndSeed++);
            } while (!success);
        }

        bool partitionGroups(List<Coord> allCoords, int[] limits, int rndSeed)
        {
            List<List<Coord>> bins = new List<List<Coord>>();

            //Create set of bins
            for (int i = 0; i < limits.Length; i++)
            {
                bins.Add(new List<Coord>());
            }

            Random rand = new Random(rndSeed);

            //Randomize the bins
            limits = limits.OrderBy(l => rand.Next()).ToArray();

            //Make a copy of the coords so that the copy can be modified
            List<Coord> coords = new List<Coord>(allCoords);

            //Loop backwards through all available and partition into bins
            for (int i = coords.Count - 1; i >= 0; i--)
            {
                //Sort available coords by count of matching letters
                coords.Sort((l, r) => coords.Count(temp => Get(temp).letter == Get(l).letter)
                    - coords.Count(temp => Get(temp).letter == Get(r).letter));

                Tile curTile = Get(coords[i]);

                //Loop through all the bins 
                //Add to first bin that doesn't already contain the same letter
                for (int j = 0; j < limits.Length; j++)
                {
                    List<Coord> cycle = bins[j];

                    //Skip this bin if it already has a tile with the same letter
                    if (cycle.Count(c => Get(c).letter == curTile.letter) > 0) continue;

                    //How many tiles are already filled in this bin
                    int usedCount = cycle.Count;

                    //Bin is full, skip this bin
                    if (usedCount == limits[j]) continue;

                    //Add this tile to the bin
                    cycle.Add(coords[i]);

                    coords.Remove(coords[i]);

                    //Found a bin for this tile. Break out to place next tile
                    break;
                }
            }

            //Perform all the swaps
            foreach (List<Coord> cycle in bins)
            {
                //Randomize the cycle
                cycle.Sort((l, r) => rand.Next() - rand.Next());

                for (int i = 1; i < cycle.Count; i++)
                {
                    Swap(cycle[i], cycle[i - 1]);
                }

                //Show cycles pre-conflict check
                if (Main.debug)
                {
                    //DEBUG: Show the cycles
                    Console.WriteLine();
                    foreach (Coord c in cycle) { Console.Write($"{Get(c).letter} ({c.row + 1},{c.col + 1}) {Get(c).ansLetter} | "); }
                }
            }

            //Copy the grid to test if there's a solution better than 10 moves
            Grid testGrid = new Grid(Answers, tiles);

            bool foundMinimal = FindMinimumSolution(testGrid);

            //Show final cyles
            //if (Main.debug)
            //{
            //    foreach (List<Coord> cycle in bins)
            //    {
            //        //DEBUG: Show the cycles
            //        Console.WriteLine();
            //        foreach (Coord c in cycle) { Console.Write($"{Get(c).letter} ({c.row + 1},{c.col + 1}) {Get(c).ansLetter} | "); }
            //    }

            //    Console.WriteLine();
            //}

            //Successfully partitioned without any potential conflicts
            if (!foundMinimal) return true;

            //Reset tiles and try again
            updateTiles(Answers);

            return false;
        }

        bool FindMinimumSolution(Grid grid, int moveCount = 0)
        {
            //No need to keep looking, already used 10 moves 
            if (moveCount++ >= 10)
            {
                return false;
            }

            List<Tile> remaining = GetUnsolved(grid.tiles);

            //Success condition: there are no possible moves after fewer than 10 moves
            if (remaining.Count == 0)
            {
                if (Main.debug) Console.WriteLine($"\nFound a solution in {moveCount - 1} moves. Generating new swap.");
                return true;
            }

            //Extract an element from remaining list
            Tile thisTile = remaining[0];
            remaining.RemoveAt(0);

            //Identify all possible swaps for this tile 
            List<Tile> moves = remaining.Where(r => r.letter == thisTile.ansLetter).ToList();

            //Loop through all moves, make the move, then recursively call
            foreach (Tile move in moves)
            {
                grid.Swap(thisTile, move);

                //Recursive call, increment move count
                bool done = FindMinimumSolution(grid, moveCount);

                if (done) return true;

                //Undo the swap for the next possible move
                grid.Swap(thisTile, move);
            }

            //Did not find a solution in fewer than 10 moves
            return false;
        }

        //Convert from Letter+Number format to a row and column
        public Coord GetCoord(char letter, int num)
        {
            int row;
            int col;

            letter = letter.ToString().ToUpper()[0];
            if (letter == 'T' || letter == 'H' || letter == 'B')
            {
                row = letter switch
                {
                    'T' => 0,
                    'H' => 2,
                    'B' => 4,
                };
                col = num - 1;
            }
            else
            {
                row = num - 1;
                col = letter switch
                {
                    'L' => 0,
                    'V' => 2,
                    'R' => 4,
                };
            }

            return new Coord(row, col);
        }

        public Tile Get(Coord coord)
        {
            return tiles[coord.row, coord.col];
        }

        public List<Tile> GetUnsolved(Tile[,] tiles)
        {
            //List of unsolved tiles
            List<Tile> unsolved = new List<Tile>();

            //Populate list of unsolved tiles
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    Tile temp = tiles[row, col];
                    if (temp != null && !temp.isSolved()) unsolved.Add(temp);
                }
            }

            return unsolved;
        }
    }
}
