using System.Collections.Generic;
using UnityEngine;

public class DoorInteractableBehavior : MonoBehaviour
{
    public GameStateManager gameState;
    [SerializeField] private List<CollectableRegister> _openRequirements = new();

    private bool IsLocked()
    {
        var foundRequirements = 0;
        foreach (var requirement in _openRequirements)
        {
            foreach (var collectable in gameState.collectedCollectables)
            {
                if (requirement.Name == collectable.Name && collectable.Amount >= requirement.Amount)
                {
                    foundRequirements++;
                }
            }
        }

        return foundRequirements != _openRequirements.Count;
    }

    public void OpenDoor()
    {
        Debug.Log("door is locked? " + (IsLocked() ? "true" : "false"));
    }
}
