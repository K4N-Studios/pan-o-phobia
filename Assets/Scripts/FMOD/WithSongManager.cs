using UnityEngine;

/// <summary>
/// Just a shortcut for MonoBehavior scripts which wants to access
/// the _songManager easily without having to write the Awake
/// </summary>
public class WithSongManager : MonoBehaviour
{
    protected FMODSoundManager _soundManager;

    private void Awake()
    {
        _soundManager = FMODSoundManager.Instance;
    }
}
