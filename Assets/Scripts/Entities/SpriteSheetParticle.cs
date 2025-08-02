using System.Collections;
using UnityEngine;

public class SpriteSheetParticle : MonoBehaviour
{
    [SerializeField] float interval;
    [SerializeField] Sprite[] anim;

    private void Start()
    {
        StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < anim.Length; i++)
        {
            spriteRenderer.sprite = anim[i];
            yield return new WaitForSeconds(interval);
        }

        Destroy(gameObject);
    }
}
