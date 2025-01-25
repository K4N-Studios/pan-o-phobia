using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    [SerializeField] private float _timer = 0;
    [SerializeField] private float _retarder = 1;
    [SerializeField] private float _multiplier = 1;
    [SerializeField] private bool _isActive = false;
    [SerializeField] private TextMeshProUGUI _tmp;
    private double _alpha;
    
    void Start () 
    {
    }
    void FixedUpdate()
    {
        if (_isActive)
        {
            _timer = _timer + 0.01f;

            _alpha = Math.Sin((_multiplier * _timer)/_retarder) ; 
            Color newColor = _tmp.color;
            newColor.a = (float) _alpha;
            _tmp.color = newColor;

        } else
        {
            return;
        }
    }
}
