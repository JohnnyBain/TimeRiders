#TimeRiders

Time riders is a single player, turn based puzzle game that sees you travelling through time to make multiple deliveries in the same time and space.
With each delivery the roads will become more and more crowded so it's important to plan your routes ahead of time. 

Each rider is colour coded and so are their start and end points in the level.
A small square in a tile denotes where the rider of that colour will spawn.
A large square in a tile denotes where the rider of that colour will need to deliver to.

The temporal engine in your vehicle leaves behind a rip in space time. This trail grows up to a length of 6 tiles and occupies the spaces you've travelled through.
When a rider reaches their destination, the trail remains at full length and does not diminish.

Contact between two riders will end the level
Contact between a rider a trail will end the level

As you input directions, if the move is valid (is not into a wall) the rider will be moved into that space. Any of your previous rides will step through
the route that you drove for their delivery.

A level is complete when all the riders have completed their deliveries and there’s been no crashes between riders. If you finish your current ride before your old routes 
have concluded, the routes will continue to play out.

If the player crashes during a ride they can choose to restart the entire level (doing every delivery again), or they can replay just the ride they were playing when
they crashed.

Project composition:

The Unity game engine works by attaching scripts to object within the gamespace
If the script attached to an object extends the MonoBehaviour class it will have inherited methods that are called at certain points in the game loop
	- (Awake) is called the moment an object with this script is instantiated
	- (Start) is called on the first frame change that happens whilst this script is instantiated
	- (Update) is called on every frame after that
These Monobehaviour scripts are responsibility for the majority of my game logic (found in TimeRiders/Assets/Scripts)

When a game is run in Unity, these three methods are called on any scripts that currently exist in the scene.
My presaved scene begines with the MenuManagerScript on an instantiated object so this can be thought of as the main method.

Although the MenuManager can be thought of as the main, the GameManagerScript is where all the in game computation happens. A GameManager 
is created for each level that is played. The user chooses a level using the MenuManagers navigation (found in TimeRiders/Assets/Scripts/UI Scripts). 
Once the game has begun, the user input is what drives the program (InputCheck() in GameManagerScript.cs)

notes:

Prefabricated objects are objects that already have scripts attached. So when I instantiate a PlayerRider prefab in the GameManagerScript
it can largely be assumed that it has attached to it the playerRiderScript (because I have specified this in the unity editor). 

A serialized variable are visible by the Unity editor. They can be observed and maniulated from within it. For example,
timeDelay in GameManagerScript.cs, is set in the editor.
