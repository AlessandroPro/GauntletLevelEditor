using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;
public class GauntletPlayerEditor : EditorWindow
{
    static GauntletPlayerEditor _window = null;
    static float width = 500;
    static float height = 350;

   // [MenuItem("Tools/GauntLet Player Editor")]
   // [Shortcut("Refresh Editor", KeyCode.F10)]
    public static void createWindow()
    {
        if (_window != null) _window.Close();
        _window = GetWindow<GauntletPlayerEditor>();
        _window.titleContent = new GUIContent("Gauntlet Player Editor");
        _window.maxSize = new Vector2(width, height);
        _window.minSize = new Vector2(width, height);
    }

    public void OnEnable()
    {
        VisualElement root = this.rootVisualElement;
        root.style.flexDirection = FlexDirection.Row;
        root.style.paddingTop = 10;
        root.style.paddingBottom = 10;
        root.style.paddingLeft = 10;
        root.style.paddingRight = 10;


        VisualElement dataRoot = new VisualElement()
        {
            style =
            {
                width = _window.maxSize.x * 0.7f,
                paddingRight = 30
            }
        };
        VisualElement spriteRoot = new VisualElement()
        {
            style =
            {
                alignContent = Align.Center,
                width = _window.maxSize.x * 0.3f
            }
        };

        root.Add(dataRoot);
        root.Add(spriteRoot);

        // Data
        dataRoot.Add(new Label("Choose a Player:"));
        ObjectField playerData = new ObjectField();
        //playerData.objectType = typeof(Player);
        dataRoot.Add(playerData);

        dataRoot.Add(new TextField("Name:"));
        dataRoot.Add(new Slider("Health:", 5, 100));
        dataRoot.Add(new Slider("Walk Speed:", 1, 100));
        dataRoot.Add(new IntegerField("MaxLives:"));
        dataRoot.Add(new Label("Weapon:"));
        dataRoot.Add(new Slider("Damage:", 0, 100));
        dataRoot.Add(new Slider("Throw Speed:", 0, 100));
        dataRoot.Add(new SliderInt("Pool Size", 0, 100));
        dataRoot.Add(new Slider("TimeInterval (s):", 0, 100));


        // sprites
        Button newData = new Button();
        newData.text = "New";
        spriteRoot.Add(newData);
        spriteRoot.Add(new Label("Player Sprite:"));
        ObjectField playerSprite = new ObjectField();
        playerSprite.objectType = typeof(Sprite);
        spriteRoot.Add(playerSprite);
        Image playerSpriteImage = new Image()
        {
            style =
                {
                    width = 80,
                    height = 80,
                    paddingTop = 10f,
                    paddingBottom = 10f,
                    paddingLeft = 10f,
                    paddingRight = 10f,
                    borderLeftWidth = 2,
                    borderRightWidth = 2,
                    borderTopWidth = 2,
                    borderBottomWidth = 2,
                    marginTop = 10,
                    marginBottom = 20,
                    marginLeft = 25,
                    marginRight = 25,
                    borderColor = Color.gray
                
                },
            scaleMode = ScaleMode.ScaleToFit
        };
        spriteRoot.Add(playerSpriteImage);

        spriteRoot.Add(new Label("Weapon Sprite:"));
        ObjectField weaponSprite = new ObjectField();
        weaponSprite.objectType = typeof(Sprite);
        spriteRoot.Add(weaponSprite);
        Image weaponSpriteImage = new Image()
        {
            style =
                {
                    width = 80,
                    height = 80,
                    paddingTop = 10f,
                    paddingBottom = 10f,
                    paddingLeft = 10f,
                    paddingRight = 10f,
                    borderLeftWidth = 2,
                    borderRightWidth = 2,
                    borderTopWidth = 2,
                    borderBottomWidth = 2,
                    marginTop = 10,
                    marginBottom = 20,
                    marginLeft = 25,
                    marginRight = 25,
                    borderColor = Color.gray

                },
            scaleMode = ScaleMode.ScaleToFit
        };
        spriteRoot.Add(weaponSpriteImage);
    }
}
