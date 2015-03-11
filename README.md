# lunchroll
A simple Unity game about a hungry rolling blob

Author:
Eric Stender
eric.stender@gmail.com
ericstender.com
github username: estender

Lunch Roll is a simple game I built while learning the 3D aspects of the Unity platform. The game was inspired by an old board game my dad owned called Labyrinth. The concept is simple: roll the ball around the board collecting treats without falling off.

Ever since playing games like Diablo and Civilization as a kid, I’ve been fascinated by procedurally generated worlds and wanted to try coding one myself. Each time you play Lunch Roll, a new board is generated using a modified growing tree algorithm. As this project was a learning exercise, chose to manually generate the map mesh by computing each vertex to gain a firm understanding of how Unity Mesh objects work. If this was a game I was planning to launch and build upon, building the board using prefabs would be better, as one could piece together better-looking grid sections in a fraction of the time.

After the board’s base is generated, obstacles and treats are placed randomly around the board. Many aspects of the map may be changed, such as the amount and size of tiles, the amount and types of objects on board, etc. To simplify this concept to the player, I organized different configurations of these values into various difficulty settings. These settings may be added and modified with zero code change.

All design and coding for this game was done by me. The art was all found on the Unity Asset Store and sounds on soundbible.com.
