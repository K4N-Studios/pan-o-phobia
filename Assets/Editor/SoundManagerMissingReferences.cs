using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FMODSoundManager))]
public class SoundManagerMissingReferences : Editor
{
    private void DetectMissingReferences(FMODSoundManager soundManager)
    {
        var references = soundManager.EventReferences;
        var missingReferences = new List<string>();

        foreach (SoundType expectedReference in Enum.GetValues(typeof(SoundType)))
        {
            if (!references.Contains(expectedReference))
            {
                missingReferences.Add(expectedReference.ToString());
            }
        }

        if (missingReferences.Count > 0)
        {
            StringBuilder message = new("You're missing the next references:\n");

            foreach (var element in missingReferences)
            {
                message.Append($"- {element}\n");
            }

            EditorGUILayout.HelpBox(message.ToString(), MessageType.Warning);
            return;
        }

        EditorGUILayout.HelpBox("All sounds have their corresponding fmod sound reference", MessageType.Info);
    }

    private void DetectMissingSoundImplementations(FMODSoundManager soundManager)
    {
        List<SoundType> missingTypes = new();

        foreach (SoundType soundType in Enum.GetValues(typeof(SoundType)))
        {
            try
            {
                soundManager.GetSoundImpl(soundType);
            }
            catch (AudioImplementationUnavailableException)
            {
                missingTypes.Add(soundType);
            }
            catch (Exception err)
            {
                Debug.LogWarning($"Error checking {soundType}: {err}");
            }
        }

        if (missingTypes.Count > 0)
        {
            StringBuilder msg = new("You're missing sound behaviors for the next sounds:\n");

            foreach (var type in missingTypes)
            {
                msg.AppendLine($"- {type}");
            }

            EditorGUILayout.HelpBox(msg.ToString(), MessageType.Error);
            return;
        }

        EditorGUILayout.HelpBox("All sounds have their corresponding sound behaviors", MessageType.Info);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DetectMissingReferences((FMODSoundManager)target);
        DetectMissingSoundImplementations((FMODSoundManager)target);
    }
}