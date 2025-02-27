using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Data")]
    public EBulletType type;
    public Vector3 direction;
    public float speed;
    
    private void Update()
    {
        switch (type)
        {
            case EBulletType.PLAYER:
            case EBulletType.BASIC:
                transform.Translate(direction * (speed * Time.deltaTime));
                break;
            case EBulletType.HEAD_HUNTER:
            case EBulletType.EXPLOSIVE:
                transform.Translate((Player.Instance.transform.position - transform.position).normalized * (speed * Time.deltaTime));
                if (type == EBulletType.EXPLOSIVE && Vector3.Distance(Player.Instance.transform.position, transform.position) < 5f)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    public static Bullet CreateBullet(EBulletType type, Vector3 direction, float speed, float size = 0.2f)
    {
        Bullet bullet = null;
        if(type == EBulletType.PLAYER) bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/PlayerBullet")).GetComponent<Bullet>();
        else bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/EnemyBullet")).GetComponent<Bullet>();

        bullet.direction = direction;
        bullet.speed = speed;
        bullet.gameObject.transform.localScale = Vector3.one * size;
        bullet.type = type;

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
