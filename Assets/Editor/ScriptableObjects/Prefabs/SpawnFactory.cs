using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFactory : MapObject
{
    public int health = 20;
    public int timeInterval = 1;
    public Enemy enemy;
    public Item dropItem;
    public override Game.GameObject save()
    {
        Game.GameObject gameObject = new Game.GameObject();

        gameObject.Components.Add(new Game.CircleCollider());

        Game.SpawnFactory sfComp = new Game.SpawnFactory();
        sfComp.health = health;
        sfComp.timeInterval = timeInterval;

        if (enemy != null)
        {
            sfComp.enemyPrefabGUID = enemy.prefabGuid;
            if(!GauntletLevel.gamePrefabs.ContainsKey(enemy.prefabGuid))
            {
                GauntletLevel.gamePrefabs.Add(enemy.prefabGuid, enemy);
            }
        }

        if(dropItem != null)
        {
            sfComp.itemPrefabGUID = dropItem.prefabGuid;
            if (!GauntletLevel.gamePrefabs.ContainsKey(dropItem.prefabGuid))
            {
                GauntletLevel.gamePrefabs.Add(dropItem.prefabGuid, dropItem);
            }
        }

        gameObject.Components.Add(sfComp);

        return gameObject;
    }
}
