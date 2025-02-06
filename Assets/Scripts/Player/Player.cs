using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float deadzone = 0.3f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float bulletSize = 0.2f;
    [SerializeField] private float _bulletSpeed = 3f;
    
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private string collideWithTag = "Untagged";

    [SerializeField] int playerHP = 3;
    bool IsInvicible = false;
    internal Action<int> OnHit;
    float timeInvicible = 1.5f;

    SpriteRenderer spriteRenderer;

    private float lastShootTimestamp = Mathf.NegativeInfinity;

    private void Awake()
    {
        if (Instance) Destroy(this);
        else
        {
            Instance = this;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void Start()
    {
        OnHit += OnHitVoid;
        OnHit += UIManager.Instance.Yippie;
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
        if (Input.GetKey(KeyCode.LeftShift)) delta /= 2;
        transform.position = GameManager.Instance.KeepInBounds(transform.position + delta);
    }

    private void OnHitVoid(int score)
    {
        if (!IsInvicible)
        {
            if(playerHP > 0 ) playerHP--;

            if (playerHP <= 0) GameManager.Instance.PlayGameOver();
            StartCoroutine(InvicibiltyFrames());
        }
    }
    public IEnumerator InvicibiltyFrames()
    {
        IsInvicible = true;
        spriteRenderer.color = Color.red;
        spriteRenderer.DOColor(Color.white, timeInvicible);
        yield return new WaitForSeconds(timeInvicible);
        IsInvicible = false;

    }
    void UpdateActions()
    {
        if (Time.time > lastShootTimestamp + shootCooldown)
        {
            Shoot();
        }
    }


    void Shoot()
    {
        Bullet.CreateBullet(EBulletType.PLAYER, transform.up, _bulletSpeed, bulletSize)
            .At(shootAt.position);
        lastShootTimestamp = Time.time;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(collideWithTag)) { return; }
        if(playerHP > 0) OnHit?.Invoke(playerHP);
    }
}
