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

    int playerLife = 3;
    [SerializeField] private float timeInvicible = 1f;
    bool IsInvicible = false;


    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateMovement();
        UpdateActions();
    }

    void UpdateMovement()
    {
        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(KeyCode.D)) moveX = 1;
        if (Input.GetKey(KeyCode.A)) moveX = -1;

        if (Input.GetKey(KeyCode.W)) moveY = 1;
        if (Input.GetKey(KeyCode.S)) moveY = -1;


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
        if (!collision.gameObject.CompareTag(collideWithTag) || IsInvicible) { return; }
        playerLife--;
        print(playerLife);
        if (playerLife == 0) GameManager.Instance.PlayGameOver();
        else StartCoroutine(InvicibiltyFrames());
    }

    public IEnumerator InvicibiltyFrames()
    {
        IsInvicible = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(timeInvicible);
        spriteRenderer.color = Color.white;
        IsInvicible = false;

    }
}
