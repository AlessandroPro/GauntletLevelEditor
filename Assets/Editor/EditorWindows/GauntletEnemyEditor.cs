using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;
using System;

public enum AttackStyles
{
    RangeBased,
    Collision,
    Melee
}

public class GauntletEnemyEditor : PrefabEditor
{
    public Enemy enemy;

    public void OnEnable()
    {
        setupWindow();

        // Data
        dataRoot.Add(new Label("Choose a Enemy:"));
        objectData = new ObjectField();
        objectData.objectType = typeof(Enemy);
        dataRoot.Add(objectData);

        objectData.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
        {
            var change = (evt.target as ObjectField).value;
            enemy = change as Enemy;
            UpdateBinding();
        });

        dataRoot.Add(new Spacer(30));
        dataRoot.Add(new Label("Name:"));
        nameTextField = new TextField();
        nameTextField.bindingPath = "objectName";
        dataRoot.Add(nameTextField);

        addSlider(ref dataRoot, 20, 100, "Health:   ", "health");
        addSlider(ref dataRoot, 30, 500, "Walk Speed:   ", "walkSpeed");
        addSlider(ref dataRoot, 1, 100, "Time Between Each Attack (s):   ", "attackTimeInterval");

        dataRoot.Add(new Spacer(30));
        dataRoot.Add(new Label("Attack Style:"));
        var attackStyleEnumField = new EnumField(AttackStyles.RangeBased);
        dataRoot.Add(attackStyleEnumField);

        attackStyleEnumField.RegisterCallback<ChangeEvent<Enum>>((evt) =>
        {
            var change = evt.newValue;
            if(enemy)
            {
                enemy.attackStyle = Convert.ToInt32(change);
            }
        });

        dataRoot.Add(new Spacer(30));
        dataRoot.Add(new Label("Weapon:"));
        var weaponData = new ObjectField();
        weaponData.objectType = typeof(Projectile);
        weaponData.bindingPath = "weapon";
        dataRoot.Add(weaponData);

        // sprites
        Button newData = new Button(() =>
        {
            Enemy newEnemy = CreateInstance<Enemy>();
            newEnemy.objectName = "Enemy";
            var path = "Assets/Resources/Gauntlet/Prefabs/Enemies";
            AssetDatabase.CreateAsset(newEnemy, AssetDatabase.GenerateUniqueAssetPath(path + "/Enemy-00.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            objectData.value = newEnemy;
            UpdateBinding();
        });
        newData.text = "New";
        spriteRoot.Add(newData);
        spriteRoot.Add(new Spacer(30));
        spriteRoot.Add(new Label("Enemy Sprite:"));
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

        objectTileSprite.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
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
                (objectData.value as Enemy).mainSprite = change as Sprite;
            }
            Repaint();
            parentWindow.rebindPrefabListView();
        });

        spriteRoot.Add(objectTileSpriteImage);
    }

    public void UpdateBinding()
    {
        if (enemy != null)
        {
            // Create serialization object
            SerializedObject so = new SerializedObject(enemy);
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
