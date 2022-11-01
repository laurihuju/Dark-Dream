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
  - 
- Inventory menu
  - The menu can be accessed during the game and it has multiple functions
  - Inventory
    - Items are stored in different slots
    - Items can be moved from slot to another by dragging them with mouse
    - Player sees the item's name as a tooltip when hovering an item with the mouse
    - Items can be used by pressing buttons 1-3 from the keyboard when the item is in the inventory's top row
  - The quest view
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
