using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MapObject
{
    // Start is called before the first frame update
    public override Game.GameObject save()
    {
        Game.GameObject gameObject = new Game.GameObject();
        gameObject.Components.Add(new Game.Transform());
        gameObject.Components.Add(new Game.Sprite());

        gameObject.name = objectName;

        return gameObject;
    }
}
