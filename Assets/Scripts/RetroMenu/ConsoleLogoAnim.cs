using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleLogoAnim : MonoBehaviour
{
    [SerializeField] float animDelay = 0.5f;
    [SerializeField] float animTime = 1;
    [SerializeField] Material mat;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        mat.SetFloat("_Threshold", 0);

        yield return new WaitForSeconds(animDelay);

        SFXManager.PlaySound(GlobalSFX.ConsoleJingle);

        float t = 0;
        while(t<1)
        {
            t += Time.deltaTime / animTime;

            mat.SetFloat("_Threshold", t);

            yield return null;
        }
    }
}
