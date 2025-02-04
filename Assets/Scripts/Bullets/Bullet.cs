using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Vector3 startVelocity;
    [SerializeField] float TimeToDie = 5f;
    // Start is called before the first frame update
    void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = startVelocity;
        StartCoroutine(WaitToDie());
    }

    IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(TimeToDie);
        Destroy(this.gameObject);
    }
}
