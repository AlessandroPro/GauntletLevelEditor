using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;


public enum MapAssetTypes
{
    GroundTile,
    Item,
    SpawnFactory
}

public enum MapLayers
{
    Layer1,
    Layer2,
    Layer3
}

public enum AssetTypes
{
    Enemy,
    Player,
    SpawnFactory,
    Item,
    GroundTile
}

public class GauntletLevelEditor : EditorWindow
{
    static GauntletLevelEditor _window = null;
    public EditorWindow assetWindow = null;

    GauntletGame game = null;
    GauntletLevel level = null;

    List<MapObject> mapObjects;
    ListView mapObjectListView;

    TileMap tileMap;

    static float width = 1000;
    static float height = 600;

    [Shortcut("Refresh Gauntlet Level Editor", KeyCode.F10)] 
    [MenuItem("Tools/Gauntlet Level Editor")]
    public static void createWindow()
    {
        if (_window != null) _window.Close();
        _window = GetWindow<GauntletLevelEditor>();
        _window.titleContent = new GUIContent("Gauntlet Level Editor");
        _window.maxSize = new Vector2(width, height);
        _window.minSize = new Vector2(width, height);
        _window.Repaint();
    }

    private void Update()
    {
        if(assetWindow)
        {
           var assetWindowPos = new Vector2(_window.position.x + _window.position.width, _window.position.y);
           assetWindow.position = new Rect(assetWindowPos, assetWindow.position.size);
        }
    }

    public void OnEnable()
    {
        VisualElement root = this.rootVisualElement;
        root.style.flexDirection = FlexDirection.Row;
        root.style.paddingTop = 10;
        root.style.paddingBottom = 10;
        root.style.paddingLeft = 10;
        root.style.paddingRight = 10;


        VisualElement gameRoot = new VisualElement()
        {
            style =
            {
                width = _window.maxSize.x * 0.2f,
                paddingRight = 10
            }
        };
        VisualElement mapRoot = new VisualElement()
        {
            style =
            {
                alignContent = Align.Center,
                width = _window.maxSize.x * 0.6f
            }
        };
        VisualElement levelRoot = new VisualElement()
        {
            style =
            {
                width = _window.maxSize.x * 0.2f,
                paddingLeft = 10
            }
        };

        root.Add(gameRoot);
        root.Add(mapRoot);
        root.Add(levelRoot);

        //game root
        {
            gameRoot.Add(new Label("Choose a Gauntlet Game:"));
            ObjectField gameData = new ObjectField();
            gameData.objectType = typeof(GauntletGame);
            gameRoot.Add(gameData);

            Button saveDataButton = new Button();
            saveDataButton.text = "Save Game Data";
            gameRoot.Add(saveDataButton);



            VisualElement levelListDataRoot = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    paddingTop = 20,
                    paddingBottom = 40,
                    height = 300
                }
            };

            VisualElement levelListRoot = new VisualElement()
            {
                style =
                {
                    flexGrow = 0.75f
                }
            };

            VisualElement levelButtonsRoot = new VisualElement()
            {
                style =
                {
                    flexGrow = 0.25f,
                    justifyContent = Justify.Center,
                    paddingLeft = 10,
                    paddingRight = 10
                }
            };

            levelListDataRoot.Add(levelListRoot);
            levelListDataRoot.Add(levelButtonsRoot);
            gameRoot.Add(levelListDataRoot);

            Label levelLabel = new Label("List of Levels:");
            levelListRoot.Add(levelLabel);
            levelListRoot.Add(createLevelList());

            Button upButton = new Button();
            Button downButton = new Button();
            Button removeButton = new Button();
            upButton.text = "Move Up";
            downButton.text = "Move Down";
            removeButton.text = "Remove";
            levelButtonsRoot.Add(upButton);
            levelButtonsRoot.Add(downButton);
            levelButtonsRoot.Add(removeButton);

            // Level 
            gameRoot.Add(new Label("Choose a Level to edit:"));
            ObjectField gauntletLevel = new ObjectField();
            gauntletLevel.objectType = typeof(GauntletLevel);
            gameRoot.Add(gauntletLevel);
            gameRoot.Add(new Label("Level Name:"));
            gameRoot.Add(new TextField());
            Button addLevelButton = new Button();
            addLevelButton.text = "Add Level To Game";
            gameRoot.Add(addLevelButton);

