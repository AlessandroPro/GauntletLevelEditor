﻿using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;
using System;

public enum ItemTypes
{
    HealthBoost,
    AttackBoost,
    WalkSpeedBoost,
    ThrowSpeedBoost,
    Key
}

public class GauntletItemEditor : PrefabEditor
{
    public Item item;
    public void OnEnable()
    {
        setupWindow();

        // Data
        dataRoot.Add(new Label("Choose a Item:"));
        objectData = new ObjectField();
        objectData.objectType = typeof(Item);
        dataRoot.Add(objectData);

        objectData.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
        {
            var change = (evt.target as ObjectField).value;
            item = change as Item;
            UpdateBinding();
        });

        dataRoot.Add(new Spacer(30));
        dataRoot.Add(new Label("Name:"));
        nameTextField = new TextField();
        nameTextField.bindingPath = "objectName";
        dataRoot.Add(nameTextField);

        dataRoot.Add(new Spacer(30));
        dataRoot.Add(new Label("Item Type:"));
        var itemTypeEnumField = new EnumField(ItemTypes.HealthBoost);
        itemTypeEnumField.bindingPath = "itemType";
        dataRoot.Add(itemTypeEnumField);

        itemTypeEnumField.RegisterCallback<ChangeEvent<Enum>>((evt) =>
        {
            var change = evt.newValue;
            if (item)
            {
                item.itemType = Convert.ToInt32(change);
            }
        });

        // sprites
        Button newData = new Button(() =>
        {
            Item newItem = CreateInstance<Item>();
            newItem.objectName = "Item";
            var path = "Assets/Resources/Gauntlet/Prefabs/Items";
            AssetDatabase.CreateAsset(newItem, AssetDatabase.GenerateUniqueAssetPath(path + "/Item-00.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            objectData.value = newItem;
            UpdateBinding();
        });
        newData.text = "New";
        spriteRoot.Add(newData);
        spriteRoot.Add(new Spacer(30));
        spriteRoot.Add(new Label("Item Sprite:"));
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
                (objectData.value as Item).mainSprite = change as Sprite;
            }
            Repaint();
            parentWindow.rebindPrefabListView();
        });

        spriteRoot.Add(objectTileSpriteImage);
    }

    public void UpdateBinding()
    {
        if (item != null)
        {
            // Create serialization object
            SerializedObject so = new SerializedObject(item);
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

