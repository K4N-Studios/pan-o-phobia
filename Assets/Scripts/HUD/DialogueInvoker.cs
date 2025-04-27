using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueInvoker : Singleton<DialogueInvoker>
{
    private readonly string _dialogueFilename = Path.Combine(Application.streamingAssetsPath, "Dialogues/es.csv");

    public void FireDialogue(string dialogueID)
    {
        Debug.Log($"dialogue file path: {_dialogueFilename}");
    }
}
