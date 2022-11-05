# Dark Dream

This project is a 2D horror adventure game made in a team of three people. The language of the game UI is Finnish.


## The Most Important Game Features

- Player movement in 2D open world
  - Smooth moving (acceleration and deceleration)
  - Walking
  - Sprinting with stamina usage
- The camera follows smoothly the player and stays within the map boundaries
- Health and stamina system
  - Stamina generates slowly back to the maximum if player has sprinted
- Item system
  - Different item types with different functionalities can be registered to the system
  - Items can be picked up from the world
  - Inventory for storing picked items (see the inventory menu section)
- Quest system
  - Multiple quests can be active at the same time
  - Quest locations are shown in the mini map
- Dialog system
  - The dialogs are made using the Fungus library
  - Dialogs are used to speak with the NPCs and to guide the player in the map
- Inventory menu
  - The menu can be accessed during the game and it has multiple functions
  - Inventory
    - Items are stored in different slots
    - Items can be moved from slot to another by dragging them with mouse
    - The player sees the item's name as a tooltip when hovering an item with the mouse
    - Items can be used by pressing buttons 1-3 from the keyboard when the item is in the inventory's top row
  - The quest view
    - Shows the active quest with highest order (multiple quests can be active at the same time)
    - Shows a short text describing the current cuest
    - Shows the item required to be collected in the quest (if any)
  - Health and stamina bars
  - Buttons
    - Continue button
    - Main menu button
    - Guide button
    - Setting button
- Settings menu
  - The player can toggle on and off two options related to the shadow rendering (if shadows are causing too much lag for the player)
- Saving
  - The game state is saved when the player completes a quest
- Enemy with AI pathfinding (not made by me)
- 2D open world map (not made by me)
  - Lighting and shadows
  - The player can go behind objects and on top of some aseas
- Mini map (not made by me)
- Music and sounds (not made by me)
- Main menu (not made by me)

## The Development of the Game

- The game was made in eight weeks (including the designing).
- The graphic assets used in the project were mainly free assets downloaded from the internet. Some assets were made by one of our team members.
- All the sounds in the game are free assets from the internet.
