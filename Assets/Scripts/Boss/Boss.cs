using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform whereToGo;

    [SerializeField] private float BossLife = 100;


    bool isSet = false;

    bool phase2 = false;

    [SerializeField] float force = 1;
    [SerializeField] float frequence = 1;
    [SerializeField] float myTime = 0;

    SpriteRenderer spriteRenderer;
    [SerializeField] float timeToRed = 0.4f;

    Coroutine makeRed;

    float PatternCD = 4f;


    public List<GameObject> EasyPattern = new List<GameObject>();
    public List<GameObject> HardPattern = new List<GameObject>();



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
        StartCoroutine(DoPattern());
    }

    private void Update()
    {
        if (isSet)
        {
            myTime += Time.deltaTime;
            transform.position = new Vector3(Mathf.Sin(myTime * force) * frequence, transform.position.y, transform.position.z);

        }
    }

    IEnumerator DoPattern()
    {
        if (!phase2) Instantiate(EasyPattern[Random.Range(0, EasyPattern.Count - 1)], transform.position - new Vector3(0, 3.4f, 3), Quaternion.identity, transform);
        else Instantiate(HardPattern[Random.Range(0, HardPattern.Count - 1)], transform.position - new Vector3(0, 3.4f, 3), Quaternion.identity, transform);
        yield return new WaitForSeconds(PatternCD);
        StartCoroutine(DoPattern());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSet || !collision.gameObject.CompareTag("Player")) return;

        Destroy(collision.gameObject);
        BossLife -= 1f;

        if (BossLife < 0f)
        {
            BossLife = 50;
            phase2 = true;
            print("Phase2");
        }

        if (makeRed != null) StopCoroutine(makeRed);
        makeRed = StartCoroutine(MakeHimRed());

        float t = 1 - (BossLife / 100f) + .2f;
        frequence = Mathf.Lerp(1, 4.5f, t);
    }

    public IEnumerator MakeHimRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(timeToRed);
        spriteRenderer.color = Color.white;
    }
}

