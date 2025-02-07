using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;

[CustomEditor(typeof(FMODSoundManager))]
public class SoundManagerMissingReferences : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var soundManager = (FMODSoundManager)target;
        var references = soundManager.EventReferences;
        var missingReferences = new List<string>();

        foreach (var refString in Enum.GetNames(typeof(SoundType)))
        {
            if (Enum.TryParse(refString, out SoundType expectedReference))
            {
                if (!references.Contains(expectedReference))
                {
                    missingReferences.Add(expectedReference.ToString());
                }
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
        }
    }
}