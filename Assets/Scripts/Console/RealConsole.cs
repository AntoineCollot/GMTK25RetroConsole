using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RealConsole : MonoBehaviour
{
    public bool isOn { get; private set; }

    [SerializeField] MeshRenderer consoleRenderer;
    [SerializeField] Material screenOnMat;
    [SerializeField] Material screenOffMat;
    Material consoleMaterial;

    float lastToggleTime;
    public InputMap inputs { get;private set; }

    public event Action<bool> onPoweredStateChanged;

    public static RealConsole Instance;

    const float MIN_TOGGLE_TIME = 0.5f;

    private void Awake()
    {
        Instance = this;
        inputs = new InputMap();
        consoleMaterial = consoleRenderer.sharedMaterial;
    }

    void Start()
    {
        ToggleOnOff();
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Gameplay.ToggleOnOff.performed += OnToggleOnOffPerformed;
    }

    private void OnDisable()
    {
        if (inputs != null)
        {
            inputs.Disable();
        }
    }

    private void OnDestroy()
    {
        if (inputs != null)
        {
            inputs.Dispose();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnToggleOnOffPerformed(InputAction.CallbackContext context)
    {
        if (Time.time >= lastToggleTime + MIN_TOGGLE_TIME)
            ToggleOnOff();
    }

    public void ToggleOnOff()
    {
        GlitchRumbleCamera.Instance.ResetCam();
        lastToggleTime = Time.time;
        isOn = !isOn;

        UpdateScreenMat();

        if (isOn)
            SFXManager.PlaySound(GlobalSFX.ConsoleTurnOn);
        else
            SFXManager.PlaySound(GlobalSFX.ConsoleTurnOff);

        onPoweredStateChanged?.Invoke(isOn);
    }

    void UpdateScreenMat()
    {
        List<Material> mats = new() { consoleMaterial };
        if (isOn)
            mats.Add(screenOnMat);
        else
            mats.Add(screenOffMat);

        consoleRenderer.SetMaterials(mats);
    }
}
