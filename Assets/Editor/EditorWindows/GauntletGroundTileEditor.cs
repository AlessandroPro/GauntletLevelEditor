using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;

public class Spacer : Label
{
    public Spacer(int space)
    {
        style.marginTop = space;
    }
}
public class GauntletGroundTileEditor : EditorWindow
{
    static GauntletGroundTileEditor _window = null;
    static GauntletLevelEditor parentWindow = null;
    static float width = 450;


    static GroundTile groundTile;

    // Properties
    TextField nameTextField;
    ObjectField groundTileData;
    ObjectField groundTileSprite;
    Image groundTileSpriteImage;

    public static EditorWindow createWindow(GauntletLevelEditor _parentWindow)
    {
        if (_window != null) _window.Close();
        _window = GetWindow<GauntletGroundTileEditor>();
        _window.titleContent = new GUIContent("Ground Tile Editor");
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
                width = width * 0.6f,
                paddingRight = 30
            }
        };
        VisualElement spriteRoot = new VisualElement()
        {
            style =
            {
                alignContent = Align.Center,
                width = width * 0.4f
            }
        };

        root.Add(dataRoot);
        root.Add(spriteRoot);

        // Data
        dataRoot.Add(new Label("Choose a Ground Tile:"));
        groundTileData = new ObjectField();
        groundTileData.objectType = typeof(GroundTile);
        dataRoot.Add(groundTileData);

        groundTileData.RegisterCallback<ChangeEvent<Object>>((evt) =>
        {
            var change = (evt.target as ObjectField).value;
            groundTile = change as GroundTile;
            UpdateBinding();
        });

        dataRoot.Add(new Spacer(30));
        dataRoot.Add(new Label("Name:"));
        nameTextField = new TextField();
        nameTextField.bindingPath = "objectName";
        dataRoot.Add(nameTextField);

        // sprites
        Button newData = new Button(() =>
        {
            GroundTile tile = CreateInstance<GroundTile>();
            var path = "Assets/Resources/Gauntlet/Prefabs/GroundTiles";
            AssetDatabase.CreateAsset(tile, AssetDatabase.GenerateUniqueAssetPath(path + "/GroundTile-00.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            groundTileData.value = tile;
            UpdateBinding();
        });
       newData.text = "New";
        spriteRoot.Add(newData);
        spriteRoot.Add(new Spacer(30));
        spriteRoot.Add(new Label("Ground Tile Sprite:"));
        groundTileSprite = new ObjectField();
        groundTileSprite.objectType = typeof(Sprite);
        groundTileSprite.bindingPath = "mainSprite";
        spriteRoot.Add(groundTileSprite);
        groundTileSpriteImage = new Image()
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

        groundTileSprite.RegisterCallback<ChangeEvent<Object>>((evt) =>
        {
            
            var change = (evt.target as ObjectField).value;
           if(change)
            {
                var sprite = change as Sprite;
                GauntletLevelEditor.rebindImageTexture(ref groundTileSpriteImage, sprite);
            }
            else
            {
                groundTileSpriteImage.image = null;
            }

            if (groundTileData.value)
            {
                (groundTileData.value as GroundTile).mainSprite = change as Sprite;
            }
            Repaint();
            parentWindow.rebindPrefabListView();
        });

        spriteRoot.Add(groundTileSpriteImage);
    }

    public void UpdateBinding()
    {
        if (groundTile != null)
        {
            // Create serialization object
            SerializedObject so = new SerializedObject(groundTile);
            // Bind it to the root of the hierarchy. 
            rootVisualElement.Bind(so);

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


