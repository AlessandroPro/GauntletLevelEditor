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
    public bool eraseMode = false;
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
                            var cellSprite = row.mapObjs[c].mainSprite;
                            if (cellSprite)
                            {
                                Vector2 ratio;
                                ratio.x = rect.width / cellSprite.rect.width;
                                ratio.y = cellSprite.rect.height;
                                float minRatio = Mathf.Min(ratio.x, ratio.y);

                                rect.width = cellSprite.rect.width * minRatio;
                                rect.height = cellSprite.rect.height * minRatio;

                                GUI.DrawTextureWithTexCoords(rect, cellSprite.texture, getSpriteCoordsInTexture(cellSprite));
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

    Rect getSpriteCoordsInTexture(Sprite sprite)
    {
        Rect coords = sprite.rect;
        coords.x /= sprite.texture.width;
        coords.width /= sprite.texture.width;
        coords.y /= sprite.texture.height;
        coords.height /= sprite.texture.height;

        return coords;
    }

    void mouseDownGridCallback(MouseDownEvent evt)
    {
        paintMode = true;
        lastPaintedCell = new Vector2Int(-1, -1);
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
        if(cell.x >= rowCount || cell.y >= columnCount)
        {
            return;
        }

        if (selectedObject && activeLevel)
        {
            if(eraseMode)
            {
                mapObjects[activeLayer].rows[cell.x].mapObjs[cell.y] = null;
            }
            else
            {
                mapObjects[activeLayer].rows[cell.x].mapObjs[cell.y] = selectedObject;
            }
            EditorUtility.SetDirty(activeLevel);
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
