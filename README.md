# WaffleGenerator
WaffleGenerator is a console application for generating and playing unlimited games in the style of https://wafflegame.net/.

![152109-C__Users_aaron faulkner_OneDrive - AVEVA Solutions Limited_Documents_source_repo](https://user-images.githubusercontent.com/20804273/193152520-47bc623a-f332-4dc5-ad86-2139e02ffad0.png)

  
# Game Rules:
  
As with the original game hosted on https://wafflegame.net/, the goal is to swap two tiles at a time to reveal all 6 solution words. You can use up to 15 moves to complete the puzzle, though every Waffle can be solved in exactly 10 moves.

Tiles are colored similar to Wordle rules:  
* Green tiles are in the correct position and cannot be moved 
* Gray tiles are neither in the correct position nor the correct word
* Yellow tiles are in the correct word, but not the correct position
    * Yellow tiles located at the intersection of words may appear in either or both intersected words!
  
  
# Move Syntax:
  
Swaps can be made with the following syntax:  

> \<Tile1\>:\<Tile2\>
  
Where <Tile#> is specified by the word location and letter position:
* Word location: [T]op, [B]ottom, [L]eft, [R]ight, [V]ertical, [H]orizontal  
  * T, B, L, and R refer to the outer square
  * V and H refer to the inner words
* Letter position: 1-5, counting left to right for horizontal words and top to bottom for vertical words
  
Tiles at the intersection of words can be referred to by either word's location. For example, the 3rd letter in the top word (T3) is equivalent to the 1st letter in the vertical word (V1).
 
 For example:
* To swap the 2nd letter in the [T]op word ('u') with the 4th letter in the [R]ight word ('b'):
  
    > T2:R4
* To swap the 3rd letter in the [B]ottom word ('e') with the 4th letter in the [V]ertical word ('t'), either of the following can be used:
  
    > B3:V4  
    > V5:V4

**Other Commands:**  
* 'R': [R]eset the scramble
* 'N': Generate a [N]ew Waffle
* 'D': [D]ebug (warning: reveals the puzzle solution)
  
  
# Application details:
* Recursively generates new puzzle word arrangements using a list of known words
* Programmatically shuffles the solved puzzle arrangement to generate a playable puzzle
* Recursively finds all solutions to ensure the puzzle can be solved in no less than 10 moves
* Interprets and validates user inputs to play the game
