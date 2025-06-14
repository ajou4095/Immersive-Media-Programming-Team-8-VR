using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class RayModeSwitcher : MonoBehaviour
{
    public XRRayInteractor directRay;
    public XRRayInteractor teleportRay;
    public InputActionReference switchModeAction; 

    private bool teleportMode = false;

    private void OnEnable()
    {
        switchModeAction.action.performed += OnSwitchMode;
    }

    private void OnDisable()
    {
        switchModeAction.action.performed -= OnSwitchMode;
    }

    private void OnSwitchMode(InputAction.CallbackContext context)
    {
        teleportMode = !teleportMode;
        UpdateMode();
    }

    private void UpdateMode()
    {
        directRay.gameObject.SetActive(!teleportMode);
        teleportRay.gameObject.SetActive(teleportMode);
    }
}
