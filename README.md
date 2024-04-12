# Pcgzombieisland

ECS7016P - Interactive Agents and Procedural Generation
Coursework: Unity Project
Implemented an agent simulation using behaviour trees within a 2D level that is procedurally generated using cellular automata.

Scenario 2: Zombie Apocalypse
Environment: Ocean Islands
Agent A: Survivor
Agent B: Zombie

*For procedural generation of map, I implemented Conway’s game of life algorithm with birth limit as 2 and death limit as 3. And run the algorithm for 9 iterations to get smooth island like generation.
And also spawn in sand sprite at edges of island by calculating neighbors 

Link to Youtube video-
https://youtu.be/Zg4DvL7AIUQ

*For agent behavior trees I used npbehave to create different behavior tree for zombie and player 
*The `ZombieBehaviour` class implements a behavior tree for zombie agents in a Unity game, facilitating adaptive behavior based on the presence of survivors.
- The behavior tree consists of a `Selector` node, functioning as the root, which orchestrates decision-making.
  - Within the `Selector`, a `Service` node periodically updates the blackboard with information about nearby survivors.
  - If survivors are detected (`thievesInRange` is true), the tree executes a sequence of actions: chasing and attacking survivors.
  - If no survivors are detected, the zombie switches to a fallback behavior, moving randomly within the game world.
- The `UpdateBlackboard()` method scans for survivors within a predefined range and updates the blackboard accordingly.
- Specific methods like `ChaseSurvivor()` and `MoveRandomly()` dictate the zombie's actions based on the information stored in the blackboard. For instance:
  - `ChaseSurvivor()` finds the nearest survivor and moves towards them.
  - `MoveRandomly()` generates a random position within the game world and moves the zombie towards it, ensuring varied exploration.
Survivor Behavior tree
The `SurvivorBehaviour` class defines the behavior tree for survivor agents in a Unity game, enabling them to dynamically respond to the presence of zombies.
- The behavior tree, initialized in the `Start()` method, is structured around a `Selector` node, serving as the root of decision-making.
- Within the `Selector`, a `Service` node periodically updates the blackboard with information about the nearest zombie and its distance from the survivor.
- If a zombie is within a certain distance (`playerDistance`), the survivor switches to a sequence of actions:
  - Changing color to blue.
 - Moving towards the position of the nearest zombie (`MoveTowards` method).
- If no zombies are nearby, the survivor waits in place (`Sequence` with `WaitUntilStopped`).
- The `UpdatePlayerDistance()` method calculates the distance to the nearest zombie andupdates the blackboard with this information.
- `MoveTowards()` adjusts the survivor's position towards the specified local position.
- `SetColor()` changes the survivor's color to the specified one.
This design empowers survivor agents to dynamically adapt their behavior based on the proximity of zombies, enhancing the realism and complexity of gameplay interactions.



ECS7016P - Interactive Agents and Procedural Generation
Coursework: Unity Project
Implemented an agent simulation using behaviour trees within a 2D level that is procedurally generated using cellular automata.

Scenario 2: Zombie Apocalypse
Environment: Ocean Islands
Agent A: Survivor
Agent B: Zombie

*For procedural generation of map, I implemented Conway’s game of life algorithm with birth limit as 2 and death limit as 3. And run the algorithm for 9 iterations to get smooth island like generation.
And also spawn in sand sprite at edges of island by calculating neighbors 

Link to Youtube video-
https://youtu.be/Zg4DvL7AIUQ

*For agent behavior trees I used npbehave to create different behavior tree for zombie and player 
*The `ZombieBehaviour` class implements a behavior tree for zombie agents in a Unity game, facilitating adaptive behavior based on the presence of survivors.
- The behavior tree consists of a `Selector` node, functioning as the root, which orchestrates decision-making.
  - Within the `Selector`, a `Service` node periodically updates the blackboard with information about nearby survivors.
  - If survivors are detected (`thievesInRange` is true), the tree executes a sequence of actions: chasing and attacking survivors.
  - If no survivors are detected, the zombie switches to a fallback behavior, moving randomly within the game world.
- The `UpdateBlackboard()` method scans for survivors within a predefined range and updates the blackboard accordingly.
- Specific methods like `ChaseSurvivor()` and `MoveRandomly()` dictate the zombie's actions based on the information stored in the blackboard. For instance:
  - `ChaseSurvivor()` finds the nearest survivor and moves towards them.
  - `MoveRandomly()` generates a random position within the game world and moves the zombie towards it, ensuring varied exploration.
Survivor Behavior tree
The `SurvivorBehaviour` class defines the behavior tree for survivor agents in a Unity game, enabling them to dynamically respond to the presence of zombies.
- The behavior tree, initialized in the `Start()` method, is structured around a `Selector` node, serving as the root of decision-making.
- Within the `Selector`, a `Service` node periodically updates the blackboard with information about the nearest zombie and its distance from the survivor.
- If a zombie is within a certain distance (`playerDistance`), the survivor switches to a sequence of actions:
  - Changing color to blue.
 - Moving towards the position of the nearest zombie (`MoveTowards` method).
- If no zombies are nearby, the survivor waits in place (`Sequence` with `WaitUntilStopped`).
- The `UpdatePlayerDistance()` method calculates the distance to the nearest zombie andupdates the blackboard with this information.
- `MoveTowards()` adjusts the survivor's position towards the specified local position.
- `SetColor()` changes the survivor's color to the specified one.
This design empowers survivor agents to dynamically adapt their behavior based on the proximity of zombies, enhancing the realism and complexity of gameplay interactions.


