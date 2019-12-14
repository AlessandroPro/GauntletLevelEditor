using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class GauntletLevel : ScriptableObject
{
    public static Dictionary<string, MapObject> gamePrefabs;
    public static Dictionary<string, Texture> gameTextures;
    public static HashSet<string> savedAssets;

    private const int gameCellSize = 64;

    public string levelName = "Level";
    public int size = 0;
    public int timeLimit = 60;

    public int rowCount = 0;
    public int columnCount = 0;
    public int cellSize = 0;
    public int numLayers = 0;

    public List<ListLayerWrapper> levelLayers = new List<ListLayerWrapper>();
    
    public void initialize(string _levelName, int _size, int mapWidth)
    {
        levelName = _levelName;
        size = _size;

        rowCount = size;
        columnCount = size;
        numLayers = 3;

        cellSize = mapWidth / size;

        for (int l = 0; l < numLayers; l++)
        {
            var layer = new ListLayerWrapper();
            levelLayers.Add(layer);
            for (int r = 0; r < rowCount; r++)
            {
                var row = new ListRowWrapper();
                layer.rows.Add(row);
                for (int c = 0; c < columnCount; c++)
                {
                    row.mapObjs.Add(null);
                }
            }
        }
    }

    public void saveLevel(string path, int levelIndex)
    {
        gamePrefabs = new Dictionary<string, MapObject>();
        gameTextures = new Dictionary<string, Texture>();

        Game.Level levelData = new Game.Level();
        levelData.TimeLimit = timeLimit;

        for (int l = 0; l < numLayers; l++)
        {
            var layer = levelLayers[l];
            for (int r = 0; r < rowCount; r++)
            {
                var row = layer.rows[r];
                for (int c = 0; c < columnCount; c++)
                {
                    if (row.mapObjs[c])
                    {
                        Game.GameObject gameObject = saveMapObject(row.mapObjs[c], l, c, r);
                        levelData.GameObjects.Add(gameObject);
                    }
                }
            }
        }

        var prefabMetas = saveGamePrefabs(path);
        foreach(var prefabMetaFile in prefabMetas)
        {
            levelData.resources.Add(prefabMetaFile);
        }

        var textureAssetMetas = saveTextureAssets(path);
        foreach(var textureAssetMetaFile in textureAssetMetas)
        {
            levelData.resources.Add(textureAssetMetaFile);
        }

        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        var json = JsonConvert.SerializeObject(levelData, settings);
        StreamWriter writer = new StreamWriter(path + "/Levels/level_" + levelIndex.ToString() + ".json");
        writer.Write(json);
        writer.Close();

        
    }

    Game.GameObject saveMapObject(MapObject mapObject, int layer, int x, int y)
    {
        Game.GameObject gameObject = mapObject.save();

        Game.Transform transform = new Game.Transform();
        transform.Position.X = x * gameCellSize;
        transform.Position.Y = y * gameCellSize;
        gameObject.Components.Add(transform);

        if(mapObject.mainSprite)
        {
            Game.Sprite sprite = new Game.Sprite();
            Rect spriteRect = mapObject.mainSprite.rect;
            sprite.Dimensions.Left = (int)spriteRect.x;
            sprite.Dimensions.Top = (int)spriteRect.y;
            if (spriteRect.width < mapObject.mainSprite.texture.width)
            {
                sprite.Dimensions.Top = (int)(mapObject.mainSprite.texture.height - spriteRect.y - spriteRect.height);
            }
            sprite.layer = layer;
            var spriteTextureGUID = StringToGUID(AssetDatabase.GetAssetPath(mapObject.mainSprite.texture));
            if (!gameTextures.ContainsKey(spriteTextureGUID.ToString()))
            {
                gameTextures.Add(spriteTextureGUID.ToString(), mapObject.mainSprite.texture);
            }
            sprite.Texture.textureAssetGUID = spriteTextureGUID.ToString();
            gameObject.Components.Add(sprite);
        }

        gameObject.name = mapObject.objectName;

        return gameObject;
    }

    static Guid StringToGUID(string value)
    {
        // Create a new instance of the MD5CryptoServiceProvider object.
        MD5 md5Hasher = MD5.Create();
        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hasher.ComputeHash(System.Text.Encoding.Default.GetBytes(value));
        return new Guid(data);
    }

    List<string> saveTextureAssets(string path)
    {
        List<string> textureAssetMetas = new List<string>();
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        foreach (var gameTexture in gameTextures)
        {
            string textureFilePath = AssetDatabase.GetAssetPath(gameTexture.Value);
            string textureName = Path.GetFileName(textureFilePath);

            var textureAssetMeta = new Game.MetaData();
            textureAssetMeta.type = "TextureAsset";
            textureAssetMeta.guid = gameTexture.Key;
            textureAssetMeta.path = "../Assets/Images/" + textureName;

            textureAssetMetas.Add(textureAssetMeta.path + ".meta");

            if (!savedAssets.Contains(gameTexture.Key))
            {
                // Save meta file for the texture asset
                var jsonMeta = JsonConvert.SerializeObject(textureAssetMeta, settings);
                StreamWriter writer1 = new StreamWriter(path + "/Images/" + textureName + ".meta");
                writer1.Write(jsonMeta);
                writer1.Close();

                // Save the texture asset file
                try
                {
                    // Will not overwrite if the destination file already exists.
                    File.Copy(textureFilePath, path + "/Images/" + textureName, true);
                }
                catch (IOException copyError)
                {
                    Debug.LogError(copyError.Message);
                }

                if (!savedAssets.Contains(gameTexture.Key))
                {
                    savedAssets.Add(gameTexture.Key);
                }
            }
        }

        return textureAssetMetas;
    }

    List<string> saveGamePrefabs(string path)
    {
        List<string> prefabMetas = new List<string>();
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        int prefabNum = 0;
        foreach(var prefab in gamePrefabs)
        {
            string prefabFileName = "prefab" + prefabNum.ToString() + "_" + prefab.Value.objectName;

            var prefabMeta = new Game.MetaData();
            prefabMeta.type = "PrefabAsset";
            prefabMeta.guid = prefab.Value.prefabGuid;
            prefabMeta.path = "../Assets/Prefabs/" + prefabFileName + ".prefab";

            prefabNum++;
            prefabMetas.Add("../Assets/Prefabs/" + prefabFileName + ".meta");

            if (!savedAssets.Contains(prefab.Key))
            {
                // Save meta file for the prefab
                var jsonMeta = JsonConvert.SerializeObject(prefabMeta, settings);
                StreamWriter writer1 = new StreamWriter(path + "/Prefabs/" + prefabFileName + ".meta");
                writer1.Write(jsonMeta);
                writer1.Close();

                // Save the prefab file
                Game.GameObject gameObject = saveMapObject(prefab.Value, 0, 0, 0);
                var jsonPrefab = JsonConvert.SerializeObject(gameObject, settings);
                StreamWriter writer2 = new StreamWriter(path + "/Prefabs/" + prefabFileName + ".prefab");
                writer2.Write(jsonPrefab);
                writer2.Close();
            }
            if (!savedAssets.Contains(prefab.Key))
            {
                savedAssets.Add(prefab.Key);
            }
        }

        return prefabMetas;
    }
}
