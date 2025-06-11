using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public GameObject uiCanvas;
    public GameObject gameplayRoot;
    public GameObject panelOverlay;     

    public void StartGame()
    {

        gameplayRoot.SetActive(true);
        uiCanvas.SetActive(false);
    }
}
