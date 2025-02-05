using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Data")]
    public Vector3 direction;
    public float speed;
    
    private void Update()
    {
        transform.Translate(direction * (speed * Time.deltaTime));
    }

    public static Bullet CreateBullet(EBulletType type, Vector3 direction, float speed, float size = 0.2f)
    {
        Bullet bullet;
        if(type == EBulletType.PLAYER) bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/PlayerBullet")).GetComponent<Bullet>();
        else bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/EnemyBullet")).GetComponent<Bullet>();

        bullet.direction = direction;
        bullet.speed = speed;
        bullet.transform.localScale *= size;

        if (type == EBulletType.PLAYER)
            bullet.gameObject.tag = "Player";
        
        return bullet;
    }

    public void At(Vector3 position) => transform.position = position;

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

public enum EBulletType
{
    PLAYER,
    BASIC,
    HEAD_HUNTER,
    EXPLOSIVE
}
