# Gauntlet Level Editor

Imported from GitLab. <br>
All relevant C# code is in /Assets/Editor/.

This is a C# Unity Editor Tool used for designing, drawing, and exporting 2D level maps based on the classic Gauntlet video game: <br>
https://www.youtube.com/watch?v=7mMJio2MO6w 

This tool includes windows for creating and designing prefabs (enemies, spawn factories, items, ground tiles, etc.) and a map editor window for designing levels through the placement of these prefabs. With the click of a button, all maps will be bundled together and exported (including all sprite assets and object data in the form of json files), to be easily loaded into a separately written C++ Gauntlet game. <br>
Map and prefab data is managed internally through scriptable objects that are saved within the Unity project, in case you want to close Unity and open it again to continue working another time.
This repo already contains a few maps and prefabs already created, saved under Assets/Resources/Gauntlet/.

Main Window:
![Screenshot1](https://user-images.githubusercontent.com/15040875/72028892-33716d00-3252-11ea-9042-3162c9b3872c.PNG)

### How to use this tool:

Go to Tools -> Gauntlet Level Editor to launch the editor.

To start editing a level, select an existing level from the object field in the left pane, or create a new one with the 
"Create New Level" button of the size of your choosing. You can also select an existing game, and open one of its levels to edit.

There is currently one game in this repo, with 4 levels. Add levels to the game using the buttons on the left. You can have more than one "game", each with its own list of levels. If you create any new objects (game, levels, prefabs), they will automatically be put in the correct folders.

Use the "Create/Edit Prefab" button in the right to open a sub window depending on your selection.
From this window, you can create or select a prefab scriptable object and select its sprite, and edit its information, then you can use thatsprite to draw on the map.

### DRAWING:
Select a prefab from the list in the right pane, and click or drag along tiles to paint on the map. Check the 'Erase Mode'
box to erase the tiles you click or drag on.
There are 3 layers, rendered in order. You can choose the layer to paint on. You can also choose which layers are visible.

### EXPORTING (to the game):
You must have a "Game" scriptable object loaded in the top left object field in order to export, as well as a player in the player field in the left pane.
A "Game" consists of a list of levels and one player object. All of the map objects are being saved when exporting, along with the player in the first level.

When the "Save Game Data" button is clicked, a file dialog will open asking for a save directory (not file).
You MUST choose the Assets folder of where a C++ Gauntlet game project resides, within it's GauntletGame/Build/Assets folder.
So, you'll need a C++ Gauntlet game to test exporting and loading into the game. <br>
Once the folder is selected, the export will begin:
- a level json file will be created for each level in the game and put into the /Levels folder.
- A json file with the list of levels in the game will also be placed in the /Levels folder.
- Textures and prefabs, and their meta files, will be created/copied into their respective /Prefabs or /Images folder.
Files with the same name will be overwritten.

With an instance of the Gauntlet Game, all you need to do is run the game and it should play as expected.

### SAVING (tool data):
All data within the scriptable objects are saved, even when Unity closes, so all work done in the tool should be safe upon closing.


### EXISTING ASSETS:

Assets/Resources/Gauntlet/GameSprites -> drag these sprites into prefab object sprite fields (these will be drawn)
Assets/Resources/Gauntlet/GameData -> scriptable objects for each game 
Assets/Resources/Gauntlet/LevelData -> scriptable objects for each level
Assets/Resources/Gauntlet/Prefabs -> scriptable objects for each prefab that can be drawn on the map

The following screenshots show the main level editor window with the right side popup window for creating/editing prefabs.

![Screenshot2](https://user-images.githubusercontent.com/15040875/72029286-6f590200-3253-11ea-8264-a0d70dbf5e4c.PNG)

![Screenshot3](https://user-images.githubusercontent.com/15040875/72029299-7aac2d80-3253-11ea-8145-10142def5d8a.PNG)


