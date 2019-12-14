using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFactory : MapObject
{
    public int health;
    public int moveSpeed;
    public Enemy enemy;
    // Start is called before the first frame update
    public override Game.GameObject save()
    {
        Game.GameObject gameObject = new Game.GameObject();

        gameObject.Components.Add(new Game.CircleCollider());
        gameObject.Components.Add(new Game.RigidBody());

        Game.SpawnFactory sfComp = new Game.SpawnFactory();

        if (enemy != null)
        {
            sfComp.enemyPrefabGUID = enemy.prefabGuid;
            if(!GauntletLevel.gamePrefabs.ContainsKey(enemy.prefabGuid))
            {
                GauntletLevel.gamePrefabs.Add(enemy.prefabGuid, enemy);
            }
        }
        gameObject.Components.Add(sfComp);

        return gameObject;
    }
}
