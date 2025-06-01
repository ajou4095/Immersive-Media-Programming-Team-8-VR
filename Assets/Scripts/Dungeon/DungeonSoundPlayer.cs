using UnityEngine;

public class DungeonSoundPlayer : MonoBehaviour
{
    public AudioSource unlockDoorSuccessSound;
    public AudioSource unlockDoorFailureSound;
    
    void Start()
    {
        var audioArray = GetComponents<AudioSource>();
        unlockDoorSuccessSound = audioArray[0];
        unlockDoorFailureSound = audioArray[1];
    }
}
