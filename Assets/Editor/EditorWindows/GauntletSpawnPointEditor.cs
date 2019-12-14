using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class GauntletSpawnPointEditor : PrefabEditor
{
    public SpawnPoint spawnPoint;
    public void OnEnable()
    {
        setupWindow();

        // Data
        dataRoot.Add(new Label("Choose a SpawnPoint:"));
        objectData = new ObjectField();
        objectData.objectType = typeof(SpawnPoint);
        dataRoot.Add(objectData);

        objectData.RegisterCallback<ChangeEvent<Object>>((evt) =>
        {
            var change = (evt.target as ObjectField).value;
            spawnPoint = change as SpawnPoint;
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
            SpawnPoint newSpawnPoint = CreateInstance<SpawnPoint>();
            newSpawnPoint.objectName = "SpawnPoint";
            var path = "Assets/Resources/Gauntlet/Prefabs/SpawnPoints";
            AssetDatabase.CreateAsset(newSpawnPoint, AssetDatabase.GenerateUniqueAssetPath(path + "/SpawnPoint-00.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            objectData.value = newSpawnPoint;
            UpdateBinding();
        });
        newData.text = "New";
        spriteRoot.Add(newData);
        spriteRoot.Add(new Spacer(30));
        spriteRoot.Add(new Label("SpawnPoint Sprite:"));
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
                    marginLeft = 10,
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
                (objectData.value as SpawnPoint).mainSprite = change as Sprite;
            }
            Repaint();
            parentWindow.rebindPrefabListView();
        });

        spriteRoot.Add(objectTileSpriteImage);
    }

    public void UpdateBinding()
    {
        if (spawnPoint != null)
        {
            // Create serialization object
            SerializedObject so = new SerializedObject(spawnPoint);
            // Bind it to the root of the hierarchy. 
            rootVisualElement.Bind(so);

        }
        else
        {
            // Unbind the object from the actual visual element
            rootVisualElement.Unbind();
        }
    }
}
