using UnityEngine;

public class CanvasActivator : MonoBehaviour
{
    [SerializeField] private GameObject canvasToEnable;

    private void Start()
    {
        if (canvasToEnable != null)
            canvasToEnable.SetActive(true);
    }
}
