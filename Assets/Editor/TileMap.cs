//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.UIElements;

//public class TileMap
//{
//    public IMGUIContainer gridContainer;
//    public GauntletLevelEditor levelEditor;
//    public GauntletLevel activeLevel;

//    int rowCount = 0;
//    int columnCount = 0;
//    float cellSize = 0;
//    int numLayers = 0;
//    int activeLayer = 0;
//    List<List<bool>> mapCells;
//    List<List<List<MapObject>>> mapObjects;
//    public MapObject selectedObject;
//    bool paintMode = false;
//    Vector2Int lastPaintedCell;

//    public TileMap()
//    {
//        gridContainer = new IMGUIContainer(drawGrid)
//        {
//            style =
//                {
//                    overflow = Overflow.Hidden,
//                    flexGrow = 1.0f,
//                    flexShrink = 0.0f,
//                    flexBasis = 0.0f,
//                    backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f)
//                }
//        };
//        gridContainer.RegisterCallback<MouseUpEvent>(mouseUpGridCallback);
//        gridContainer.RegisterCallback<MouseDownEvent>(mouseDownGridCallback);
//        gridContainer.RegisterCallback<MouseMoveEvent>(mouseMoveGridCallback);
//        gridContainer.RegisterCallback<MouseLeaveEvent>(mouseLeaveGridCallback);
//        mapObjects = new List<List<List<MapObject>>>();
//    }

//    private void resetGrid()
//    {
//        activeLevel = null;
//        mapObjects = new List<List<List<MapObject>>>();
//        rowCount = 0;
//        columnCount = 0;
//        cellSize = 0;
//        numLayers = 0;
//    }
//    public void createGrid(GauntletLevel level)
//    {
//        if(level == null)
//        {
//            resetGrid();
//            return;
//        }

//        rowCount = level.rowCount;
//        columnCount = level.columnCount;
//        cellSize = level.cellSize;
//        numLayers = level.numLayers;

//        mapCells = new List<List<bool>>(rowCount);
//        for (int r = 0; r < rowCount; r++)
//        {
//            mapCells.Add(new List<bool>(columnCount));
//            for (int c = 0; c < columnCount; c++)
//            {
//                mapCells[r].Add(false);
//            }
//        }
//        mapObjects = level.mapObjects;
//        activeLevel = level;
//    }

//    void drawGrid()
//    {

//        for (int l = 0; l < mapObjects.Count; l++)
//        {
//            for (int r = 0; r < mapObjects[l].Count; r++)
//            {
//                for (int c = 0; c < mapObjects[l][r].Count; c++)
//                {
//                    if (mapObjects[l][r][c])
//                    {
//                        //EditorGUI.DrawRect(new Rect(c * cellSize, r * cellSize, cellSize, cellSize), Color.white);
//                        var rect = new Rect(c * cellSize, r * cellSize, cellSize, cellSize);
//                        var cellImage = mapObjects[l][r][c].mainSprite.texture;
//                        if (cellImage)
//                        {
//                            GUI.DrawTexture(rect, cellImage);
//                        }
//                        levelEditor.Repaint();
//                    }
//                }
//            }
//        }


//        Color color = new Color(1, 1, 1, 0.1f);

//        for (int r = 0; r < rowCount + 1; r++)
//        {
//            EditorGUI.DrawRect(new Rect(new Vector2(0, r * cellSize), new Vector2(columnCount * cellSize, 1)), color);
//        }

//        for (int c = 0; c < columnCount + 1; c++)
//        {
//            EditorGUI.DrawRect(new Rect(new Vector2(c * cellSize, 0), new Vector2(1, columnCount * cellSize)), color);
//        }
//    }


//    void mouseDownGridCallback(MouseDownEvent evt)
//    {
//        paintMode = true;
//        lastPaintedCell = new Vector2Int(-1, -1);
//        //drawInCell(new Vector2(evt.localMousePosition.x, evt.localMousePosition.y));

//    }
//    void mouseUpGridCallback(MouseUpEvent evt)
//    {
//        float mousePosX = evt.localMousePosition.x;
//        float mousePosY = evt.localMousePosition.y;
//        Vector2Int mouseCell = getMouseCell(new Vector2(mousePosX, mousePosY));

//        if (paintMode && !mouseCell.Equals(lastPaintedCell))
//        {
//            drawInCell(mouseCell);
//            lastPaintedCell = mouseCell;
//        }
//        paintMode = false;
//        //drawInCell(new Vector2(evt.localMousePosition.x, evt.localMousePosition.y));
//    }
//    void mouseMoveGridCallback(MouseMoveEvent evt)
//    {
//        float mousePosX = evt.localMousePosition.x;
//        float mousePosY = evt.localMousePosition.y;
//        Vector2Int mouseCell = getMouseCell(new Vector2(mousePosX, mousePosY));

//        if(paintMode && !mouseCell.Equals(lastPaintedCell))
//        {
//            drawInCell(mouseCell);
//            lastPaintedCell = mouseCell;
//        }
//    }
//    void mouseLeaveGridCallback(MouseLeaveEvent evt)
//    {
//        paintMode = false;
//    }


//    Vector2Int getMouseCell(Vector2 mousePos)
//    {
//        int rowIndex = Mathf.FloorToInt(mousePos.y / cellSize);
//        int colIndex = Mathf.FloorToInt(mousePos.x / cellSize);
//        return new Vector2Int(rowIndex, colIndex);
//    }

//    void drawInCell(Vector2Int cell)
//    {
//        if (cell.x < rowCount && cell.y < columnCount)
//        {
//            mapCells[cell.x][cell.y] = !mapCells[cell.x][cell.y];
//        }

