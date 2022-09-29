# WaffleGenerator
WaffleGenerator is a console application for generating and playing unlimited games in the style of https://wafflegame.net/.

![152109-C__Users_aaron faulkner_OneDrive - AVEVA Solutions Limited_Documents_source_repo](https://user-images.githubusercontent.com/20804273/193152520-47bc623a-f332-4dc5-ad86-2139e02ffad0.png)

  
**How to Play:**  
  
As with the original game hosted on https://wafflegame.net/, the goal is to swap two tiles at a time to reveal all 6 solution words. You can use up to 15 moves to complete the puzzle, though every Waffle can be solved in exactly 10 moves.

Tiles are colored according to Wordle rules:  
* Gray tiles are neither in the correct position nor the correct word
* Green tiles are in the correct position and cannot be moved
* Yellow tiles are in the correct word, but not the correct position
  * Yellow tiles located at the intersection of words may appear in either or both intersected words!
  
  
**Move Syntax:**  
Swaps can be made with the following syntax:  

&nbsp;&nbsp;&nbsp;&nbsp;\<Tile1\>:\<Tile2\>
  
 Where <Tile#> is specified by the word location and letter position:
 * Word location: [T]op, [B]ottom, [L]eft, [R]ight, [V]ertical, [H]orizontal
  *T, B, L, and R refer to the outer square, while V and H refer to the inner words
 * Letter position: 1-5, counting left to right for horizontal words and top to bottom for vertical words
  
 For example:
* To swap the 2nd letter in the [T]op word ('u') with the 4th letter in the [R]ight word ('b'):
  * T2:R4

Tiles at the intersection of words can be referred to by either word's location. For example, the Top 3rd Tile (T3) is equivalent to the Vertical 1st tile (V1)

**Other Commands:**  
* 'R': Reset the scramble
* 'N': Generate a new Waffle
* 'D': Debug (warning: reveals the puzzle solution)
  
  
**Application details:**
* Recursively generates new puzzle word arrangements using a list of known words
* Programmatically shuffles the solved puzzle arrangement to generate a playable puzzle
* Recursively finds all solutions to ensure the puzzle can be solved in no less than 10 moves
* Implements game logic to color each tile correctly 
* Interprets and validates user inputs to play the game
