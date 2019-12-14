using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MapObject
{
    public override Game.GameObject save()
    {
        Game.GameObject gameObject = new Game.GameObject();
        gameObject.Components.Add(new Game.SpawnPoint());

        gameObject.name = objectName;

        return gameObject;
    }
}

