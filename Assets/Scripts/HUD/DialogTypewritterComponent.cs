using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class DialogTypewritterComponent : MonoBehaviour
{
    public TMP_Text label;
    public float maxTransparencyLevel = 0.85f;
    public float minTransparencyLevel = 0f;
    public float fadeDuration = 0.25f;
    public float typewritterDelay = 0.05f;
    public GameStateManager gameState;

    [SerializeField] private UnityEngine.UI.Image _boxImage;
    [SerializeField] private GameObject _chevron;

    [SerializeField] private List<string> _ongoingTexts = new();
    [SerializeField] private int _currentTextQueueIndex = 0;

    private bool _isRunning = false;

    public bool CanStartSequence => !_isRunning;

    public bool Active
    {
        get => IsActive();
    }

    private bool IsActive()
    {
        return _boxImage.color.a == maxTransparencyLevel;
    }

    public void EnqueueText(string text)
    {
        _ongoingTexts.Add(text);
    }

    private void SetImageAlpha(float alpha)
    {
        if (_boxImage)
        {
            Color color = _boxImage.color;
            color.a = alpha;
            _boxImage.color = color;
        }
    }

    private IEnumerator ResetMetadata()
    {
        _currentTextQueueIndex = 0;
        _ongoingTexts.Clear();
        _isRunning = false;
        Assert.IsTrue(_ongoingTexts.Count == 0);
        if (gameState.blockControlsRequest) gameState.UnblockControls();
        yield return null;
    }

    private IEnumerator FadeImageIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            SetImageAlpha(Mathf.Lerp(minTransparencyLevel, maxTransparencyLevel, elapsedTime / fadeDuration));
            yield return null;
        }
        SetImageAlpha(maxTransparencyLevel);
    }

    private IEnumerator FadeImageOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            SetImageAlpha(Mathf.Lerp(maxTransparencyLevel, minTransparencyLevel, elapsedTime / fadeDuration));
            yield return null;
        }
        SetImageAlpha(minTransparencyLevel);
    }

    private IEnumerator NextDialogue()
    {
        _chevron.SetActive(false);
        label.SetText("");

        var currentText = _ongoingTexts[_currentTextQueueIndex];
        var currentCharIndex = 0;

        while (currentCharIndex < currentText.Length)
        {
            var currentChar = currentText[currentCharIndex++];
            label.SetText(label.text + currentChar);
            yield return new WaitForSeconds(typewritterDelay);
        }

        _chevron.SetActive(true);
    }

    private IEnumerator TypewritterBackwards()
    {
        var currentText = _ongoingTexts[_currentTextQueueIndex];
        var currentCharIndex = currentText.Length - 1;

        while (currentCharIndex >= 0)
        {
            var newText = currentText[0..currentCharIndex--];
            label.SetText(newText);
            yield return new WaitForSeconds(typewritterDelay);
        }
    }

    private IEnumerator TypewritterSequence()
    {
        yield return StartCoroutine(FadeImageIn());
        yield return StartCoroutine(NextDialogue());
    }

    public void StartSequence()
    {
        if (_ongoingTexts.Count == 0)
        {
            return;
        }

        _currentTextQueueIndex = 0;
        _isRunning = true;

        StartCoroutine(TypewritterSequence());
    }

    private IEnumerator HidePanel()
    {
        _chevron.SetActive(false);
        yield return StartCoroutine(TypewritterBackwards());
        yield return StartCoroutine(FadeImageOut());
        yield return StartCoroutine(ResetMetadata());
    }

    private void CheckForNextPage()
    {
        var waitingForNext = _chevron.activeSelf;
        var isLastPage = _currentTextQueueIndex == _ongoingTexts.Count - 1;

        if (waitingForNext && !isLastPage && Input.GetKeyDown(KeyCode.Return))
        {
            _currentTextQueueIndex++;
            StartCoroutine(NextDialogue());
            return;
        }

        if (waitingForNext && isLastPage && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(HidePanel());
        }
    }

    private void Update()
    {
        if (_isRunning)
        {
            CheckForNextPage();

            if (gameState.blockControlsRequest == false)
            {
                gameState.RequestControlsBlock();
            }
        }
    }
}
