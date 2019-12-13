using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;

public class GauntletSpawnFactoryEditor : PrefabEditor
{

    //[MenuItem("Tools/GauntLet SpawnFactory Editor")]


    public void OnEnable()
    {
        setupWindow();

        // Data
        dataRoot.Add(new Label("Choose a Spawn Factory:"));
        objectData = new ObjectField();
        dataRoot.Add(objectData);

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
        ObjectField objectTileSprite = new ObjectField();
        objectTileSprite.objectType = typeof(Sprite);
        spriteRoot.Add(objectTileSprite);
        Image objectTileSpriteImage = new Image()
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
        spriteRoot.Add(objectTileSpriteImage);
    }
}
