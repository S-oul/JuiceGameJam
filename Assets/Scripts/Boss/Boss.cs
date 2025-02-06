using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform whereToGo;

    bool isSet = false;

    [SerializeField] float force = 1;
    [SerializeField] float frenquence = 1;
    [SerializeField] float myTime=0;


    // Update is called once per frame
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
            transform.position = new Vector3(Mathf.Sin(myTime * force) * frenquence, transform.position.y, transform.position.z);
        }
    }

}

