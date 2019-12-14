using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MapObject
{
    public int damage = 20;
    public int throwSpeed = 50;

    public override Game.GameObject save()
    {
        Game.GameObject gameObject = new Game.GameObject();
        gameObject.Components.Add(new Game.RigidBody());

        Game.CircleCollider col = new Game.CircleCollider();
        col.radius = 20.0f;
        gameObject.Components.Add(col);

        Game.Projectile proj = new Game.Projectile();
        proj.damage = damage;
        proj.throwSpeed = throwSpeed;
        gameObject.Components.Add(proj);

        gameObject.name = objectName;

        return gameObject;
    }
}