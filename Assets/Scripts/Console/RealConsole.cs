using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RealConsole : MonoBehaviour
{
    public bool isOn { get; private set; }

    [SerializeField] GameObject mainMenuReal;
    [SerializeField] TutoControls tutoControls;

    [SerializeField] MeshRenderer consoleRenderer;
    [SerializeField] Material screenOnMat;
    [SerializeField] Material screenOffMat;
    Material consoleMaterial;

    public bool isCartridgeOut { get; private set; }

    float lastToggleTime;
    public InputMap inputs { get; private set; }

    public event Action<bool> onPoweredStateChanged;
    public event Action<bool> onCartridgeChanged;

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
        if (mainMenuReal.activeInHierarchy)
            StartCoroutine(WaitFirstInput());
        else
            StartCoroutine(FirstBoot());
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
            inputs.Gameplay.ToggleOnOff.performed -= OnToggleOnOffPerformed;
            inputs.Gameplay.Cartridge.performed -= OnCartridgePerformed;
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

    IEnumerator WaitFirstInput()
    {
        UpdateScreenMat();
        while (!inputs.Gameplay.A.IsPressed() && !inputs.Gameplay.Start.IsPressed())
        {
            yield return null;
        }

        mainMenuReal.SetActive(false);
        tutoControls.Show(false);
        tutoControls.enabled = false;
        StartCoroutine(FirstBoot());
    }

    IEnumerator FirstBoot()
    {
        yield return null;
        ToggleOnOff();
    }

    private void OnToggleOnOffPerformed(InputAction.CallbackContext context)
    {
        if (mainMenuReal.activeInHierarchy)
            return;

        if (Time.time >= lastToggleTime + MIN_TOGGLE_TIME)
            ToggleOnOff();
    }

    public void ToggleOnOff()
    {
        if (isCartridgeOut)
        {
            SFXManager.PlaySound(GlobalSFX.UICancel);
            return;
        }

        GlitchRumbleCamera.Instance.ResetCam();
        lastToggleTime = Time.time;
        isOn = !isOn;

        UpdateScreenMat();

        if (Time.time > 0.5f)
        {
            GlitchManager.ClearLastGlitch();
            if (isOn)
            {
                SFXManager.PlaySound(GlobalSFX.ConsoleTurnOn);
                inputs.Gameplay.Cartridge.performed -= OnCartridgePerformed;
            }
            else
            {
                SFXManager.PlaySound(GlobalSFX.ConsoleTurnOff);
                inputs.Gameplay.Cartridge.performed += OnCartridgePerformed;
            }
        }

        onPoweredStateChanged?.Invoke(isOn);
    }

    private void OnCartridgePerformed(InputAction.CallbackContext obj)
    {
        if (isOn)
            return;

        isCartridgeOut = !isCartridgeOut;
        SFXManager.PlaySound(GlobalSFX.Cartouche);
        onCartridgeChanged?.Invoke(isCartridgeOut);
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
