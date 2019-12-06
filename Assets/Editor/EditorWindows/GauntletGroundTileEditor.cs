using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;

public class GauntletGroundTileEditor : EditorWindow
{
    static GauntletGroundTileEditor _window = null;
    static GauntletLevelEditor parentWindow = null;
    static float width = 400;


    static GroundTile groundTile;

    // Properties
    TextField nameTextField;
    ObjectField groundTileSprite;
    Image groundTileSpriteImage;




    //[MenugroundTile("Tools/GauntLet Ground Tile Editor")]
    public static EditorWindow createWindow(GauntletLevelEditor _parentWindow)
    {
        if (_window != null) _window.Close();
        _window = GetWindow<GauntletGroundTileEditor>();
        _window.titleContent = new GUIContent("Gauntlet Ground Tile Editor");
        _window.maxSize = new Vector2(width, _parentWindow.position.height);
        _window.minSize = new Vector2(width, _parentWindow.position.height);
        _window.Repaint();
        parentWindow = _parentWindow;
        return _window;
    }


    private void OnDestroy()
    {
        if(parentWindow)
        {
            parentWindow.assetWindow = null;
        }
        parentWindow = null;
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
        dataRoot.Add(new Label("Choose a Ground Tile:"));
        ObjectField groundTileData = new ObjectField();
        groundTileData.objectType = typeof(GroundTile);
        dataRoot.Add(groundTileData);

        groundTileData.RegisterCallback<ChangeEvent<Object>>((evt) =>
        {
            var change = (evt.target as ObjectField).value;
            groundTile = change as GroundTile;
            UpdateBinding();
        });

        nameTextField = new TextField("Name:");
        nameTextField.bindingPath = "objectName";
        dataRoot.Add(nameTextField);

        // sprites
        Button newData = new Button();
        newData.text = "New";
        spriteRoot.Add(newData);
        spriteRoot.Add(new Label("Ground Tile Sprite:"));
        groundTileSprite = new ObjectField();
        groundTileSprite.objectType = typeof(Sprite);
        groundTileSprite.bindingPath = "mainSprite";
        spriteRoot.Add(groundTileSprite);
        groundTileSpriteImage = new Image()
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

        groundTileSprite.RegisterCallback<ChangeEvent<Object>>((evt) =>
        {
           var change = (evt.target as ObjectField).value;
           groundTileSpriteImage.image = (change as Sprite).texture;
        });

        //groundTileSpriteImage.image = (groundTileSprite.value as Sprite).texture;
        spriteRoot.Add(groundTileSpriteImage);
    }

    public void UpdateBinding()
    {
        if (groundTile != null)
        {
            // Create serialization object
            SerializedObject so = new SerializedObject(groundTile);
            // Bind it to the root of the hierarchy. It will find the right object to bind to...
            rootVisualElement.Bind(so);

            // ... or alternatively you can also bind it to the TextField itself.
            // m_ObjectNameBinding.Bind(so);
        }
        else
        {
            // Unbind the object from the actual visual element
            rootVisualElement.Unbind();
            //groundTileSpriteImage.image = null;
           // groundTileSprite.value = null;
             //nameTextField.value = "";
            //m_ObjectNameBinding.Unbind();

            // Clear the TextField after the binding is removed
            // m_ObjectNameBinding.value = "";
        }
    }
}


