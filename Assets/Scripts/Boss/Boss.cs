using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform whereToGo;

    [SerializeField] private float BossLife = 100;

    bool isSet = false;

    [SerializeField] float force = 1;
    [SerializeField] float frequence = 1;
    [SerializeField] float myTime=0;

    SpriteRenderer spriteRenderer;
    [SerializeField] float timeToRed = 0.4f;

    Coroutine makeRed;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public IEnumerator StartBoss()
    {
        while (Vector3.Distance(transform.position, whereToGo.position) > .05f)
        {
            transform.position = Vector3.Lerp(transform.position, whereToGo.position, Time.deltaTime);
            yield return null;
        }
        GameManager.Instance.gameState += 1;
        isSet = true;
    }

    private void Update()
    {
        if (isSet)
        {
            myTime += Time.deltaTime;
            transform.position = new Vector3(Mathf.Sin(myTime * force) * frequence, transform.position.y, transform.position.z);
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        print("DEEDE");
        if (!isSet || !collision.gameObject.CompareTag("Player")) return;
        
        Destroy(collision.gameObject);
        BossLife -= 1f;
        if(makeRed != null) StopCoroutine(makeRed);
        makeRed = StartCoroutine(MakeHimRed());

        float t = 1 - (BossLife / 100f) + .2f;
        frequence = Mathf.Lerp(1,4.5f,t);
    }

    public IEnumerator MakeHimRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(timeToRed);
        spriteRenderer.color = Color.white;
    }
}

