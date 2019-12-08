using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapObject : ScriptableObject
{
    public string objectName = "MapObject";
    public Sprite mainSprite;
    public string prefabGuid;
    public abstract Game.GameObject save();
}
