using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDict<K, V>
{
    [System.Serializable]
    private class Register
    {
        public K Key;
        public V Value;
    }

    [SerializeField] private List<Register> _dict = new();

    public bool TryGet(K key, out V value)
    {
        foreach (var element in _dict)
        {
            if (element.Key.Equals(key))
            {
                value = element.Value;
                return true;
            }
        }

        value = default(V);
        return false;
    }

    private bool Contains(K key)
    {
        return TryGet(key, out _);
    }

    public void Update(K key, V value)
    {
        foreach (var element in _dict)
        {
            if (element.Key.Equals(key))
            {
                element.Value = value;
            }
        }
    }

    private void Append(K key, V value)
    {
        _dict.Add(new()
        {
            Key = key,
            Value = value,
        });
    }

    public void Add(K key, V value)
    {
        if (Contains(key))
        {
            Update(key, value);
        }
        else
        {
            Append(key, value);
        }
    }
}