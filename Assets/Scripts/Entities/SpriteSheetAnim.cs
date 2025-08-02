using UnityEngine;

public class SpriteSheetAnim : MonoBehaviour
{
    [SerializeField] float interval;
    [SerializeField] Sprite[] anim;
    float elaspedTime;
    int spriteID;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        elaspedTime += Time.deltaTime;
        if (elaspedTime >= interval)
        {
            elaspedTime -= interval;
            DisplayNext();
        }
    }

    void DisplayNext()
    {
        spriteRenderer.sprite = anim[spriteID % anim.Length];
        spriteID++;
    }
}
