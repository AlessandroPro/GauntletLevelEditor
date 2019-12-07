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

    ObjectField gameData;
    ObjectField levelData;
    ListView levelListView;
    SliderInt levelSizeSlider;
    EnumField mapAssetTypes;

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
            gameData = new ObjectField();
            gameData.objectType = typeof(GauntletGame);
            gameRoot.Add(gameData);

            gameData.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
            {
                var change = (evt.target as ObjectField).value;
                game = change as GauntletGame;
                levelData.value = null;
                rebindLevelListView();
            });

            Button createGameButton = new Button(() =>
            {
                var newGame = CreateInstance<GauntletGame>();
                var path = "Assets/Resources/Gauntlet/GameData";
                AssetDatabase.CreateAsset(newGame, AssetDatabase.GenerateUniqueAssetPath(path + "/GameData-00.asset"));
                EditorUtility.SetDirty(newGame);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                gameData.value = newGame;
            });
            createGameButton.text = "Create New Game";
            gameRoot.Add(createGameButton);


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
            levelListRoot.Add(createLevelListView());

            Button upButton = new Button(() =>
            {
                if (game != null)
                {
                    int newIndex = game.changeLevelOrder(levelListView.selectedIndex, true);
                    rebindLevelListView();
                    levelListView.selectedIndex = newIndex;
                }
            });
            Button downButton = new Button(() =>
            {
                if (game != null)
                {
                    int newIndex = game.changeLevelOrder(levelListView.selectedIndex, false);
                    rebindLevelListView();
                    levelListView.selectedIndex = newIndex;
                }
            });
            Button removeButton = new Button(() =>
            {
                if (game != null && levelListView.selectedIndex >= 0)
                {
                    game.levels.RemoveAt(levelListView.selectedIndex);
                    rebindLevelListView();
                }
            });
            Button openButton = new Button(() =>
            {
                if (game != null && levelListView.selectedIndex >= 0)
                {
                    levelData.value = game.levels[levelListView.selectedIndex];
                }
            });
            upButton.text = "Move Up";
            downButton.text = "Move Down";
            removeButton.text = "Remove";
            openButton.text = "Open";
            levelButtonsRoot.Add(upButton);
            levelButtonsRoot.Add(downButton);
            levelButtonsRoot.Add(removeButton);
            levelButtonsRoot.Add(openButton);

            // Level 
            gameRoot.Add(new Label("Choose a Level to edit:"));
            levelData = new ObjectField();
            levelData.objectType = typeof(GauntletLevel);
            gameRoot.Add(levelData);
            gameRoot.Add(new Label("Level Name:"));

            levelData.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
            {
                var change = (evt.target as ObjectField).value;
                level = change as GauntletLevel;
                UpdateLevelBinding();
                tileMap.createGrid(level);
            });

            var levelName = new TextField();
            levelName.bindingPath = "levelName";
            gameRoot.Add(levelName);
            Button addLevelButton = new Button(() =>
            {
                if(game != null && level != null)
                {
                    game.levels.Add(level);
                    EditorUtility.SetDirty(game);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    rebindLevelListView();
                }
            });
            addLevelButton.text = "Add Level To Game";
            gameRoot.Add(addLevelButton);

            // New Level
            Button newLevelButton = new Button(() =>
            {
                var newLevel = CreateInstance<GauntletLevel>();
                newLevel.initialize("TestLevel", levelSizeSlider.value, 590);
                var path = "Assets/Resources/Gauntlet/LevelData";
                AssetDatabase.CreateAsset(newLevel, AssetDatabase.GenerateUniqueAssetPath(path + "/LevelData-00.asset"));
                EditorUtility.SetDirty(newLevel);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                levelData.value = newLevel;
            });

            newLevelButton.text = "Create New Level";
            newLevelButton.style.marginTop = 40;
            gameRoot.Add(newLevelButton);

            levelSizeSlider = new SliderInt(5, 35);
            levelSizeSlider.value = 20;
            var levelSizeLabel = new Label("Size of New Level:" + levelSizeSlider.value);
            gameRoot.Add(levelSizeLabel);
            levelSizeSlider.RegisterCallback<ChangeEvent<int>>((evt) =>
            {
                levelSizeLabel.text = "Size of New Level:  " + evt.newValue;
            });

            gameRoot.Add(levelSizeSlider);
        }


        // map root
        //GauntletLevel testLevel = new GauntletLevel("TestLevel", 20, 590); // will be deleted later, used for testing
        //level = testLevel;

        tileMap = new TileMap();
        //tileMap.createGrid(level);
        tileMap.levelEditor = this;
        mapRoot.Add(tileMap.gridContainer);


        // Level Root
        levelRoot.Add(new Label("Choose an asset to place:"));
        mapAssetTypes = new EnumField(MapAssetTypes.GroundTile);
        mapAssetTypes.RegisterCallback<ChangeEvent<Enum>>((evt) =>
        {
            rebindPrefabListView();
        });
        levelRoot.Add(mapAssetTypes);
        //levelRoot.Add(createSpriteList());
        levelRoot.Add(createMapTileList());

        levelRoot.Add(new Toggle("Set as start point"));
        levelRoot.Add(new Toggle("Set as portal to next level"));
        var timeLimit = new SliderInt(60, 600);
        var timeLimitLabel = new Label("Time Limit:   " + timeLimit.value + " seconds");
        levelRoot.Add(timeLimitLabel);
        timeLimit.RegisterCallback<ChangeEvent<int>>((evt) =>
        {
            timeLimitLabel.text = "Time Limit:   " + evt.newValue + " seconds";
        });
        timeLimit.bindingPath = "timeLimit";
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
        var layer1Toggle = new Toggle("Layer 1") { value = true };
        var layer2Toggle = new Toggle("Layer 2") { value = true }; 
        var layer3Toggle = new Toggle("Layer 3") { value = true }; 

        layer1Toggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
        {
            tileMap.layersOn[0] = evt.newValue;
        });
        
        layer2Toggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
        {
            tileMap.layersOn[1] = evt.newValue;
        });
        
        layer3Toggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
        {
            tileMap.layersOn[2] = evt.newValue;
        });

        levelRoot.Add(layer1Toggle);
        levelRoot.Add(layer2Toggle);
        levelRoot.Add(layer3Toggle);


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


    public ListView createLevelListView()
    {
        //const int itemCount = 10;
        var items = new List<GauntletLevel>();
        //for (int i = 1; i <= itemCount; i++)
        //{
        //    levelData level = new levelData("Level " + i.ToString(), 30, 300);
        //    items.Add(level);
        //}

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
            if(game)
            {
                (e as Label).text = game.levels[i].levelName;
            }
        };

        // Provide the list view with an explict height for every row
        // so it can calculate how many items to actually display
        const int itemHeight = 40;


        levelListView = new ListView(items, itemHeight, makeItem, bindItem)
        {
            style =
            {
                backgroundColor = Color.grey
            }
        };


        levelListView.selectionType = SelectionType.Single;

        levelListView.style.flexGrow = 1.0f;
        return levelListView;
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
        UnityEngine.Object[] objects = Resources.LoadAll<GroundTile>("Gauntlet/Prefabs/GroundTiles");
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

    private void rebindLevelListView()
    {
        if(game != null && levelListView != null)
        {
            levelListView.itemsSource = game.levels;
            levelListView.Refresh();
            Repaint();
        }
    }

    public void rebindPrefabListView()
    {
        string prefabDirectory = "";
        if(mapObjectListView != null)
        {
            switch (mapAssetTypes.value)
            {
                case MapAssetTypes.GroundTile:
                    {
                        prefabDirectory = "GroundTiles";
                        break;
                    }
                case MapAssetTypes.Item:
                    {
                        prefabDirectory = "Items";
                        break;
                    }
                case MapAssetTypes.SpawnFactory:
                    {
                        prefabDirectory = "SpawnFactories";
                        break;
                    }
                default: break;
            }
            mapObjects = new List<MapObject>();
            UnityEngine.Object[] objects = Resources.LoadAll<GroundTile>("Gauntlet/Prefabs/" + prefabDirectory);
            for (int i = 0; i < objects.Length; i++)
            {
                mapObjects.Add(objects[i] as MapObject);
            }

            mapObjectListView.itemsSource = mapObjects;
            mapObjectListView.Refresh();
            Repaint();
        }
    }
    private void OnDestroy()
    {
        if(assetWindow != null)
        {
            assetWindow.Close();
        }
    }

    public void UpdateLevelBinding()
    {
        if (level != null)
        {
            // Create serialization object
            SerializedObject so = new SerializedObject(level);
            // Bind it to the root of the hierarchy. It will find the right object to bind to...
            rootVisualElement.Bind(so);

            // ... or alternatively you can also bind it to the TextField itself.
            // m_ObjectNameBinding.Bind(so);
        }
        else
        {
            // Unbind the object from the actual visual element
            rootVisualElement.Unbind();

            // Clear the TextField after the binding is removed
            // m_ObjectNameBinding.value = "";
        }
    }
}


