using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;

public enum AttackStyles
{
    RangeBased,
    Collision,
    Melee
}

public class GauntletEnemyEditor : EditorWindow
{
    static GauntletEnemyEditor _window = null;
    static float width = 500;
    static float height = 350;

    //[MenuItem("Tools/GauntLet Enemy Editor")]
    public static void createWindow()
    {
        if (_window != null) _window.Close();
        _window = GetWindow<GauntletEnemyEditor>();
        _window.titleContent = new GUIContent("Gauntlet Enemy Editor");
        _window.maxSize = new Vector2(width, height);
        _window.minSize = new Vector2(width, height);
        _window.Repaint();
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
        dataRoot.Add(new Label("Choose an Enemy:"));
        ObjectField enemyData = new ObjectField();
        dataRoot.Add(enemyData);

        dataRoot.Add(new TextField("Name:"));
        dataRoot.Add(new Slider("Health:", 5, 100));
        dataRoot.Add(new Slider("Walk Speed:", 1, 100));
        dataRoot.Add(new EnumField(AttackStyles.Melee));
        dataRoot.Add(new Label("Weapon:"));
        dataRoot.Add(new Slider("Damage:", 0, 100));
        dataRoot.Add(new Slider("Throw Speed:", 0, 100));
        dataRoot.Add(new SliderInt("Pool Size", 0, 100));
        dataRoot.Add(new Slider("TimeInterval (s):", 0, 100));


        // sprites
        Button newData = new Button();
        newData.text = "New";
        spriteRoot.Add(newData);
        spriteRoot.Add(new Label("Enemy Sprite:"));
        ObjectField enemySprite = new ObjectField();
        enemySprite.objectType = typeof(Sprite);
        spriteRoot.Add(enemySprite);
        Image enemySpriteImage = new Image()
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
        spriteRoot.Add(enemySpriteImage);

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
