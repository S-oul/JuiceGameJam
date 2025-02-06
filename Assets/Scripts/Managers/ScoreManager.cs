using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TMP_Text scoreTxt;
    
    public long score;
    private GameObject scorePrefab;

    private void Awake()
    {
        if(Instance)
            Destroy(gameObject);
        Instance = this;

        scorePrefab = Resources.Load<GameObject>("UI/ScoreName");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            AddScore(123456789, Color.red, "Giga bonus !!!");
        }
    }

    public void AddScore(long score, Color color, string name = "")
    {
        GameObject go = Instantiate(scorePrefab, transform);
        TMP_Text nameTxt = go.GetComponent<TMP_Text>();
        TMP_Text scoreTxt = go.transform.GetChild(0).GetComponent<TMP_Text>();

        nameTxt.color = color;
        scoreTxt.color = color;

        nameTxt.text = name;
        scoreTxt.DOText("+" + score.ToString("D9"), 0.1f);
        
        //SEQUENCE
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(go.transform.DOScale(1, 0.25f));
            sequence.Append(scoreTxt.transform.DOScale(1, 0.1f));
            sequence.AppendCallback(() => StartCoroutine(PlayScoreAnimation(score, scoreTxt)));
            sequence.AppendInterval(CalculateScoreAnimationDuration(score) + 3f);
            sequence.Append(go.transform.DOScale(0, 0.25f));
            sequence.AppendCallback(() => Destroy(go));
            sequence.Play();
        }
    }

    public void SetScore(long score)
    {
        this.score = score;
        scoreTxt.text = this.score.ToString("D9");

        scoreTxt.transform.localScale *= 1.2f;
        scoreTxt.transform.DOScale(1, 0.025f);
    }

    private IEnumerator PlayScoreAnimation(long score, TMP_Text text)
    {
        while (score > 0)
        {
            switch (score)
            {
                case > 100000000:
                    score-=100000000;
                    SetScore(this.score + 100000000);
                    break;
                
                case > 10000000:
                    score-=10000000;
                    SetScore(this.score + 10000000);
                    break;
                
                case > 1000000:
                    score-=1000000;
                    SetScore(this.score + 1000000);
                    break;
                
                case > 100000:
                    score-=100000;
                    SetScore(this.score + 100000);
                    break;
                
                case > 10000:
                    score-=10000;
                    SetScore(this.score + 10000);
                    break;
                
                case > 1000:
                    score-=1000;
                    SetScore(this.score + 1000);
                    break;
                
                case > 100:
                    score-=100;
                    SetScore(this.score + 100);
                    break;
                
                case > 10:
                    score-=10;
                    SetScore(this.score + 10);
                    break;
                
                default:
                    score--;
                    SetScore(this.score + 1);
                    break;
            }

            text.text = "+" + score.ToString("D9");
            yield return new WaitForSecondsRealtime(0.035f);
        }
    }
    
    private float CalculateScoreAnimationDuration(long score)
    {
        int iterations = 0;

        while (score > 0)
        {
            switch (score)
            {
                case > 100000000:
                    score -= 100000000;
                    break;
                case > 10000000:
                    score -= 10000000;
                    break;
                case > 1000000:
                    score -= 1000000;
                    break;
                case > 100000:
                    score -= 100000;
                    break;
                case > 10000:
                    score -= 10000;
                    break;
                case > 1000:
                    score -= 1000;
                    break;
                case > 100:
                    score -= 100;
                    break;
                case > 10:
                    score -= 10;
                    break;
                default:
                    score--;
                    break;
            }
            iterations++;
        }
        return iterations * 0.035f;
    }
}
