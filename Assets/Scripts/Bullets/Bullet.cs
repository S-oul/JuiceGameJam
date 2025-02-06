using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{

    private bool IsHeadHunter => type == EBulletType.HEAD_HUNTER;
    
    [Header("Bullet Data")]
    public EBulletType type;
    public Vector3 direction;
    public float speed;
    public Transform particleTransform;

    [Header("Head Hunter Data")] [ShowIf("IsHeadHunter")]
    public float followTime;

    private void Update()
    {
        switch (type)
        {
            case EBulletType.PLAYER:
            case EBulletType.BASIC:
                transform.position += (direction * (speed * Time.deltaTime));
                break;
            case EBulletType.HEAD_HUNTER:
                transform.position += (Vector3.Lerp(direction, (Player.Instance.transform.position - transform.position).normalized, 0.85f) * (speed * Time.deltaTime));
                followTime -= Time.deltaTime;
                if (followTime < 0)
                {
                    type = EBulletType.BASIC;
                    direction = (Player.Instance.transform.position - transform.position).normalized;
                }
                break;
            case EBulletType.EXPLOSIVE:
                transform.position += ((Player.Instance.transform.position - transform.position).normalized * (speed * Time.deltaTime));
                if (Vector3.Distance(Player.Instance.transform.position, transform.position) < 5f)
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

        if(bullet.particleTransform)
            bullet.particleTransform.localScale = Vector3.one * size;
        bullet.direction = direction;
        bullet.transform.up = direction;
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
        Destroy(gameObject,2);
    }
}

public enum EBulletType
{
    PLAYER,
    BASIC,
    HEAD_HUNTER,
    EXPLOSIVE
}