            // New Level
            Button newLevelButton = new Button()
            {
                text = "Create New Level",
                style =
                {
                    marginTop = 40
                }
            };
            gameRoot.Add(newLevelButton);
            gameRoot.Add(new Label("Size of New Level:"));
            gameRoot.Add(new SliderInt(2, 50));
        }


        // map root
        GauntletLevel testLevel = new GauntletLevel("TestLevel", 20, 590); // will be deleted later, used for testing
        level = testLevel;

        tileMap = new TileMap();
        tileMap.createGrid(level);
        tileMap.levelEditor = this;
        mapRoot.Add(tileMap.gridContainer);


        // Level Root
        levelRoot.Add(new Label("Choose an asset to place:"));
        EnumField mapAssetTypes = new EnumField(MapAssetTypes.GroundTile);
        levelRoot.Add(mapAssetTypes);
        //levelRoot.Add(createSpriteList());
        levelRoot.Add(createMapTileList());

        levelRoot.Add(new Toggle("Set as start point"));
        levelRoot.Add(new Toggle("Set as portal to next level"));
        IntegerField timeLimit = new IntegerField("Time Limit (s):");
        levelRoot.Add(timeLimit);
        levelRoot.Add(new Label("Current Layer:"));

        EnumField mapLayers = new EnumField(MapLayers.Layer1);
        mapLayers.RegisterCallback<ChangeEvent<Enum>>((evt) =>
        {
            var change = evt.newValue;
            tileMap.changeLayer(Convert.ToInt32(change));
        });

        levelRoot.Add(mapLayers);
        levelRoot.Add(new Label("Viewable layers in editor:"));
        levelRoot.Add(new Toggle("Layer 1"));
        levelRoot.Add(new Toggle("Layer 2"));
        levelRoot.Add(new Toggle("Layer 3"));


        //createEditAssetButton.style.marginTop = 40;
        
        levelRoot.Add(new Label("Type of asset to create/edit:"));
        EnumField assetEnums = new EnumField(AssetTypes.Enemy);
        levelRoot.Add(assetEnums);
        Button createEditAssetButton = new Button(() =>
        {
            switch (assetEnums.value)
            {
                case AssetTypes.Player:
                    {
                        GauntletPlayerEditor.createWindow();
                        break;
                    }
                case AssetTypes.Enemy:
                    {
                        GauntletEnemyEditor.createWindow();
                        break;
                    }
                case AssetTypes.GroundTile:
                    {
                        assetWindow = GauntletGroundTileEditor.createWindow(_window);
                        break;
                    }
                case AssetTypes.SpawnFactory:
                    {
                        GauntletSpawnFactoryEditor.createWindow();
                        break;
                    }
                case AssetTypes.Item:
                    {
                        GauntletItemEditor.createWindow();
                        break;
                    }
                default: break;
            }


        });
        createEditAssetButton.text = "Create/Edit Asset";
        levelRoot.Add(createEditAssetButton);

    }


    public ListView createLevelList()
    {
        const int itemCount = 10;
        var items = new List<GauntletLevel>(itemCount);
        for (int i = 1; i <= itemCount; i++)
        {
            GauntletLevel level = new GauntletLevel("Level " + i.ToString(), 30, 300);
            items.Add(level);
        }

        Func<VisualElement> makeItem = () => new Label()
        {
            style =
                {
                    paddingTop = 5f,
                    paddingBottom = 5f,
                    paddingLeft = 10f,
                    paddingRight = 10f,
                }
        };

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            (e as Label).text = items[i].levelName;
        };

        // Provide the list view with an explict height for every row
        // so it can calculate how many items to actually display
        const int itemHeight = 40;

        var listView = new ListView(items, itemHeight, makeItem, bindItem)
        {
            style =
            {
                backgroundColor = Color.grey
            }
        };

        listView.selectionType = SelectionType.Single;

        listView.style.flexGrow = 1.0f;

        return listView;
    }

    public ListView createSpriteList()
    {
        Texture2D tex = new Texture2D(50, 50);
        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                Color color = new Color(x / 255f, y / 255f, 0);
                tex.SetPixel(x, y, color);
            }
        }
        tex.Apply();

        var mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        // Create some list of data, here simply numbers in interval [1, 1000]
        const int itemCount = 20;
        var items = new List<Sprite>(itemCount);
        for (int i = 1; i <= itemCount; i++)
            items.Add(mySprite);

        // The "makeItem" function will be called as needed
        // when the ListView needs more items to render
        Func<VisualElement> makeItem = () => new Image()
        {
            style =
                {
                    paddingTop = 10f,
                    paddingBottom = 10f,
                    paddingLeft = 10f,
                    paddingRight = 10f,
                },
            scaleMode = ScaleMode.ScaleToFit
            
        };

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            (e as Image).image = items[i].texture;
        };

        // Provide the list view with an explict height for every row
        // so it can calculate how many items to actually display
        const int itemHeight = 70;

        var listView = new ListView(items, itemHeight, makeItem, bindItem)
        {
            style =
            {
                backgroundColor = Color.gray
            }
        };

        listView.selectionType = SelectionType.Single;


        listView.style.flexGrow = 1f;

        return listView;
    }

    public ListView createMapTileList()
    {
        mapObjects = new List<MapObject>();
        UnityEngine.Object[] objects = Resources.LoadAll<GroundTile>("Prefabs/GroundTiles");
        for (int i = 0; i < objects.Length; i++)
            mapObjects.Add(objects[i] as MapObject);

        // The "makeItem" function will be called as needed
        // when the ListView needs more items to render
        Func<VisualElement> makeItem = () => new Image()
        {
            style =
                {
                    width = 50,
                    height = 50,
                    justifyContent = Justify.Center,
                    alignSelf = Align.Center,
                    paddingTop = 10f,
                    paddingBottom = 10f,
                    paddingLeft = 10f,
                    paddingRight = 10f,
                },
            scaleMode = ScaleMode.ScaleToFit

        };

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            (e as Image).image = mapObjects[i].mainSprite.texture;
        };

        // Provide the list view with an explict height for every row
        // so it can calculate how many items to actually display
        const int itemHeight = 70;

        mapObjectListView = new ListView(mapObjects, itemHeight, makeItem, bindItem)
        {
            style =
            {
                backgroundColor = Color.gray
            }
        };

        mapObjectListView.selectionType = SelectionType.Single;

        mapObjectListView.style.flexGrow = 1f;

        mapObjectListView.onSelectionChanged += selections =>
        {
            if(selections.Count > 0)
            {
                if(tileMap != null)
                {
                    tileMap.selectedObject = selections[0] as MapObject;
                }
            }

            else
            {
                tileMap.selectedObject = null;
            }
        };

        return mapObjectListView;
    }

    private void OnDestroy()
    {
        if(assetWindow != null)
        {
            assetWindow.Close();
        }
    }
}


