using System.Collections;
using TMPro;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameOverText;

    private void OnEnable()
    {
        RetroGameManager.Instance.WinGame();
        StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        gameOverText.gameObject.SetActive(false);
        MusicManager.Instance.ClearLastAreaTheme();
        MusicManager.Instance.EnqueueTheme(Theme.MainTheme);

        yield return new WaitForSeconds(1);

        gameOverText.gameObject.SetActive(true);
        float c = 46 / 256.0f;
        gameOverText.color = new Color(c, c, c, 1);
        yield return new WaitForSeconds(1);
        c = 176 / 256.0f;
        gameOverText.color = new Color(c, c, c, 1);
        yield return new WaitForSeconds(1.2f);
        gameOverText.color = new Color(1, 1, 1, 1);
        ScreenShakeSimple.Instance.Shake(0.25f);
    }
}
