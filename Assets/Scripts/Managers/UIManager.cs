using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Image[] lifes = new Image[3];

    private void Awake()
    {
        if(Instance)
            Destroy(gameObject);
        Instance = this;
    }

    public void Yippie(int health)
    {
        lifes[health - 1].transform.DOScale(0, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        {
            lifes[health - 1].transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        });
    }
}
