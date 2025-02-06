using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DissolveMesh : MonoBehaviour
{
    public Texture2D dissolveTexture;
    public Color dissolveColor = Color.red;
    public float dissolveTime = 2f;
    public float dissolveSpeed = 2;
    private Material material;
    private float dissolveProgress = 0f;

    private void Start()
    {
        //StartCoroutine(DissolveAfterDelay());
    }

    public IEnumerator DissolveAfterDelay()
    {
        material = GetComponent<Renderer>().material;
        material.SetTexture("_DissolveTex", dissolveTexture);
        material.SetColor("_DissolveColor", dissolveColor);

        while (dissolveProgress < 1f)
        {
            dissolveProgress += Time.deltaTime* dissolveSpeed;
            material.SetFloat("_DissolveThreshold", dissolveProgress);
            yield return null;
        }

        Destroy(gameObject);
    }
}
