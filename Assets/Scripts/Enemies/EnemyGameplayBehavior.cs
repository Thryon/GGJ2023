using Pathfinding;
using UnityEngine;

public class EnemyGameplayBehavior : MonoBehaviour
{
    AIDestinationSetter destinationSetter;
    void Start()
    {
        destinationSetter = GetComponentInChildren<AIDestinationSetter>();
        if (ReferencesSingleton.Instance != null)
            destinationSetter.target = ReferencesSingleton.Instance.treeRef.transform;
    }

}
