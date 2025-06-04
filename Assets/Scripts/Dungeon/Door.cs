using UnityEngine;

public class Door : MonoBehaviour
{
    public delegate void OnFailure();

    public delegate void OnSuccess();

    [SerializeField]
    private bool _isLocked;
    public bool isLocked
    {
        get => _isLocked;
        set
        {
            _isLocked = value;
            UpdateLockState();
        }
    }
    public string password;
    private DungeonSoundPlayer _dungeonSoundPlayer;

    private HingeJoint hingeJoint;

    private void Start()
    {
        hingeJoint = GetComponent<HingeJoint>();
        _dungeonSoundPlayer = FindFirstObjectByType<DungeonSoundPlayer>();

        UpdateLockState();
    }

    public void TryUnlockDoor(
        string password,
        OnSuccess onSuccess = null,
        OnFailure onFailure = null)
    {
        if (isLocked == false) return;
        
        if (this.password == password)
        {
            _dungeonSoundPlayer.unlockDoorSuccessSound.Play();
            isLocked = false;
            onSuccess?.Invoke();
        }
        else
        {
            _dungeonSoundPlayer.unlockDoorFailureSound.Play();
            onFailure?.Invoke();
        }
    }

    private void UpdateLockState()
    {
        if (isLocked)
            hingeJoint.limits = new JointLimits
            {
                min = 0,
                max = 0
            };
        else
            hingeJoint.limits = new JointLimits
            {
                min = -90,
                max = 90
            };
    }
}