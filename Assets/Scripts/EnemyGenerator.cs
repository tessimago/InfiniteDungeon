using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] prefabs;
    public Enemy[] enemyTypes;
    void Start(){
        enemyTypes = new Enemy[]{
            new Enemy_Standard(prefabs[0]),
            new Enemy_Shooter_Standard(prefabs[1]),

            new Enemy_Fast(prefabs[0]),
            new Enemy_Shooter_Fast(prefabs[1]),

            new Enemy_Strong(prefabs[0]),
            new Enemy_Shooter_Strong(prefabs[1]),
        };
    }

    public void spawnRandomEnemy(Vector3Int coords, int distance){
        int limit;
        if (distance < 100) limit = 2;
        else if(distance < 250) limit = 4;
        else limit = enemyTypes.Length;
        
        spawnEnemy(Random.Range(0, limit), coords);
    }
    public void spawnEnemy(int type, Vector3Int coords){
        var e = Instantiate(enemyTypes[type].enemy, coords, Quaternion.identity);
        enemyTypes[type].Set(e);
    }

}

public abstract class Enemy
{
    public GameObject enemy;
    public int xpDrop = 10;
    public int maxHealth;
    public float speed;
    public float attackSpeed = 3f;
    public int damage;
    public Color color;
    public EnemyBehaviour.BEHAVIOURS behaviour;

    public virtual void Set(GameObject e)
    {
        Debug.Log("Doing Base");
        e.GetComponent<EnemyStats>().Initialize(maxHealth, xpDrop);
        var behaviourComponent = e.GetComponent<EnemyBehaviour>();
        if (behaviourComponent != null)
        {
            behaviourComponent.damage = damage;
            behaviourComponent.speed = speed;
            behaviourComponent.attackSpeed = attackSpeed;
            behaviourComponent.behaviour = behaviour;
        }
        else
        {
            Debug.LogError("EnemyBehaviour component not found!");
        }

        var renderer = e.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = color;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found!");
        }
    }
}
public abstract class Enemy_Shooter : Enemy{
    public float bulletSpeed;
    public float bulletSize = 0.2f;
    public float range = 8f;
    public override void Set(GameObject e)
    {
        base.Set(e);
        var behaviourComponent = e.GetComponent<EnemyBehaviour>();
        if (behaviourComponent != null)
        {
            behaviourComponent.bulletSpeed = bulletSpeed;
            behaviourComponent.bulletSize = bulletSize;
            behaviourComponent.range = range;
        }
        else
        {
            Debug.LogError("EnemyBehaviour component not found!");
        }
    }
}
public class Enemy_Shooter_Strong : Enemy_Shooter
{
    public Enemy_Shooter_Strong(GameObject enemyPrefab)
    {
        enemy = enemyPrefab;
        maxHealth = 75;
        speed = 3;
        damage = 15;
        range = 20f;
        bulletSpeed = 15f;
        bulletSize = 0.5f;
        color = Color.red;
        behaviour = EnemyBehaviour.BEHAVIOURS.SHOOT;
    }
}

public class Enemy_Shooter_Standard : Enemy_Shooter
{
    public Enemy_Shooter_Standard(GameObject enemyPrefab)
    {
        enemy = enemyPrefab;
        maxHealth = 50;
        speed = 5;
        attackSpeed = 1f;
        damage = 7;
        bulletSpeed = 15f;
        color = Color.white;
        behaviour = EnemyBehaviour.BEHAVIOURS.SHOOT;
    }
}

public class Enemy_Shooter_Fast : Enemy_Shooter
{

    public Enemy_Shooter_Fast(GameObject enemyPrefab)
    {
        enemy = enemyPrefab;
        maxHealth = 35;
        speed = 8;
        attackSpeed = 0.6f;
        damage = 4;
        bulletSpeed = 25f;
        color = Color.yellow;
        behaviour = EnemyBehaviour.BEHAVIOURS.SHOOT;
    }
}


public class Enemy_Standard : Enemy
{
    public Enemy_Standard(GameObject enemyPrefab)
    {
        enemy = enemyPrefab;
        maxHealth = 100;
        speed = 5;
        damage = 10;
        color = Color.white;
        behaviour = EnemyBehaviour.BEHAVIOURS.RUSH;
    }
}

public class Enemy_Fast : Enemy
{
    public Enemy_Fast(GameObject enemyPrefab)
    {
        enemy = enemyPrefab;
        maxHealth = 50;
        speed = 10;
        damage = 5;
        color = Color.yellow;
        behaviour = EnemyBehaviour.BEHAVIOURS.RUSH;
    }
}

public class Enemy_Strong : Enemy
{
    public Enemy_Strong(GameObject enemyPrefab)
    {
        enemy = enemyPrefab;
        maxHealth = 250;
        speed = 3;
        damage = 25;
        color = Color.red;
        behaviour = EnemyBehaviour.BEHAVIOURS.RUSH;
    }
}

