using UnityEngine;
using UnityEditor;
using FMODUnity;

public class FindFMODStudioEventEmitters : EditorWindow
{
    [MenuItem("Tools/Find FMOD Emitters")]
    static void Init()
    {
        var emitters = FindObjectsOfType<StudioEventEmitter>(true);
        Debug.Log($"Found {emitters.Length} FMOD Studio Event Emitters!");

        foreach (var emitter in emitters)
        {
            Debug.Log($"Emitter {emitter.name}", emitter.gameObject);
        }
    }
}
