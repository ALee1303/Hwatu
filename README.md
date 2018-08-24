# Hwatu
Library for Traditional Korean Card Game using Monogame

## Project Overview
This project contains scripts and Visual Studio project for Asian traditional playing cards, Hwatu(Hanafuda). The data structure for Hwatu is created in C# POCO classes to extend portability. Other Game features are included in the project as MonoGame GameComponent. Lastly project also includes Unit Test used during the development.

## Hwatu Data Structure
Hwatu(Hanafuda) is traditional Asian playing cards. Much similar to French playing cards. It has many varieties of games with varying rules. The provided Hwatu classes and ICollection data structure allows new game mode to be created in ease by creating separations of concerns between game rules and card classes.
* [Hanafuda](https://github.com/ALee1303/Hwatu/tree/master/Hwatu/Card)
* [CardCollection](https://github.com/ALee1303/Hwatu/tree/master/Hwatu/Collection)

## Board and Board Manager
The Board class holds the general logic for each game type. It holds functions that varies depending on each game types such as dealing cards and calculating points. This way, developers can easily inherit and override the base Board class. This allows easier integration to other game mode.
BoardManager class starts and manages the game by loading and interacting with a board of a specific game type. It also implements MonoGame Components to draw the progress of a board in play.
Check [here](https://github.com/ALee1303/Hwatu/tree/master/Hwatu/Board) for details.

## Player
Player classes interact with BoardManager to choose cards to play. Both NPC and MainPlayer are derived from Player classes so that interaction with BoardManager stays independent of which player takes turn.
Check [here](https://github.com/ALee1303/Hwatu/tree/master/Hwatu/Players) for details.

## Drawing the Card
The project is using MonoGame to draw the gameplay. The DrawableCard class implements both Hanafuda and Sprite2D to visualize revealed cards. The BoardManager sorts these cards into dictionaries depending on their status and location, which also decides the draw order.
Check [here](https://github.com/ALee1303/Hwatu/tree/master/Hwatu/MonoGameComponents) for details.
