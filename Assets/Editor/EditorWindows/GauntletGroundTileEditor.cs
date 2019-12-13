using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;


public class GauntletGroundTileEditor : PrefabEditor
{
    static GroundTile groundTile;

    public void OnEnable()
    {
        setupWindow();

        // Data
        dataRoot.Add(new Label("Choose a Ground Tile:"));
        objectData = new ObjectField();
        objectData.objectType = typeof(GroundTile);
        dataRoot.Add(objectData);

        objectData.RegisterCallback<ChangeEvent<Object>>((evt) =>
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

        dataRoot.Add(new Spacer(30));
        Toggle colToggle = new Toggle("Is Collider:");
        Toggle triggerToggle = new Toggle("Is Trigger:");
        colToggle.bindingPath = "isCollider";
        triggerToggle.bindingPath = "isTrigger";
        triggerToggle.visible = false;

        colToggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
        {
            if(evt.newValue == false)
            {
                triggerToggle.value = false;
                triggerToggle.visible = false;
            }
            else
            {
                triggerToggle.visible = true;
            }
        });

        dataRoot.Add(colToggle);
        dataRoot.Add(triggerToggle);



        // sprites
        Button newData = new Button(() =>
        {
            GroundTile tile = CreateInstance<GroundTile>();
            var path = "Assets/Resources/Gauntlet/Prefabs/GroundTiles";
            AssetDatabase.CreateAsset(tile, AssetDatabase.GenerateUniqueAssetPath(path + "/GroundTile-00.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            objectData.value = tile;
            UpdateBinding();
        });
       newData.text = "New";
        spriteRoot.Add(newData);
        spriteRoot.Add(new Spacer(30));
        spriteRoot.Add(new Label("Ground Tile Sprite:"));
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
           if(change)
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
                (objectData.value as GroundTile).mainSprite = change as Sprite;
            }
            Repaint();
            parentWindow.rebindPrefabListView();
        });

        spriteRoot.Add(objectTileSpriteImage);
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
            //objectTileSpriteImage.image = null;
           // objectTileSprite.value = null;
             //nameTextField.value = "";
            //m_ObjectNameBinding.Unbind();

            // Clear the TextField after the binding is removed
            // m_ObjectNameBinding.value = "";
        }
    }
}


