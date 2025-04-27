using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

class Dialogue
{
    public string Id { get; private set; }
    public List<string> Slides { get; private set; }

    public Dialogue(string id, List<string> slides)
    {
        Id = id;
        Slides = slides;
    }
}

public class DialogueInvoker : Singleton<DialogueInvoker>
{
    private List<Dialogue> _dialogues = null;
    private readonly string _dialoguesFilepath = Path.Combine(Application.streamingAssetsPath, "Dialogues/es.csv");

    public string ReadFileContents(string filename)
    {
        string fileContent;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        // in platforms where direct file access is usually allowed, we can just call `File.ReadAllText`
        fileContent = File.ReadAllText(filename);
#else
        // for mobile platforms or others where direct access is not usually allowed, we will have to do a web request to the file ;-; ;-; ;-;
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filename))
        {
            www.SendWebRequest();
            while (!www.IsDone())
            {
                // Avoid blocking the main thread.
                System.Threading.Thread.Sleep(10);
            }

            if (www.result === UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                fileContent = www.downloadHandler.text;
            }
            else
            {
                throw new FileNotFoundException($"Unable to fetch content from {filename}: {www.error}");
            }
        }
#endif

        return fileContent;
    }

    private void LoadDialogues()
    {
        string fileContents;
        _dialogues = new();

        try
        {
            fileContents = ReadFileContents(_dialoguesFilepath);
        }
        catch (FileNotFoundException)
        {
            Debug.LogError($"Dialogue file not found at: {_dialoguesFilepath}");
            return;
        }
        catch (IOException)
        {
            Debug.LogError($"Unable to read dialogues file at {_dialoguesFilepath}");
            return;
        }

        // start processing the dialogues in the file and build the list.
        string[] lines = fileContents.Split("\n");
        string currentSection = null;

        foreach (string rawLine in lines)
        {
            var line = rawLine.Trim();

            if (currentSection == null)
            {
                currentSection = line;
                continue;
            }

            if (line == "end" && currentSection != null)
            {
                currentSection = null;
                continue;
            }

            string currentDialogue = line;

            if (currentDialogue.StartsWith("\"") && currentDialogue.EndsWith("\""))
            {
                currentDialogue = currentDialogue[1..^1];
            }

            Dialogue existingDialogue = _dialogues.FirstOrDefault(d => d.Id == currentSection);

            if (existingDialogue == null)
            {
                _dialogues.Add(new(currentSection, new List<string>{currentDialogue}));
            }
            else
            {
                existingDialogue.Slides.Add(currentDialogue);
            }
        }
    }

    public void FireDialogue(string dialogueID)
    {
        if (_dialogues == null)
        {
            LoadDialogues();
        }

        var dialogBox = GameObject.Find("DialogBox");
        var typewritter = dialogBox.GetComponent<DialogTypewritterComponent>();

        if (typewritter.CanStartSequence)
        {
            foreach (var dialogue in _dialogues)
            {
                if (dialogue.Id != dialogueID)
                {
                    continue;
                }

                foreach (var slide in dialogue.Slides)
                {
                    Debug.Log($"enqueuing text: {slide}");
                    typewritter.EnqueueText(slide);
                }

                break;  // break since we've already processed the needed dialogueID.
            }

            typewritter.StartSequence();
        }
    }
}
