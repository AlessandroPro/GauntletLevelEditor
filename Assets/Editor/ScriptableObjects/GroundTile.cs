using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundTile-00", menuName = "Gauntlet Prefabs/Ground Tile")]
public class GroundTile : MapObject
{
    //public override void save()
    //{
    //    SimpleJSON.JSONObject node = new SimpleJSON.JSONObject();
    //    node.Add("name", objectName);

    //    SimpleJSON.JSONArray components = new SimpleJSON.JSONArray();
    //    components.Add("Sprite", "FFFF");
    //    components.Add("Sprite", "FFFF");

    //    node.Add("Components", components);
    //}

    public override Game.GameObject save()
    {
        Game.GameObject gameObject = new Game.GameObject();
        gameObject.Components.Add(new Game.Transform());
        gameObject.Components.Add(new Game.Sprite());

        gameObject.name = objectName;

        return gameObject;
    }
}
