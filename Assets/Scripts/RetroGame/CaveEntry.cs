using System.Collections;
using UnityEngine;

public class CaveEntry : MonoBehaviour
{
    [SerializeField] Material roofMat;
    bool isOpen;
    Vector2Int gridPos;

    [SerializeField] float fadeTime;
    const string PROPERTY_NAME = "_Threshold";

    void Start()
    {
        isOpen = false;
        PlayerState.Instance.onPlayerPositionChanged += OnPlayerPositionChanged;
        gridPos = GameGrid.WordPosToGrid(transform.position);
        roofMat.SetFloat(PROPERTY_NAME, 0);
    }

    private void OnDestroy()
    {
        if (PlayerState.Instance != null)
            PlayerState.Instance.onPlayerPositionChanged -= OnPlayerPositionChanged;
    }

    private void OnPlayerPositionChanged(Vector2Int playerPos)
    {
        if (isOpen)
            return;

        if (playerPos != gridPos)
            return;

        Open();
    }

    void Open()
    {
        isOpen = true;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        float t = 0;
        while(t<1)
        {
            t += Time.deltaTime / fadeTime;

            roofMat.SetFloat(PROPERTY_NAME, t);

            yield return null;
        }
    }
}
