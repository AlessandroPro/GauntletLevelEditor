using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;

public class GauntletSpawnFactoryEditor : EditorWindow
{
    static GauntletSpawnFactoryEditor _window = null;
    static float width = 500;
    static float height = 350;

    //[MenuItem("Tools/GauntLet SpawnFactory Editor")]
    public static void createWindow()
    {
        if (_window != null) _window.Close();
        _window = GetWindow<GauntletSpawnFactoryEditor>();
        _window.titleContent = new GUIContent("Gauntlet Spawn Factory Editor");
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
        dataRoot.Add(new Label("Choose a Spawn Factory:"));
        ObjectField sfData = new ObjectField();
        dataRoot.Add(sfData);

        dataRoot.Add(new TextField("Name:"));
        dataRoot.Add(new Slider("Health:", 5, 100));
        dataRoot.Add(new Slider("Spawn Rate:", 1, 100));
        dataRoot.Add(new SliderInt("Pool Size", 0, 100));
        dataRoot.Add(new Slider("TimeInterval (s):", 0, 100));

        dataRoot.Add(new Label("Spawn Enemy:"));
        ObjectField spawnEnemy = new ObjectField();
        dataRoot.Add(spawnEnemy);

        // sprites
        Button newData = new Button();
        newData.text = "New";
        spriteRoot.Add(newData);
        spriteRoot.Add(new Label("Factory Sprite:"));
        ObjectField sfSprite = new ObjectField();
        sfSprite.objectType = typeof(Sprite);
        spriteRoot.Add(sfSprite);
        Image sfSpriteImage = new Image()
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
        spriteRoot.Add(sfSpriteImage);
    }
}
