using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MapObject
{
    // Start is called before the first frame update
    public override Game.GameObject save()
    {
        Game.GameObject gameObject = new Game.GameObject();
        gameObject.Components.Add(new Game.Player());
        gameObject.Components.Add(new Game.CircleCollider());
        gameObject.Components.Add(new Game.RigidBody());

        return gameObject;
    }
}
