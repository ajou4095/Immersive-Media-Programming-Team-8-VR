using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject clearUI;
    [SerializeField] private GameObject gameOverUI;

#if UNITY_EDITOR
    [SerializeField] private SceneAsset nextScene;
#endif

    [SerializeField, HideInInspector]
    private string nextSceneName;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (nextScene != null)
        {
            nextSceneName = nextScene.name;
        }
#endif
    }

    private void Start()
    {
        if (clearUI != null) clearUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(false);
    }

    public void ShowGameClearUI()
    {
        if (clearUI != null) clearUI.SetActive(true);
    }

    public void ShowGameOverUI()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("gameOverUI가 연결되어 있지 않습니다.");
        }
    }

    public void GoToNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("There is no next scene.");
        }
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
