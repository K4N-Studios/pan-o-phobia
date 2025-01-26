using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CollectableRegister
{
    public string Name;
    public int Amount;
}

public class GameStateManager : MonoBehaviour
{
    public GameObject postProcess;
    private bool _enabledMainLightSwitch = false;

    [SerializeField] public List<CollectableRegister> collectedCollectables = new();

    public bool enabledMainLightSwitch
    {
        get => _enabledMainLightSwitch;

        set
        {
            _enabledMainLightSwitch = value;
            postProcess.SetActive(!value);
        }
    }

    public CollectableRegister GetCollectable(string name)
    {
        foreach (var collectable in collectedCollectables)
        {
            if (collectable.Name == name)
            {
                return collectable;
            }
        }

        return null;
    }

    public void RegisterCollectable(string name)
    {
        var existingElement = collectedCollectables.FirstOrDefault(x => x.Name == name);
        if (existingElement != null)
        {
            existingElement.Amount += 1;
        }
        else
        {
            collectedCollectables.Add(new CollectableRegister
            {
                Name = name,
                Amount = 1,
            });
        }
    }
}
