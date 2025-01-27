using System;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ScopedLightTimerComponent : MonoBehaviour
{
    public GameStateManager gameState;
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _label;

    private void Awake()
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
    }

    private void AttachTo(GameObject element)
    {
        if (element.TryGetComponent(out TimedSwitchInteractableBehavior timedSwitchBehavior))
        {
            Assert.IsTrue(timedSwitchBehavior.TimerIsRunning);

            var secondsLeft = timedSwitchBehavior.ShutdownTimerProgress;
            var roundedTime = math.round(secondsLeft);

            _image.fillAmount = secondsLeft / timedSwitchBehavior.DefaultTimerTime;

            if (roundedTime >= 10)
            {
                _label.SetText(roundedTime + "s");
            }
            else if (roundedTime == 0)
            {
                _label.SetText("");
            }
            else
            {
                _label.SetText(" " + roundedTime + "s");
            }
        }
    }

    private void CheckLights()
    {
        var possibleMatch = gameState.lightsStates.FindAll(x => x.IsOn == true).LastOrDefault();
        _image.enabled = possibleMatch != null;

        // reset text if image has been reset.
        if (_image.enabled == false && _label.text != "")
        {
            _label.SetText("");
        }

        if (possibleMatch != null)
        {
            var gameObjectName = possibleMatch.Name;
            AttachTo(GameObject.Find(gameObjectName));
        }
    }

    private void Update()
    {
        CheckLights();
    }
}
