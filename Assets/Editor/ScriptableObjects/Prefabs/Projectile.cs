using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MapObject
{
    // Start is called before the first frame update
    public override Game.GameObject save()
    {
        Game.GameObject gameObject = new Game.GameObject();
        gameObject.Components.Add(new Game.Projectile());
        gameObject.Components.Add(new Game.RigidBody());

        Game.CircleCollider col = new Game.CircleCollider();
        col.radius = 20.0f;
        gameObject.Components.Add(col);

        gameObject.name = objectName;

        return gameObject;
    }
}