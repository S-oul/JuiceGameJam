using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    
    [SerializeField] private float deadzone = 0.3f;
    [SerializeField] private float speed = 1f;
    
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private string collideWithTag = "Untagged";

    private float lastShootTimestamp = Mathf.NegativeInfinity;

    private void Awake()
    {
        if(Instance)
            Destroy(gameObject);
        Instance = this;
    }

    void Update()
    {
        UpdateMovement();
        UpdateActions();
    }

    void UpdateMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 delta = new Vector2(moveX * speed * Time.deltaTime, moveY * speed * Time.deltaTime);
        transform.position = GameManager.Instance.KeepInBounds(transform.position + delta);
    }

    void UpdateActions()
    {
        if (Input.GetKey(KeyCode.Space) 
            &&  Time.time > lastShootTimestamp + shootCooldown )
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Bullet.CreateBullet(EBulletType.PLAYER, transform.up, 3f)
            .At(shootAt.position);
        lastShootTimestamp = Time.time;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(collideWithTag)) { return; }

        GameManager.Instance.PlayGameOver();
    }
}
