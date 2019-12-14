using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MapObject
{
    public override Game.GameObject save()
    {
        Game.GameObject gameObject = new Game.GameObject();
        gameObject.Components.Add(new Game.Portal());
        gameObject.Components.Add(new Game.CircleCollider(true));

        gameObject.name = objectName;

        return gameObject;
    }
}
