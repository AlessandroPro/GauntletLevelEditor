using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;


public enum ItemTypes
{
    HealthBoost,
    AttackBoost,
    Key,
    Barrier
}
public class GauntletItemEditor : EditorWindow
{
    static GauntletItemEditor _window = null;
    static float width = 500;
    static float height = 350;

    //[MenuItem("Tools/GauntLet SpawnFactory Editor")]
    public static void createWindow()
    {
        if (_window != null) _window.Close();
        _window = GetWindow<GauntletItemEditor>();
        _window.titleContent = new GUIContent("Gauntlet Item Editor");
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
        dataRoot.Add(new Label("Choose an Item:"));
        ObjectField itemData = new ObjectField();
        dataRoot.Add(itemData);

        dataRoot.Add(new TextField("Name:"));
        dataRoot.Add(new EnumField(ItemTypes.HealthBoost));
        dataRoot.Add(new Toggle("Can be picked up"));

        // sprites
        Button newData = new Button();
        newData.text = "New";
        spriteRoot.Add(newData);
        spriteRoot.Add(new Label("Item Sprite:"));
        ObjectField itemSprite = new ObjectField();
        itemSprite.objectType = typeof(Sprite);
        spriteRoot.Add(itemSprite);
        Image itemSpriteImage = new Image()
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
        spriteRoot.Add(itemSpriteImage);
    }
}

