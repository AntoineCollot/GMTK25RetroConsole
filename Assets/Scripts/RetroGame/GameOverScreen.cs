using System.Collections;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameOverText;
    //const float DELAY_GAME_OVER_TEXT = 3;
    const float DELAY_BACK_TO_MENU = 4;

    private void OnEnable()
    {
        if (RetroGameManager.Instance.GameIsPlaying)
        {
            gameObject.SetActive(false);
            return;
        }

        StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        gameOverText.gameObject.SetActive(false);
        
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

        yield return new WaitForSeconds(DELAY_BACK_TO_MENU);

        RetroConsoleManager.Instance.OpenInterface(false);
    }
}
