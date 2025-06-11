using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public GameObject uiCanvas;         
    public GameObject gameplayRoot;     

    public void StartGame()
    {
        uiCanvas.SetActive(false);      
        gameplayRoot.SetActive(true);   
    }
}
