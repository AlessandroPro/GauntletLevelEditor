using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    [Serializable]
    public class Level
    {
        public List<string> resources = new List<string>();
        public List<Game.GameObject> GameObjects = new List<Game.GameObject>();
        public int TimeLimit = 0;
    }

    [Serializable]
    public class GameObject
    {
        public string name = "GameObject";
        public bool enabled = true;
        public bool destroyOnUnload = true;
        public List<Component> Components;

        public GameObject()
        {
            Components = new List<Component>();
        }
    }

    [Serializable]
    public class Component
    {
        public string type = "Component";
        public bool enabled = true;
    }

    [Serializable]
    public class Transform : Component
    {
        public Position Position;
        public Transform()
        {
            type = "Transform";
            Position = new Position();
        }
    }

    [Serializable]
    public class Sprite : Component
    {
        public Texture Texture;
        public DimensionList Dimensions;
        public int layer;
        public Sprite()
        {
            type = "Sprite";
            Dimensions = new DimensionList();
            Texture = new Texture();
            layer = 0;
        }

    }

    [Serializable]
    public class RigidBody : Component
    {
        public int BodyType = 1;
        public RigidBody()
        {
            type = "RigidBody";
        }
    }

    [Serializable]
    public class CircleCollider : Component
    {
        public float radius = 32.0f;
        public bool trigger = false;

        public CircleCollider()
        {
            type = "CircleCollider";
        }

        public CircleCollider(bool isTrigger) : this()
        {
            trigger = isTrigger;
        }
    }

    [Serializable]
    public class PolygonCollider : Component
    {
        public bool trigger = false;
        public Box box;
        public PolygonCollider()
        {
            type = "PolygonCollider";
            box = new Box();
        }

        public PolygonCollider(bool isTrigger) : this()
        { 
            trigger = isTrigger;
        }
    }

    [Serializable]
    public class Player : Component
    {
        public string weaponPrefabGUID;
        public Player()
        {
            type = "Player";
            weaponPrefabGUID = "";
        }
    }

    [Serializable]
    public class Enemy : Component
    {
        public string weaponPrefabGUID;
        public Enemy()
        {
            type = "Enemy";
            weaponPrefabGUID = "";
        }
    }


    [Serializable]
    public class SpawnFactory : Component
    {
        public string enemyPrefabGUID;
        public SpawnFactory()
        {
            type = "SpawnFactory";
            enemyPrefabGUID = "";
        }
    }

    [Serializable]
    public class Item : Component
    {
        public Item()
        {
            type = "Item";
        }
    }

    [Serializable]
    public class SpawnPoint : Component
    {
        public SpawnPoint()
        {
            type = "SpawnPoint";
        }
    }

    [Serializable]
    public class Portal : Component
    {
        public Portal()
        {
            type = "Portal";
        }
    }

    [Serializable]
    public class Projectile : Component
    {
        public Projectile()
        {
            type = "Projectile";
        }
    }

    [Serializable]
    public class Text : Component
    {

    }

    [Serializable]
    public class Position
    {
        public float X = 0.0f;
        public float Y = 0.0f;
    }

    [Serializable]
    public class Box
    {
        public float height = 64.0f;
        public float width = 64.0f;
    }

    [Serializable]
    public class Texture
    {
        public string textureAssetGUID = "";
    }

    [Serializable]
    public class DimensionList
    {
        public int Left = 0;
        public int Top = 0;
        public int Width = 64;
        public int Height = 64;
    }

    [Serializable]
    public class MetaData
    {
        public string type = "";
        public string guid = "";
        public string path = "";
    }
}


