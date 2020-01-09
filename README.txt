Alessandro Profenna
PROG56693 Project 2

GitLab:
https://gitlab.com/AlessandroPro/prog56693.project2.git


This tool can actually be used to make working maps for the Gauntlet Game.

Go to Tools -> Gauntlet Level Editor to launch the editor.

To start editing a level, select an existing level from the object field in the left pane, or create a new one with the 
"Create New Level" button of the size of your choosing. You can also select an existing game, and open one of its levels to edit.
There is currently one game, with 2 levels. Add levels to the game using the buttons on the left. You can have more than one "game", each with its own list of levels.
If you create any new objects (game, levels, prefabs), they will automatically be put in the correct folders.

Use the "Create/Edit Prefab" button in the right to open a sub window depending on your selection.
From this window, you can create or select a prefab scriptable object and select its sprite, and edit its information, then you can use that
sprite to draw on the map.

DRAWING:
Select a prefab from the list in the right pane, and click or drag along tiles to paint on the map. Check the 'Erase Mode'
box to erase the tiles you click or drag on.
There are 3 layers, rendered in order. You can choose the layer to paint on. You can also choose which layers are visible.

EXPORTING (to the game):
You must have a "Game" scriptable object loaded in the top left object field in order to export, as well as a player in the player field in the left pane.
A "Game" consists of a list of levels and one player object. All of the map objects are being saved when exporting, along with the player in the first level.

When the "Save Game Data" button is clicked, a file dialog will open asking for a save directory (not file).
You MUST choose the Assets folder of where the Gauntlet Game data resides. In the case of this Gauntlet Project, it will be prog50016.Project2/GauntletGame/Build/Assets
For reference, prog50016.Project2/GauntletGame/ is where the solution file for the Gauntlet Game is.
This will be submitted with the prog50016 project 2 submission. So you'll need it to test exporting and loading into the game.
Once the folder is selected, the export will begin:
- a level json file will be created for each level in the game and put into the /Levels folder.
- A json file with the list of levels in the game will also be placed in the /Levels folder.
- Textures and prefabs, and their meta files, will be created/copied into their respective /Prefabs or /Images folder.
Files with the same name will be overwritten.

With an instance of the Gauntlet Game, all you need to do is run the game and it should play as expected.

SAVING (tool data):
All data within the scriptable objects are saved, even when Unity closes, so all work done in the tool should be safe upon closing.


EXISTING ASSETS:

Assets/Resources/Gauntlet/GameSprites -> drag these sprites into prefab object sprite fields (these will be drawn)
Assets/Resources/Gauntlet/GameData -> scriptable objects for each game 
Assets/Resources/Gauntlet/LevelData -> scriptable objects for each level
Assets/Resources/Gauntlet/Prefabs -> scriptable objects for each prefab that can be drawn on the map

If there are any questions, just ask :)

Thanks!