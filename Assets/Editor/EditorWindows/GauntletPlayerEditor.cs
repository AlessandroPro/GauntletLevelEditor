using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;


public class GauntletPlayerEditor : PrefabEditor
{
    public Player player;

    public void OnEnable()
    {
        setupWindow();

        // Data
        dataRoot.Add(new Label("Choose a Player:"));
        objectData = new ObjectField();
        objectData.objectType = typeof(Player);
        dataRoot.Add(objectData);

        objectData.RegisterCallback<ChangeEvent<Object>>((evt) =>
        {
            var change = (evt.target as ObjectField).value;
            player = change as Player;
            UpdateBinding();
        });

        dataRoot.Add(new Spacer(30));
        dataRoot.Add(new Label("Name:"));
        nameTextField = new TextField();
        nameTextField.bindingPath = "objectName";
        dataRoot.Add(nameTextField);

        addSlider(ref dataRoot, 20, 100, "Health:   ", "health");
        addSlider(ref dataRoot, 5, 100, "Walk Speed:   ", "walkSpeed");

        dataRoot.Add(new Spacer(30));
        dataRoot.Add(new Label("Weapon:"));
        var weaponData = new ObjectField();
        weaponData.objectType = typeof(Projectile);
        weaponData.bindingPath = "weapon";
        dataRoot.Add(weaponData);

        weaponData.RegisterCallback<ChangeEvent<Object>>((evt) =>
        {
            var change = (evt.target as ObjectField).value;
            player.weapon = change as Projectile;
            UpdateBinding();
        });

        //dataRoot.Add(new IntegerField("MaxLives:"));
        //        dataRoot.Add(new Label("Weapon:"));
        //        dataRoot.Add(new Slider("Damage:", 0, 100));
        //        dataRoot.Add(new Slider("Throw Speed:", 0, 100));
        //        dataRoot.Add(new SliderInt("Pool Size", 0, 100));
        //        dataRoot.Add(new Slider("TimeInterval (s):", 0, 100));

        // sprites
        Button newData = new Button(() =>
        {
            Player newPlayer = CreateInstance<Player>();
            var path = "Assets/Resources/Gauntlet/Prefabs/Players";
            AssetDatabase.CreateAsset(newPlayer, AssetDatabase.GenerateUniqueAssetPath(path + "/Player-00.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            objectData.value = newPlayer;
            UpdateBinding();
        });
        newData.text = "New";
        spriteRoot.Add(newData);
        spriteRoot.Add(new Spacer(30));
        spriteRoot.Add(new Label("Player Sprite:"));
        objectTileSprite = new ObjectField();
        objectTileSprite.objectType = typeof(Sprite);
        objectTileSprite.bindingPath = "mainSprite";
        spriteRoot.Add(objectTileSprite);
        objectTileSpriteImage = new Image()
        {
            style =
                {
                    width = 100,
                    height = 100,
                    borderLeftWidth = 2,
                    borderRightWidth = 2,
                    borderTopWidth = 2,
                    borderBottomWidth = 2,
                    marginTop = 10,
                    marginBottom = 20,
                    marginLeft = 30,
                    borderColor = Color.gray

                },
            scaleMode = ScaleMode.ScaleToFit
        };

        objectTileSprite.RegisterCallback<ChangeEvent<Object>>((evt) =>
        {

            var change = (evt.target as ObjectField).value;
            if (change)
            {
                var sprite = change as Sprite;
                GauntletLevelEditor.rebindImageTexture(ref objectTileSpriteImage, sprite);
            }
            else
            {
                objectTileSpriteImage.image = null;
            }

            if (objectData.value)
            {
                (objectData.value as Player).mainSprite = change as Sprite;
            }
            Repaint();
            parentWindow.rebindPrefabListView();
        });

        spriteRoot.Add(objectTileSpriteImage);
    }

    public void UpdateBinding()
    {
        if (player != null)
        {
            // Create serialization object
            SerializedObject so = new SerializedObject(player);
            // Bind it to the root of the hierarchy. 
            rootVisualElement.Bind(so);

        }
        else
        {
            // Unbind the object from the actual visual element
            rootVisualElement.Unbind();
            //objectTileSpriteImage.image = null;
            // objectTileSprite.value = null;
            //nameTextField.value = "";
            //m_ObjectNameBinding.Unbind();

            // Clear the TextField after the binding is removed
            // m_ObjectNameBinding.value = "";
        }
    }
}

//public class GauntletPlayerEditor : PrefabEditor
//{
//    public Player player;

//    public void OnEnable()
//    {
//        setupWindow();

//        // Data
//        dataRoot.Add(new Label("Choose a Player:"));
//        objectData = new ObjectField();
//        objectData.objectType = typeof(Player);
//        dataRoot.Add(objectData);

//        objectData.RegisterCallback<ChangeEvent<Object>>((evt) =>
//        {
//            var change = (evt.target as ObjectField).value;
//            player = change as Player;
//            UpdateBinding();
//        });

//        dataRoot.Add(new Spacer(30));
//        dataRoot.Add(new Label("Name:"));
//        nameTextField = new TextField();
//        nameTextField.bindingPath = "objectName";
//        dataRoot.Add(nameTextField);

//        // sprites
//        Button newData = new Button(() =>
//        {
//            Player newPlayer = CreateInstance<Player>();
//            var path = "Assets/Resources/Gauntlet/Prefabs/Players";
//            AssetDatabase.CreateAsset(newPlayer, AssetDatabase.GenerateUniqueAssetPath(path + "/Player-00.asset"));
//            AssetDatabase.SaveAssets();
//            AssetDatabase.Refresh();
//            objectData.value = newPlayer;
//            UpdateBinding();
//        });
//        newData.text = "New";
//        spriteRoot.Add(newData);
//        spriteRoot.Add(new Spacer(30));
//        spriteRoot.Add(new Label("Player Sprite:"));
//        objectTileSprite = new ObjectField();
//        objectTileSprite.objectType = typeof(Sprite);
//        objectTileSprite.bindingPath = "mainSprite";
//        spriteRoot.Add(objectTileSprite);
//        objectTileSpriteImage = new Image()
//        {
//            style =
//                {
//                    width = 100,
//                    height = 100,
//                    borderLeftWidth = 2,
//                    borderRightWidth = 2,
//                    borderTopWidth = 2,
//                    borderBottomWidth = 2,
//                    marginTop = 10,
//                    marginBottom = 20,
//                    marginLeft = 30,
//                    borderColor = Color.gray

//                },
//            scaleMode = ScaleMode.ScaleToFit
//        };

//        objectTileSprite.RegisterCallback<ChangeEvent<Object>>((evt) =>
//        {

//            var change = (evt.target as ObjectField).value;
//            if (change)
//            {
//                var sprite = change as Sprite;
//                GauntletLevelEditor.rebindImageTexture(ref objectTileSpriteImage, sprite);
//            }
//            else
//            {
//                objectTileSpriteImage.image = null;
//            }

//            if (objectData.value)
//            {
//                (objectData.value as Player).mainSprite = change as Sprite;
//            }
//            Repaint();
//            parentWindow.rebindPrefabListView();
//        });

//        spriteRoot.Add(objectTileSpriteImage);
//    }

//    public void UpdateBinding()
//    {
//        if (player != null)
//        {
//            // Create serialization object
//            SerializedObject so = new SerializedObject(player);
//            // Bind it to the root of the hierarchy. 
//            rootVisualElement.Bind(so);

//        }
//        else
//        {
//            // Unbind the object from the actual visual element
//            rootVisualElement.Unbind();
//            //objectTileSpriteImage.image = null;
//            // objectTileSprite.value = null;
//            //nameTextField.value = "";
//            //m_ObjectNameBinding.Unbind();

//            // Clear the TextField after the binding is removed
//            // m_ObjectNameBinding.value = "";
//        }
//    }
//}


//public class GauntletPlayerEditor : EditorWindow
//{
//    static GauntletPlayerEditor _window = null;
//    static float width = 500;
//    static float height = 350;

//   // [MenuItem("Tools/GauntLet Player Editor")]
//   // [Shortcut("Refresh Editor", KeyCode.F10)]
//    public static void createWindow()
//    {
//        if (_window != null) _window.Close();
//        _window = GetWindow<GauntletPlayerEditor>();
//        _window.titleContent = new GUIContent("Gauntlet Player Editor");
//        _window.maxSize = new Vector2(width, height);
//        _window.minSize = new Vector2(width, height);
//    }

//    public void OnEnable()
//    {
//        VisualElement root = this.rootVisualElement;
//        root.style.flexDirection = FlexDirection.Row;
//        root.style.paddingTop = 10;
//        root.style.paddingBottom = 10;
//        root.style.paddingLeft = 10;
//        root.style.paddingRight = 10;


//        VisualElement dataRoot = new VisualElement()
//        {
//            style =
//            {
//                width = _window.maxSize.x * 0.7f,
//                paddingRight = 30
//            }
//        };
//        VisualElement spriteRoot = new VisualElement()
//        {
//            style =
//            {
//                alignContent = Align.Center,
//                width = _window.maxSize.x * 0.3f
//            }
//        };

//        root.Add(dataRoot);
//        root.Add(spriteRoot);

//        // Data
//        dataRoot.Add(new Label("Choose a Player:"));
//        ObjectField playerData = new ObjectField();
//        //playerData.objectType = typeof(Player);
//        dataRoot.Add(playerData);

//        dataRoot.Add(new TextField("Name:"));
//        dataRoot.Add(new Slider("Health:", 5, 100));
//        dataRoot.Add(new Slider("Walk Speed:", 1, 100));
//        dataRoot.Add(new IntegerField("MaxLives:"));
//        dataRoot.Add(new Label("Weapon:"));
//        dataRoot.Add(new Slider("Damage:", 0, 100));
//        dataRoot.Add(new Slider("Throw Speed:", 0, 100));
//        dataRoot.Add(new SliderInt("Pool Size", 0, 100));
//        dataRoot.Add(new Slider("TimeInterval (s):", 0, 100));


//        // sprites
//        Button newData = new Button();
//        newData.text = "New";
//        spriteRoot.Add(newData);
//        spriteRoot.Add(new Label("Player Sprite:"));
//        ObjectField playerSprite = new ObjectField();
//        playerSprite.objectType = typeof(Sprite);
//        spriteRoot.Add(playerSprite);
//        Image playerSpriteImage = new Image()
//        {
//            style =
//                {
//                    width = 80,
//                    height = 80,
//                    paddingTop = 10f,
//                    paddingBottom = 10f,
//                    paddingLeft = 10f,
//                    paddingRight = 10f,
//                    borderLeftWidth = 2,
//                    borderRightWidth = 2,
//                    borderTopWidth = 2,
//                    borderBottomWidth = 2,
//                    marginTop = 10,
//                    marginBottom = 20,
//                    marginLeft = 25,
//                    marginRight = 25,
//                    borderColor = Color.gray

//                },
//            scaleMode = ScaleMode.ScaleToFit
//        };
//        spriteRoot.Add(playerSpriteImage);

//        spriteRoot.Add(new Label("Weapon Sprite:"));
//        ObjectField weaponSprite = new ObjectField();
//        weaponSprite.objectType = typeof(Sprite);
//        spriteRoot.Add(weaponSprite);
//        Image weaponSpriteImage = new Image()
//        {
//            style =
//                {
//                    width = 80,
//                    height = 80,
//                    paddingTop = 10f,
//                    paddingBottom = 10f,
//                    paddingLeft = 10f,
//                    paddingRight = 10f,
//                    borderLeftWidth = 2,
//                    borderRightWidth = 2,
//                    borderTopWidth = 2,
//                    borderBottomWidth = 2,
//                    marginTop = 10,
//                    marginBottom = 20,
//                    marginLeft = 25,
//                    marginRight = 25,
//                    borderColor = Color.gray

//                },
//            scaleMode = ScaleMode.ScaleToFit
//        };
//        spriteRoot.Add(weaponSpriteImage);
//    }
//}
