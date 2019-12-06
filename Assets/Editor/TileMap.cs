using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TileMap
{
    public IMGUIContainer gridContainer;
    public GauntletLevelEditor levelEditor;
    public GauntletLevel level;

    int rowCount = 0;
    int columnCount = 0;
    float cellSize = 0;
    int numLayers = 3;
    int activeLayer = 0;
    List<List<bool>> mapCells;
    List<List<List<MapObject>>> mapObjects;
    public MapObject selectedObject;
    bool paintMode = false;
    Vector2Int lastPaintedCell;

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
    }

    public void createGrid(GauntletLevel level)
    {
        rowCount = level.rowCount;
        columnCount = level.columnCount;
        cellSize = level.cellSize;

        mapCells = new List<List<bool>>(rowCount);
        for (int r = 0; r < rowCount; r++)
        {
            mapCells.Add(new List<bool>(columnCount));
            for (int c = 0; c < columnCount; c++)
            {
                mapCells[r].Add(false);
            }
        }

        mapObjects = new List<List<List<MapObject>>>(numLayers);
        for (int l = 0; l < numLayers; l++)
        {
            mapObjects.Add(new List<List<MapObject>>(rowCount));
            for (int r = 0; r < rowCount; r++)
            {
                mapObjects[l].Add(new List<MapObject>(columnCount));
                for (int c = 0; c < columnCount; c++)
                {
                    mapObjects[l][r].Add(null);
                }
            }
        }
    }

    void drawGrid()
    {

        //for (int r = 0; r < mapCells.Count; r++)
        //{
        //    for (int c = 0; c < mapCells[r].Count; c++)
        //    {
        //        if (mapCells[r][c])
        //        {
        //            //EditorGUI.DrawRect(new Rect(c * cellSize, r * cellSize, cellSize, cellSize), Color.white);
        //            var rect = new Rect(c * cellSize, r * cellSize, cellSize, cellSize);
        //            //EditorGUI.DrawRect(rect, Color.white);
        //            //var cellimage = mapobjects[mapobjectlistview.selectedindex].mainsprite.texture;
        //            // if (cellimage)
        //            // {
        //            //    gui.drawtexture(rect, cellimage);
        //            //}
        //            //levelEditor.Repaint();
        //        }
        //    }
        //}

        for(int l = 0; l < mapObjects.Count; l++)
        {
            for (int r = 0; r < mapObjects[l].Count; r++)
            {
                for (int c = 0; c < mapObjects[l][r].Count; c++)
                {
                    if (mapObjects[l][r][c])
                    {
                        //EditorGUI.DrawRect(new Rect(c * cellSize, r * cellSize, cellSize, cellSize), Color.white);
                        var rect = new Rect(c * cellSize, r * cellSize, cellSize, cellSize);
                        var cellImage = mapObjects[l][r][c].mainSprite.texture;
                        if (cellImage)
                        {
                            GUI.DrawTexture(rect, cellImage);
                        }
                        levelEditor.Repaint();
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

        if(paintMode && !mouseCell.Equals(lastPaintedCell))
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
        if (cell.x < rowCount && cell.y < columnCount)
        {
            mapCells[cell.x][cell.y] = !mapCells[cell.x][cell.y];
        }

        if(selectedObject)
        {
            mapObjects[activeLayer][cell.x][cell.y] = selectedObject;
        }
    }

    public void changeLayer(int layerIndex)
    {
        if(layerIndex < numLayers && layerIndex >= 0)
        {
            activeLayer = layerIndex;
        }
    }
}
