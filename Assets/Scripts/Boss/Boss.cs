using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform whereToGo;
    [SerializeField] private Transform whereItLive;

    [SerializeField] private float BossLife = 100;


    bool isSet = false;

    bool phase2 = false;

    [SerializeField] float force = 1;
    [SerializeField] float frequence = 1;
    [SerializeField] float myTime = 0;

    SpriteRenderer spriteRenderer;
    [SerializeField] float timeToRed = 0.4f;
    
    Coroutine patern;


    float PatternCD = 4f;


    public List<GameObject> EasyPattern = new List<GameObject>();
    public List<GameObject> HardPattern = new List<GameObject>();

    AudioSource aaaaaaaaaa;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        aaaaaaaaaa = GetComponent<AudioSource>();   
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
        patern = StartCoroutine(DoPattern());
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
        patern = StartCoroutine(DoPattern());
    }

    public IEnumerator EndBoss()
    {
        while (Vector3.Distance(transform.position, whereItLive.position) > .05f)
        {
            transform.position = Vector3.Lerp(transform.position, whereItLive.position, Time.deltaTime);
            yield return null;
        }
        print("Good");
        GameManager.Instance.gameState = GameManager.GameStates.Invader;
        GameManager.Instance.wave.CreateWave();

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSet || !collision.gameObject.CompareTag("Player")) return;

        UIManager.Instance.AddComboPart();
        ScoreManager.Instance.SetScore(ScoreManager.Instance.score + (1499 * UIManager.Instance.currentCombo));
        
        Destroy(Instantiate(Resources.Load<GameObject>("Prefabs/Particles/Explosion"), collision.transform.position, Quaternion.Euler(-90f, 0, 0)), 1f);
        Destroy(collision.gameObject);
        BossLife -= 1f;

        spriteRenderer.color = Color.red;
        spriteRenderer.DOColor(Color.white, 0.15f);

        if (phase2 && BossLife < 0f) {

            isSet = false;
            phase2 = false;
            BossLife = 100;
            aaaaaaaaaa.Play();
            StopCoroutine(patern);
            StartCoroutine(Music.Instance.ToJazz());
            StartCoroutine(EndBoss());

            ScoreManager.Instance.AddScore(999998, Color.red, "BOSS CLEAR BONUS");

            return;
        }
        else if (BossLife < 0f) {
            BossLife = 100;
            ScoreManager.Instance.AddScore(54321, new Color(255, 125, 125), "BOSS PHASE 1 BONUS");
            phase2 = true;
            print("Phase2");
        }
        

        float t = 1 - (BossLife / 100f) + .2f;
        frequence = Mathf.Lerp(1, 4.5f, t);
    }
}

