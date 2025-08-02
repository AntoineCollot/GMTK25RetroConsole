using UnityEngine;

public class GlitchRumbleCamera : MonoBehaviour
{
    public static GlitchRumbleCamera Instance;
    float t;
    public float shakeFreq = 10;
    public float shakeAmplitude = 0.3f;

    Vector3 originalPos;

    void Awake()
    {
        Instance = this;
        originalPos = transform.position;
    }

   //[Range(0,1)] public float shake;
   // private void Update()
   // {
   //     ShakeForFrame(shake);
   // }

    public void ResetCam()
    {
        transform.position = originalPos;
    }

    public void ShakeForFrame(float amplitude01)
    {
        t += Time.deltaTime;

        float shakeX = Curves.QuadEaseInOut(-1, 1, Mathf.PingPong(t * shakeFreq, 1));
        float shakeY = Curves.QuadEaseInOut(-1, 1, Mathf.PingPong(t * (shakeFreq + 1), 1));
        transform.position = originalPos + new Vector3(shakeX, shakeY) * amplitude01 * shakeAmplitude;
    }
}
