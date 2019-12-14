using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MapObject
{

    public int itemType = 0;

    public override Game.GameObject save()
    {
        Game.GameObject gameObject = new Game.GameObject();
        gameObject.Components.Add(new Game.CircleCollider(true));

        Game.Item itemComp = new Game.Item();
        itemComp.itemType = itemType;

        gameObject.Components.Add(itemComp);

        return gameObject;
    }
}
