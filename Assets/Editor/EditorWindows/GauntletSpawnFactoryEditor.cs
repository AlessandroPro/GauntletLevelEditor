using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;
public class GauntletSpawnFactoryEditor : PrefabEditor
{
    public SpawnFactory spawnFactory;

    public void OnEnable()
    {
        setupWindow();

        // Data
        dataRoot.Add(new Label("Choose a SpawnFactory:"));
        objectData = new ObjectField();
        objectData.objectType = typeof(SpawnFactory);
        dataRoot.Add(objectData);

        objectData.RegisterCallback<ChangeEvent<Object>>((evt) =>
        {
            var change = (evt.target as ObjectField).value;
            spawnFactory = change as SpawnFactory;
            UpdateBinding();
        });

        dataRoot.Add(new Spacer(30));
        dataRoot.Add(new Label("Name:"));
        nameTextField = new TextField();
        nameTextField.bindingPath = "objectName";
        dataRoot.Add(nameTextField);

        addSlider(ref dataRoot, 20, 100, "Health:   ", "health");
        addSlider(ref dataRoot, 1, 100, "TimeInterval (s):   ", "timeInterval");


        dataRoot.Add(new Spacer(30));
        dataRoot.Add(new Label("Enemy:"));
        var enemyData = new ObjectField();
        enemyData.objectType = typeof(Enemy);
        enemyData.bindingPath = "enemy";
        dataRoot.Add(enemyData);

        dataRoot.Add(new Spacer(30));
        dataRoot.Add(new Label("Drop Item:"));
        var itemData = new ObjectField();
        itemData.objectType = typeof(Item);
        itemData.bindingPath = "dropItem";
        dataRoot.Add(itemData);

        // sprites
        Button newData = new Button(() =>
        {
            SpawnFactory newSpawnFactory = CreateInstance<SpawnFactory>();
            newSpawnFactory.objectName = "SpawnFactory";
            var path = "Assets/Resources/Gauntlet/Prefabs/SpawnFactories";
            AssetDatabase.CreateAsset(newSpawnFactory, AssetDatabase.GenerateUniqueAssetPath(path + "/SpawnFactory-00.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            objectData.value = newSpawnFactory;
            UpdateBinding();
        });
        newData.text = "New";
        spriteRoot.Add(newData);
        spriteRoot.Add(new Spacer(30));
        spriteRoot.Add(new Label("SpawnFactory Sprite:"));
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
                (objectData.value as SpawnFactory).mainSprite = change as Sprite;
            }
            Repaint();
            parentWindow.rebindPrefabListView();
        });

        spriteRoot.Add(objectTileSpriteImage);
    }

    public void UpdateBinding()
    {
        if (spawnFactory != null)
        {
            // Create serialization object
            SerializedObject so = new SerializedObject(spawnFactory);
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
