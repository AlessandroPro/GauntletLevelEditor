using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GauntletLevel : ScriptableObject
{
    private const int gameCellSize = 64;

    public string levelName = "Level";
    public int size = 0;
    public int timeLimit = 60;

    public int rowCount = 0;
    public int columnCount = 0;
    public int cellSize = 0;
    public int numLayers = 0;

    public List<ListLayerWrapper> levelLayers = new List<ListLayerWrapper>();

    public void initialize(string _levelName, int _size, int mapWidth)
    {
        levelName = _levelName;
        size = _size;

        rowCount = size;
        columnCount = size;
        numLayers = 3;

        cellSize = mapWidth / size;

        for (int l = 0; l < numLayers; l++)
        {
            var layer = new ListLayerWrapper();
            levelLayers.Add(layer);
            for (int r = 0; r < rowCount; r++)
            {
                var row = new ListRowWrapper();
                layer.rows.Add(row);
                for (int c = 0; c < columnCount; c++)
                {
                    row.mapObjs.Add(null);
                }
            }
        }
    }

    public Game.Level saveLevel()
    {
        Game.Level levelData = new Game.Level();
        levelData.TimeLimit = timeLimit;
        levelData.resources.Add("../Assets/Images/Dungeon.png.meta");
        levelData.resources.Add("../Assets/Images/Witch.png.meta");

        for (int l = 0; l < numLayers; l++)
        {
            var layer = levelLayers[l];
            for (int r = 0; r < rowCount; r++)
            {
                var row = layer.rows[r];
                for (int c = 0; c < columnCount; c++)
                {
                    if (row.mapObjs[c])
                    {
                        Game.GameObject gameObject = row.mapObjs[c].save();

                        Game.Transform transform = new Game.Transform();
                        transform.Position.X = c * gameCellSize;
                        transform.Position.Y = r * gameCellSize;
                        gameObject.Components.Add(transform);

                        Game.Sprite sprite = new Game.Sprite();
                        Rect spriteRect = row.mapObjs[c].mainSprite.rect;
                        sprite.Dimensions.Left = (int) spriteRect.x;
                        sprite.Dimensions.Top = (int) (row.mapObjs[c].mainSprite.texture.height - spriteRect.y - spriteRect.height);
                        sprite.layer = l;
                        gameObject.Components.Add(sprite);

                        gameObject.name = row.mapObjs[c].objectName;

                        levelData.GameObjects.Add(gameObject);
                    }
                }
            }
        }

        return levelData;
    }
}
