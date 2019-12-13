using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "GroundTile-00", menuName = "Gauntlet Prefabs/Ground Tile")]
public class GroundTile : MapObject
{
    public bool isCollider = false;
    public bool isTrigger = false;
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

        Game.PolygonCollider pCol;

        if(isCollider)
        {
            pCol = new Game.PolygonCollider();
            if(isTrigger)
            {
                pCol.trigger = true;
            }
            gameObject.Components.Add(pCol);
        }

        return gameObject;
    }
}
