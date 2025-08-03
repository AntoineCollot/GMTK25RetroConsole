using UnityEngine;

public class TutoPanel : MonoBehaviour
{
    [SerializeField] float wiggleRange;
    [SerializeField] float wiggleFreq;
    RectTransform rectT;
    Vector2 originalPos;

    private void Start()
    {
        rectT = transform as RectTransform;
        originalPos = rectT.anchoredPosition;
    }

    private void Update()
    {
        Wiggle();
    }

    void Wiggle()
    {
        Vector2 pos = originalPos;
        pos.y += Curves.QuadEaseInOut(-wiggleRange, wiggleRange, Mathf.PingPong(Time.time * wiggleFreq, 1)) * 0.5f;
        rectT.anchoredPosition = pos;
    }
}
