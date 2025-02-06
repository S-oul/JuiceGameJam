using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private string collideWithTag = "Player";

    internal Action<Invader> onDestroy;

    DissolveMesh dissolver;
    Collider2D col;

    public Vector2Int GridIndex { get; private set; }

    public void Initialize(Vector2Int gridIndex)
    {
        this.GridIndex = gridIndex;
        dissolver = GetComponent<DissolveMesh>();
        col = GetComponent<Collider2D>();
    }

    public void OnDestroy()
    {
        if(Application.isPlaying) onDestroy?.Invoke(this);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag(collideWithTag)) { return; }
        Destroy(collision.gameObject);
        col.GetComponent<Collider2D>().enabled = false; 
        StartCoroutine(dissolver.DissolveAfterDelay());
        
    }

    public void Shoot()
    {
        Bullet.CreateBullet(EBulletType.BASIC, -transform.up, 1f)
            .At(shootAt.position);
    }
}
