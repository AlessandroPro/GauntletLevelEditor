using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MapObject
{
    public int health = 20;
    public int walkSpeed = 30;
    public int attackTimeInterval = 1;
    public int attackStyle = 0;
    public Projectile weapon;

    // Start is called before the first frame update
    public override Game.GameObject save()
    {
        Game.GameObject gameObject = new Game.GameObject();

        gameObject.Components.Add(new Game.CircleCollider());
        gameObject.Components.Add(new Game.RigidBody());

        Game.Enemy enemyComp = new Game.Enemy();
        enemyComp.health = health;
        enemyComp.walkSpeed = walkSpeed;
        enemyComp.attackTimeInterval = attackTimeInterval;
        enemyComp.attackStyle = attackStyle;

        if (weapon != null)
        {
            enemyComp.weaponPrefabGUID = weapon.prefabGuid;
            if (!GauntletLevel.gamePrefabs.ContainsKey(weapon.prefabGuid))
            {
                GauntletLevel.gamePrefabs.Add(weapon.prefabGuid, weapon);
            }
        }
        gameObject.Components.Add(enemyComp);

        return gameObject;
    }
}
