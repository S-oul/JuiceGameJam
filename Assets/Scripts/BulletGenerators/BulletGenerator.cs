using System;
using System.Collections;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    private bool IsSpiral => type == EGeneratorType.SPIRAL;
    private bool IsFan => type == EGeneratorType.FAN;
    
    [Header("Generator Data")]
    public EGeneratorType type;
    [Min(1)] public int amountOfBullets;
    [Min(0f)] public float delta;
    [ReadOnly] public float duration;
    
    //SPIRAL
    [Header("Spiral Data"), ShowIf("IsSpiral")] public float angleDelta;
    
    //FAN
    [Header("Fan Data"), ShowIf("IsFan")] public int shootAmount;
    [ShowIf("IsFan")] public Vector2 minMaxAngle;

    [Header("Bullet Data")]
    public EBulletType bulletType;
    public float speed;
    public float size;

    private void Awake()
    {
        StartCoroutine(StartSalvo());
    }

    public IEnumerator StartSalvo()
    {
        switch (type)
        {
            case EGeneratorType.SPIRAL:
            {
                int remainingBullets = amountOfBullets;
                float angle = 0f;
                while (remainingBullets > 0)
                {
                    remainingBullets--;
                    angle += angleDelta;
                    Bullet.CreateBullet(EBulletType.BASIC, Quaternion.Euler(0, 0, angle) * transform.up, speed)
                        .At(transform.position);

                    yield return new WaitForSecondsRealtime(delta);
                }
                break;
            }
            case EGeneratorType.FAN:
            {
                int remainingBullets = amountOfBullets;
                while (remainingBullets > 0)
                {
                    remainingBullets--;
                    for (int i = 0; i < shootAmount; i++)
                    {
                        float t = (float)i / (shootAmount - 1);
                        float currentAngle = Mathf.Lerp(minMaxAngle.x, minMaxAngle.y, t);
    
                        Quaternion spreadRotation = Quaternion.AngleAxis(currentAngle, transform.forward);
                        Vector3 direction = spreadRotation * transform.up;
                        
                        Bullet.CreateBullet(EBulletType.BASIC, direction.normalized, speed)
                            .At(transform.position);;
                    }
                    yield return new WaitForSecondsRealtime(delta);
                }
                break;
            }
            case EGeneratorType.RING:
            {
                int remainingBullets = amountOfBullets;
                float angle = 0f;
                while (remainingBullets > 0)
                {
                    remainingBullets--;
                    angle += 360f / amountOfBullets;
                    Bullet.CreateBullet(EBulletType.BASIC, Quaternion.Euler(0, 0, angle) * transform.up, speed)
                        .At(transform.position);;
                }
                break;
            }
        }
        
        Destroy(gameObject);
    }

    private void OnValidate()
    {
        duration = amountOfBullets * delta;
    }
}

public enum EGeneratorType
{
    SPIRAL,
    FAN,
    RING
}
