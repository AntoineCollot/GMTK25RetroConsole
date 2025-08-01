using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeSimple : MonoBehaviour
{
    public float shakeTime = 0.3f;
    public float shakeFreq = 10;
    public float shakeAmplitude = 0.3f;
    public static ScreenShakeSimple Instance;

    RetroGameCamera followPlayer;

    private void Awake()
    {
        Instance = this;

        followPlayer = GetComponent<RetroGameCamera>();
    }

    [ContextMenu("Shake")]
    public void ShakeDebug()
    {
        Shake(1);
    }

    public void Shake(float amplitude01)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeC(Mathf.Clamp01(amplitude01)));
    }

    IEnumerator ShakeC(float amplitude01)
    {
        followPlayer.enabled = false;
        Vector3 originalPos = transform.position;
        float t = 0;
        float effectiveShakeTime = Mathf.Lerp(shakeTime * 0.5f, shakeTime, amplitude01);

        while(t<1)
        {
            t += Time.deltaTime / effectiveShakeTime;

            float amplitude = Curves.QuadEaseOut(shakeAmplitude * amplitude01, 0, Mathf.Clamp01(t));
            float shakeX = Curves.QuadEaseInOut(-1, 1, Mathf.PingPong(t * shakeFreq, 1));
            float shakeY = Curves.QuadEaseInOut(-1, 1, Mathf.PingPong(t * (shakeFreq+1), 1));
            transform.position = originalPos + new Vector3(shakeX, shakeY)* amplitude;

            yield return null;
        }
        followPlayer.enabled = true;
    }
}
