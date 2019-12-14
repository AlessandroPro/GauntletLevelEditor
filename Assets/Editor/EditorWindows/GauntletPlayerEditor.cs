using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
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
        addSlider(ref dataRoot, 50, 500, "Walk Speed:   ", "walkSpeed");
        addSlider(ref dataRoot, 1, 20, "MaxLives:   ", "maxLives");

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

        // sprites
        Button newData = new Button(() =>
        {
            Player newPlayer = CreateInstance<Player>();
            newPlayer.objectName = "Player";
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
        }
    }
}

