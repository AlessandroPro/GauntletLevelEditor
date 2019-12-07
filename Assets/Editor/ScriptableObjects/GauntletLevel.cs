using System.Collections;
using System.Collections.Generic;
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

    //public List<List<List<MapObject>>> mapObjects;

    public List<ListLayerWrapper> levelLayers = new List<ListLayerWrapper>();

    //public void initialize(string _levelName, int _size, int mapWidth)
    //{
    //    levelName = _levelName;
    //    size = _size;

    //    rowCount = size;
    //    columnCount = size;
    //    numLayers = 3;

    //    cellSize = mapWidth / size;

    //    mapObjects = new List<List<List<MapObject>>>(numLayers);
    //    for (int l = 0; l < numLayers; l++)
    //    {
    //        mapObjects.Add(new List<List<MapObject>>(rowCount));
    //        for (int r = 0; r < rowCount; r++)
    //        {
    //            mapObjects[l].Add(new List<MapObject>(columnCount));
    //            for (int c = 0; c < columnCount; c++)
    //            {
    //                mapObjects[l][r].Add(null);
    //            }
    //        }
    //    }
    //}

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
}
