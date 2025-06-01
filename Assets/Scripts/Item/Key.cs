using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Key : XRGrabInteractable
{
    public string password;
    
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        EnableColliderTrigger();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        DisableColliderTrigger();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // var door = other.gameObject.GetComponent<Door>();
        // if (door != null)
        // {
        //     if (door.password == password)
        //     {
        //         door.UnlockDoorSuccess();
        //         Destroy(gameObject);
        //     }
        //     else
        //     {
        //         door.UnlockDoorFailure();
        //     }
        // }
    }
    
    private void EnableColliderTrigger()
    {
        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }
    
    private void DisableColliderTrigger()
    {
        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = false;
        }
    }
}