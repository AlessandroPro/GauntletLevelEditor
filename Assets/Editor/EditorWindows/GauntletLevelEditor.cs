using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;
using System.IO;
using Newtonsoft.Json;

public enum MapPrefabTypes
{
    GroundTile,
    Item,
    SpawnFactory,
    SpawnPoint,
    Portal
}

public enum MapLayers
{
    Layer1,
    Layer2,
    Layer3
}

public enum PrefabTypes
{
    Enemy,
    Projectile,
    Player,
    SpawnFactory,
    Item,
    GroundTile,
    SpawnPoint,
    Portal
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
    EnumField MapPrefabTypesField;

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
    }

    private void Update()
    {
        if(assetWindow && _window)
        {
           var assetWindowPos = new Vector2(_window.position.x + _window.position.width, _window.position.y);
           assetWindow.position = new Rect(assetWindowPos, assetWindow.position.size);
        }
    }

    public void OnEnable()
    {
        VisualElement root = this.rootVisualElement;
        root.style.flexDirection = FlexDirection.Row;
        root.style.paddingTop = 20;
        root.style.paddingBottom = 20;



        VisualElement gameRoot = new VisualElement()
        {
            style =
            {
                width = width * 0.22f,
                paddingRight = 20,
                paddingLeft = 20
            }
        };
        VisualElement mapRoot = new VisualElement()
        {
            style =
            {
                alignContent = Align.Center,
                width = width * 0.56f
            }
        };
        VisualElement levelRoot = new VisualElement()
        {
            style =
            {
                width = width * 0.22f,
                paddingLeft = 20,
                paddingRight = 20
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


            Button saveDataButton = new Button(() =>
            {
                var path = EditorUtility.SaveFilePanel("Export Game Data", "", "levelData.json", "json");
                
                if(path == null)
                {
                    return;
                }

                if (path.Length != 0)
                {
                    var destinationPath = Path.GetDirectoryName(path);
                    if (level != null)
                    {
                        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                        var json = JsonConvert.SerializeObject(level.saveLevel(), settings);
                        //string json = JsonUtility.ToJson(level.saveLevel());

                        StreamWriter writer = new StreamWriter(path);
                        writer.Write(json);
                        writer.Close();
                    }
                }
            });
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
            Button editButton = new Button(() =>
            {
                if (game != null && levelListView.selectedIndex >= 0)
                {
                    levelData.value = game.levels[levelListView.selectedIndex];
                }
            });
            upButton.text = "Move Up";
            downButton.text = "Move Down";
            removeButton.text = "Remove";
            editButton.text = "Edit";
            levelButtonsRoot.Add(upButton);
            levelButtonsRoot.Add(downButton);
            levelButtonsRoot.Add(removeButton);
            levelButtonsRoot.Add(editButton);

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
                newLevel.initialize("TestLevel", levelSizeSlider.value, 570);
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


        tileMap = new TileMap();
        tileMap.levelEditor = this;
        mapRoot.Add(tileMap.gridContainer);


        // Level Root
        levelRoot.Add(new Label("Choose a prefab to place:"));
        MapPrefabTypesField = new EnumField(MapPrefabTypes.GroundTile);
        MapPrefabTypesField.RegisterCallback<ChangeEvent<Enum>>((evt) =>
        {
            rebindPrefabListView();
        });
        levelRoot.Add(MapPrefabTypesField);
        //levelRoot.Add(createSpriteList());
        levelRoot.Add(createMapTileList());

        var eraseToggle = new Toggle("Erase Mode");
        eraseToggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
        {
            tileMap.eraseMode = evt.newValue;
        });
        levelRoot.Add(eraseToggle);
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


        levelRoot.Add(new Label("Type of asset to create/edit:"));
        EnumField prefabEnums = new EnumField(PrefabTypes.GroundTile);
        levelRoot.Add(prefabEnums);
        Button createEditPrefabButton = new Button(() =>
        {
            switch (prefabEnums.value)
            {
                case PrefabTypes.Player:
                    {
                        assetWindow = PrefabEditor.createWindow<GauntletPlayerEditor>(_window, "Player Editor");
                        break;
                    }
                case PrefabTypes.Enemy:
                    {
                        //GauntletEnemyEditor.createWindow();
                        break;
                    }
                case PrefabTypes.GroundTile:
                    {
                        assetWindow = PrefabEditor.createWindow<GauntletGroundTileEditor>(_window, "Ground Tile Editor");
                        break;
                    }
                case PrefabTypes.SpawnFactory:
                    {
                        assetWindow = PrefabEditor.createWindow<GauntletSpawnFactoryEditor>(_window, "Spawn Factory Editor");
                        break;
                    }
                case PrefabTypes.Item:
                    {
                        //GauntletItemEditor.createWindow();
                        break;
                    }
                default: break;
            }


        });
        createEditPrefabButton.text = "Create/Edit Prefab";
        levelRoot.Add(createEditPrefabButton);

    }


    public ListView createLevelListView()
    {
        var items = new List<GauntletLevel>();

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

        // The "makeItem" function will be called as needed
        // when the ListView needs more items to render
        Func<VisualElement> makeItem = () => new Image()
        {
            style =
                {
                    width = mapObjectListView.contentRect.width,
                    height = 100,
                    justifyContent = Justify.Center,
                    alignSelf = Align.Center,
                    paddingTop = 30f,
                    paddingBottom = 30f,
                    paddingLeft = mapObjectListView.contentRect.width * 0.16f,
                    paddingRight = mapObjectListView.contentRect.width * 0.24f,
                },
            scaleMode = ScaleMode.ScaleToFit

        };

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            var sprite = mapObjects[i].mainSprite;
            if(sprite == null)
            {
                (e as Image).image = Texture2D.whiteTexture;
            }
            else
            {
                var image = e as Image;
                rebindImageTexture(ref image, sprite);
            }
        };

        // Provide the list view with an explict height for every row
        // so it can calculate how many items to actually display
        const int itemHeight = 120;

        mapObjectListView = new ListView(mapObjects, itemHeight, makeItem, bindItem)
        {
            style =
            {
                backgroundColor = Color.gray
            }
        };

        rebindPrefabListView();

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
        mapObjects = new List<MapObject>();
        UnityEngine.Object[] objects = null;

        if (mapObjectListView != null)
        {
            switch (MapPrefabTypesField.value)
            {
                case MapPrefabTypes.GroundTile:
                    {
                        objects = Resources.LoadAll<GroundTile>("Gauntlet/Prefabs/GroundTiles");
                        break;
                    }
                case MapPrefabTypes.Item:
                    {
                        objects = Resources.LoadAll<Player>("Gauntlet/Prefabs/Items"); ///CHANGE THIS TO <ITEM>
                        break;
                    }
                case MapPrefabTypes.SpawnFactory:
                    {
                        objects = Resources.LoadAll<SpawnFactory>("Gauntlet/Prefabs/SpawnFactories");
                        break;
                    }
                default: break;
            }

            for (int i = 0; i < objects.Length; i++)
            {
                var mapObject = objects[i] as MapObject;
                if (mapObject.mainSprite)
                {
                    mapObjects.Add(mapObject);
                }
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
        }
        else
        {
            // Unbind the object from the actual visual element
            rootVisualElement.Unbind();

        }
    }

    public static void rebindImageTexture(ref Image image, Sprite sprite)
    {
        image.image = sprite.texture;
        var spriteRect = sprite.rect;
        // Small hack to get the image rect's y-axis to line up with the sprite's rect
        // if the sprite is from a sliced spritesheet
        if (sprite.rect.width < sprite.texture.width)
        {
            float newY = sprite.texture.height - spriteRect.y - spriteRect.height;
            Rect newRect = new Rect(spriteRect.x, newY, spriteRect.width, spriteRect.height);
            image.sourceRect = newRect;
        }
        else
        {
            image.sourceRect = sprite.rect;
        }

        image.MarkDirtyRepaint();
    }
}


