using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            gameManager.ShowGameClearUI();
        }
    }
}
