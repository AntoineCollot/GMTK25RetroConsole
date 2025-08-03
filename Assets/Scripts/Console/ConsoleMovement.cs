using System;
using System.Collections;
using UnityEngine;

public class ConsoleMovement : MonoBehaviour
{
    [SerializeField] Transform idlePosition;
    [SerializeField] Transform togglePosition;
    [SerializeField] Transform cartouchePosition;

    float lastTargetChangeTime;
    Transform currentTarget;
    Vector3 refPos;
    Quaternion refRot;

    const float SMOOTH = 0.15f;
    const float MAX_SMOOTH_TIME = 3;

    void Start()
    {
        currentTarget = idlePosition;
        RealConsole.Instance.onPoweredStateChanged += OnPowererdChanged;
        RealConsole.Instance.onCartridgeChanged += OnCartridgeChanged;
    }

    private void OnDestroy()
    {
        if(RealConsole.Instance != null)
        {
            RealConsole.Instance.onPoweredStateChanged -= OnPowererdChanged;
            RealConsole.Instance.onCartridgeChanged -= OnCartridgeChanged;
        }
    }

    private void OnCartridgeChanged(bool isOut)
    {
        lastTargetChangeTime = Time.time;
        SetTarget(cartouchePosition);
    }

    private void OnPowererdChanged(bool isOn)
    {
        lastTargetChangeTime = Time.time;
        if (isOn)
            StartCoroutine(SetTargetDelayed(0.2f, idlePosition));
        else
            SetTarget(togglePosition);
    }

    IEnumerator SetTargetDelayed(float delay, Transform target)
    {
        yield return new WaitForSeconds(delay);
        SetTarget(target);
    }

    void SetTarget(Transform target)
    {
        StopAllCoroutines();
        currentTarget = target;
    }

    void Update()
    {
        if (Time.time < lastTargetChangeTime + MAX_SMOOTH_TIME)
        {
            transform.position = Vector3.SmoothDamp(transform.position, currentTarget.position, ref refPos, SMOOTH);
            transform.localRotation = QuaternionUtils.SmoothDamp(transform.localRotation, currentTarget.localRotation, ref refRot, SMOOTH);
        }
    }

}
