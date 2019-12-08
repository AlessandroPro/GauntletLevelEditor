Alessandro Profenna
PROG56693 Exercise 4

This submission is all of the work I've done on my tool up until now, which includes drawing.

Go to Tools -> Gauntlet Level Editor to launch the editor.

To start editing a level, select an existing level from the object field in the left pane, or create a new one with the 
"Create New Level" button of the size of your choosing. You can also select an existing game, and open one of its levels to edit.
There is currently one game, with 3 levels. The third level already has some stuff in its map.
If you create any new objects (game, levels, prefabs), they will automatically be put in the correct folders.

Everything in the main level editor window works for the most part. For the "Create/Edit Prefab" button in the right,
a sub window will open depending on your selection, but right now only the Ground Tile window will open.
From this window, you can create or select a Ground Tile scriptable object and select it's sprite, then you can use that
sprite to draw on the map.

DRAWING:
Select a prefab from the list in the right pane, and click or drag along tiles to paint on the map. Check the 'Erase Mode'
box to erase the tiles you click or drag on.
There are 3 layers, rendered in order. You can choose the layer to paint on. You can also choose which layers are visible.


EXISTING ASSETS:

Assets/Resources/Gauntlet/GameSprites -> drag these sprites to create objects for the map

Assets/Resources/Gauntlet/GameData -> scriptable objects for each game (there is one right now)

Assets/Resources/Gauntlet/LevelData -> scriptable objects for each level (there are three now)

Assets/Resources/Gauntlet/Prefabs -> scriptable objects for each prefab that can be drawn on the map (there are nine now)