//        if(selectedObject && activeLevel)
//        {
//            mapObjects[activeLayer][cell.x][cell.y] = selectedObject;
//            EditorUtility.SetDirty(activeLevel);
//            //AssetDatabase.SaveAssets();
//            //AssetDatabase.Refresh();
//        }
//    }

//    public void changeLayer(int layerIndex)
//    {
//        if(layerIndex < numLayers && layerIndex >= 0)
//        {
//            activeLayer = layerIndex;
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TileMap
{
    public IMGUIContainer gridContainer;
    public GauntletLevelEditor levelEditor;
    public GauntletLevel activeLevel;

    int rowCount = 0;
    int columnCount = 0;
    float cellSize = 0;
    int numLayers = 0;
    int activeLayer = 0;
    List<ListLayerWrapper> mapObjects;
    public MapObject selectedObject;
    bool paintMode = false;
    Vector2Int lastPaintedCell;

    public List<bool> layersOn;

    public TileMap()
    {
        gridContainer = new IMGUIContainer(drawGrid)
        {
            style =
                {
                    overflow = Overflow.Hidden,
                    flexGrow = 1.0f,
                    flexShrink = 0.0f,
                    flexBasis = 0.0f,
                    backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f)
                }
        };
        gridContainer.RegisterCallback<MouseUpEvent>(mouseUpGridCallback);
        gridContainer.RegisterCallback<MouseDownEvent>(mouseDownGridCallback);
        gridContainer.RegisterCallback<MouseMoveEvent>(mouseMoveGridCallback);
        gridContainer.RegisterCallback<MouseLeaveEvent>(mouseLeaveGridCallback);
        mapObjects = new List<ListLayerWrapper>();

        layersOn = new List<bool>() { true, true, true };
    }

    private void resetGrid()
    {
        activeLevel = null;
        mapObjects = new List<ListLayerWrapper>();
        rowCount = 0;
        columnCount = 0;
        cellSize = 0;
        numLayers = 0;
    }
    public void createGrid(GauntletLevel level)
    {
        if (level == null)
        {
            resetGrid();
            return;
        }

        rowCount = level.rowCount;
        columnCount = level.columnCount;
        cellSize = level.cellSize;
        numLayers = level.numLayers;

        mapObjects = level.levelLayers;
        activeLevel = level;
    }

    void drawGrid()
    {
        for (int l = 0; l < numLayers; l++)
        {
            if(layersOn[l])
            {
                var layer = mapObjects[l];
                for (int r = 0; r < rowCount; r++)
                {
                    var row = layer.rows[r];
                    for (int c = 0; c < columnCount; c++)
                    {
                        if (row.mapObjs[c])
                        {
                            var rect = new Rect(c * cellSize, r * cellSize, cellSize, cellSize);
                            var cellImage = row.mapObjs[c].mainSprite.texture;
                            if (cellImage)
                            {
                                GUI.DrawTexture(rect, cellImage);
                            }
                            levelEditor.Repaint();
                        }
                    }
                }
            }

        }



        Color color = new Color(1, 1, 1, 0.1f);

        for (int r = 0; r < rowCount + 1; r++)
        {
            EditorGUI.DrawRect(new Rect(new Vector2(0, r * cellSize), new Vector2(columnCount * cellSize, 1)), color);
        }

        for (int c = 0; c < columnCount + 1; c++)
        {
            EditorGUI.DrawRect(new Rect(new Vector2(c * cellSize, 0), new Vector2(1, columnCount * cellSize)), color);
        }
    }


    void mouseDownGridCallback(MouseDownEvent evt)
    {
        paintMode = true;
        lastPaintedCell = new Vector2Int(-1, -1);
        //drawInCell(new Vector2(evt.localMousePosition.x, evt.localMousePosition.y));

    }
    void mouseUpGridCallback(MouseUpEvent evt)
    {
        float mousePosX = evt.localMousePosition.x;
        float mousePosY = evt.localMousePosition.y;
        Vector2Int mouseCell = getMouseCell(new Vector2(mousePosX, mousePosY));

        if (paintMode && !mouseCell.Equals(lastPaintedCell))
        {
            drawInCell(mouseCell);
            lastPaintedCell = mouseCell;
        }
        paintMode = false;
        //drawInCell(new Vector2(evt.localMousePosition.x, evt.localMousePosition.y));
    }
    void mouseMoveGridCallback(MouseMoveEvent evt)
    {
        float mousePosX = evt.localMousePosition.x;
        float mousePosY = evt.localMousePosition.y;
        Vector2Int mouseCell = getMouseCell(new Vector2(mousePosX, mousePosY));

        if (paintMode && !mouseCell.Equals(lastPaintedCell))
        {
            drawInCell(mouseCell);
            lastPaintedCell = mouseCell;
        }
    }
    void mouseLeaveGridCallback(MouseLeaveEvent evt)
    {
        paintMode = false;
    }


    Vector2Int getMouseCell(Vector2 mousePos)
    {
        int rowIndex = Mathf.FloorToInt(mousePos.y / cellSize);
        int colIndex = Mathf.FloorToInt(mousePos.x / cellSize);
        return new Vector2Int(rowIndex, colIndex);
    }

    void drawInCell(Vector2Int cell)
    {

        if (selectedObject && activeLevel)
        {
            mapObjects[activeLayer].rows[cell.x].mapObjs[cell.y] = selectedObject;
            EditorUtility.SetDirty(activeLevel);
            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();
        }
    }

    public void changeLayer(int layerIndex)
    {
        if (layerIndex < numLayers && layerIndex >= 0)
        {
            activeLayer = layerIndex;
        }
    }
}
