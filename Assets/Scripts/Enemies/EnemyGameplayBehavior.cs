using Pathfinding;
using UnityEngine;

public class EnemyGameplayBehavior : MonoBehaviour
{
    AIDestinationSetter destinationSetter;
    void Start()
    {
        destinationSetter = GetComponentInChildren<AIDestinationSetter>();
        Debug.Log(destinationSetter);
        Debug.Log(ReferencesSingleton.Instance);

        if (ReferencesSingleton.Instance != null)
            destinationSetter.target = ReferencesSingleton.Instance.treeRef.transform;
    }

}
