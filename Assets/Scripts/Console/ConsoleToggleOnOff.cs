using System.Collections;
using UnityEngine;

public class ConsoleToggleOnOff : MonoBehaviour
{
    [SerializeField] Transform slider;
    [SerializeField] Transform offTarget;
    [SerializeField] Transform onTarget;
    [SerializeField] float slideTime = 0.2f;

    void Start()
    {
        RealConsole.Instance.onPoweredStateChanged += OnPoweredStateChanged;
    }

    private void OnDestroy()
    {
        if (RealConsole.Instance != null)
            RealConsole.Instance.onPoweredStateChanged -= OnPoweredStateChanged;
    }

    private void OnPoweredStateChanged(bool isOn)
    {
        SFXManager.PlaySound(GlobalSFX.ToggleClick);
        SetState(isOn);
    }

    public void SetState(bool isOn)
    {
        StopAllCoroutines();

        if (isOn)
            StartCoroutine(Slide(offTarget.localPosition, onTarget.localPosition));
        else
            StartCoroutine(Slide(onTarget.localPosition, offTarget.localPosition));
    }

    IEnumerator Slide(Vector3 from, Vector3 to)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / slideTime;

            slider.localPosition = Vector3.Lerp(from, to, t);

            yield return null;
        }
    }
}
