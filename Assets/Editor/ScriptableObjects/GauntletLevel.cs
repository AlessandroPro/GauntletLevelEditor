using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GauntletLevel : ScriptableObject
{
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
        levelData.resources.Add("test1");
        levelData.resources.Add("test2");

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
                        transform.position.X = r * cellSize;
                        transform.position.Y = c * cellSize;
                        levelData.GameObjects.Add(gameObject);
                    }
                }
            }
        }

        return levelData;
    }
}
