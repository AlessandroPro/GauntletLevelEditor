﻿using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;


public class GauntletProjectileEditor : PrefabEditor
{
    public Projectile projectile;

    public void OnEnable()
    {
        setupWindow();

        // Data
        dataRoot.Add(new Label("Choose a Projectile:"));
        objectData = new ObjectField();
        objectData.objectType = typeof(Projectile);
        dataRoot.Add(objectData);

        objectData.RegisterCallback<ChangeEvent<Object>>((evt) =>
        {
            var change = (evt.target as ObjectField).value;
            projectile = change as Projectile;
            UpdateBinding();
        });

        dataRoot.Add(new Spacer(30));
        dataRoot.Add(new Label("Name:"));
        nameTextField = new TextField();
        nameTextField.bindingPath = "objectName";
        dataRoot.Add(nameTextField);

        addSlider(ref dataRoot, 20, 100, "Health:   ", "health");
        addSlider(ref dataRoot, 5, 100, "Walk Speed:   ", "walkSpeed");


        //dataRoot.Add(new IntegerField("MaxLives:"));
        //        dataRoot.Add(new Label("Weapon:"));
        //        dataRoot.Add(new Slider("Damage:", 0, 100));
        //        dataRoot.Add(new Slider("Throw Speed:", 0, 100));
        //        dataRoot.Add(new SliderInt("Pool Size", 0, 100));
        //        dataRoot.Add(new Slider("TimeInterval (s):", 0, 100));

        // sprites
        Button newData = new Button(() =>
        {
            Projectile newProjectile = CreateInstance<Projectile>();
            var path = "Assets/Resources/Gauntlet/Prefabs/Projectiles";
            AssetDatabase.CreateAsset(newProjectile, AssetDatabase.GenerateUniqueAssetPath(path + "/Projectile-00.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            objectData.value = newProjectile;
            UpdateBinding();
        });
        newData.text = "New";
        spriteRoot.Add(newData);
        spriteRoot.Add(new Spacer(30));
        spriteRoot.Add(new Label("Projectile Sprite:"));
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
                (objectData.value as Projectile).mainSprite = change as Sprite;
            }
            Repaint();
            parentWindow.rebindPrefabListView();
        });

        spriteRoot.Add(objectTileSpriteImage);
    }

    public void UpdateBinding()
    {
        if (projectile != null)
        {
            // Create serialization object
            SerializedObject so = new SerializedObject(projectile);
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
