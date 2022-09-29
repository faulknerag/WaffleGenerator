using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaffleGenerator
{
    internal class Tile
    {
        public STATE state;
        public char letter;
        public char ansLetter;

        public Tile(char _letter, char _ansLetter)
        {
            letter = _letter.ToString().ToUpper()[0];
            ansLetter = _ansLetter.ToString().ToUpper()[0];
        }

        public ConsoleColor Color()
        {
            return state switch
            {
                STATE.Green => ConsoleColor.Green,
                STATE.Yellow => ConsoleColor.DarkYellow,
                STATE.Gray => ConsoleColor.Gray,
                _ => ConsoleColor.Cyan,
            };
        }

        public bool isSolved()
        {
            return letter == ansLetter;
        }
    }
}
