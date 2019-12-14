using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MapObject
{
    public int health;
    public int moveSpeed;
    public Projectile weapon;
    // Start is called before the first frame update
    public override Game.GameObject save()
    {
        Game.GameObject gameObject = new Game.GameObject();
        
        gameObject.Components.Add(new Game.CircleCollider());
        gameObject.Components.Add(new Game.RigidBody());

        Game.Player playerComp = new Game.Player();
        
        if(weapon != null)
        {
            playerComp.weaponPrefabGUID = weapon.prefabGuid;
            if (!GauntletLevel.gamePrefabs.ContainsKey(weapon.prefabGuid))
            {
                GauntletLevel.gamePrefabs.Add(weapon.prefabGuid, weapon);
            }
        }
        gameObject.Components.Add(playerComp);

        return gameObject;
    }
}
