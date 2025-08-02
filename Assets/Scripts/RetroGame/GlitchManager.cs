using UnityEngine;

public class GlitchManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int glitchTime = 30;
    float elapsedTime01;

    [Header("Visuals")]
    [SerializeField] Material ppGlitchMat;
    AudioSource audioSource;

    const string GLITCH_VAR_NAME = "_GlitchAmount";
    int glitchVarID;

    public float VisualGlitchAmount
    {
        get
        {
            //Only last part of glitch time visible
            float visible = Mathf.InverseLerp(0.66f, 1, elapsedTime01);
            //Square for me effect toward the end
            return visible * visible;
        }
    }

    private void Awake()
    {
        glitchVarID = Shader.PropertyToID(GLITCH_VAR_NAME);
        audioSource =GetComponent<AudioSource>();
    }

    private void Start()
    {
        ppGlitchMat.SetFloat(glitchVarID, 0);
    }

    private void OnDestroy()
    {
        if (ppGlitchMat != null)
            ppGlitchMat.SetFloat(glitchVarID, 0);
    }

    void Update()
    {
        if (!RetroGameManager.Instance.GameIsPlaying)
            return;
        elapsedTime01 += Time.deltaTime / glitchTime;
        float amount = VisualGlitchAmount;

        if(amount>0 && !audioSource.isPlaying)
            audioSource.Play();
        audioSource.volume = Mathf.Lerp(0, 1, amount);

        ppGlitchMat.SetFloat(glitchVarID, amount);
        GlitchRumbleCamera.Instance.ShakeForFrame(amount);

        if (elapsedTime01 >= 1)
        {
            Crash();
        }
    }

    void Crash()
    {
        audioSource.Stop();
        GlitchRumbleCamera.Instance.ResetCam();
        ppGlitchMat.SetFloat(glitchVarID, 3);
        RetroGameManager.Instance.Crash();
    }
}
