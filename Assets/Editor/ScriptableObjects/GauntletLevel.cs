using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauntletLevel
{
    public string levelName;
    public int size;

    public int rowCount;
    public int columnCount;
    public int cellSize;

    public GauntletLevel(string _levelName, int _size, int mapWidth)
    {
        levelName = _levelName;
        size = _size;

        rowCount = size;
        columnCount = size;

        cellSize = mapWidth / size;
    }
}
