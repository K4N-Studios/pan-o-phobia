using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeSkin : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int _skin;

    [SerializeField] private GameObject flashLight;
    [SerializeField] private GameObject Mask;
    [SerializeField] private GameObject Gloves;
    [SerializeField] private GameObject Spray;

    [SerializeField] private GameStateManager _gameState;
    
    void Update () {
        ChangeSkin();
    }

    void ChangeSkin () {

        bool haveFlashlight = CheckItem(flashLight);
        bool haveGloves = CheckItem(Gloves);
        bool haveMask = CheckItem(Mask);
        bool haveSpray = CheckItem(Spray);

        if (haveGloves && haveMask) _skin = 1;
        else if (haveSpray) _skin = 1;
        else if (haveFlashlight) _skin = 3;
        else _skin = 0;

        _animator.SetFloat("ObjectSkin", _skin);
    }

    bool CheckItem (GameObject obj) 
    {
        var flashlight = _gameState.GetCollectable(obj.name);
        return flashlight != null && flashlight.Amount >= 1;
    }
}
