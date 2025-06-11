using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPortal : MonoBehaviour
{
    private GameObject player;
    private float radius;
    
    void Start()
    {
        player = GameObject.FindWithTag("MainCamera");
        radius = 2f;
    }
    
    private void Update()
    {
        var position1 = new Vector2(player.transform.position.x, player.transform.position.z);
        var position2 = new Vector2(transform.position.x, transform.position.z);
        var distance = Vector2.Distance(position1, position2);
        
        if (distance < radius)
        {
            SceneManager.LoadScene("Dungeon");
        }
    }
}
