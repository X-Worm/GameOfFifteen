# Game of Fifteen

[![N|Solid](https://upload.wikimedia.org/wikipedia/commons/thumb/3/39/15-puzzle-loyd.svg/220px-15-puzzle-loyd.svg.png)](https://ru.wikipedia.org/wiki/%D0%98%D0%B3%D1%80%D0%B0_%D0%B2_15)



The 15-puzzle (also called Gem Puzzle, Boss Puzzle, Game of Fifteen, Mystic Square and many others) is a sliding puzzle that consists of a frame of numbered square tiles in random order with one tile missing. The puzzle also exists in other sizes, particularly the smaller 8-puzzle. If the size is 3×3 tiles, the puzzle is called the 8-puzzle or 9-puzzle, and if 4×4 tiles, the puzzle is called the 15-puzzle or 16-puzzle named, respectively, for the number of tiles and the number of spaces. The object of the puzzle is to place the tiles in order by making sliding moves that use the empty space.

The n-puzzle is a classical problem for modelling algorithms involving heuristics. Commonly used heuristics for this problem include counting the number of misplaced tiles and finding the sum of the taxicab distances between each block and its position in the goal configuration. Note that both are admissible, i.e. they never overestimate the number of moves left, which ensures optimality for certain search algorithms such as A*.

### Program view

[![N|Solid](https://raw.githubusercontent.com/X-Worm/GameOfFifteen/master/Resources/View.bmp)](https://github.com/X-Worm/GameOfFifteen/tree/master/Resources)
- Shuffle button.
"Shuffle" key is used to mix the initial state. By default, she makes 100 shuffle, but you can specify the number of shuffles in the box opposite. 1 mixing takes place 10 ms, so 100 shuffle will be performed for 1 second. The result of mixing is recorded in a text document "Shuffle.txt".
- Solve button.
The "Solve" key is intended to solve the problem (it allows you to find the correct path). After pressing the key in the field opposite will be shown 1 step to be implemented. Also under the key there is a field showing the number of moves to be performed. The solution is based on the mixed state, which is written in a text document "Shuffle.txt". The result is recorded in a text document "Solve.txt".
- Next Step button.
After receiving the result with the "Solve" key, this key allows you to view the resolution steps.
- Play Move with time step button.
This key shows all the steps of the solution with the given time, the time is set in the field opposite the key.


###### X_Worm 2019 , vasilkindiy@gmail.com

