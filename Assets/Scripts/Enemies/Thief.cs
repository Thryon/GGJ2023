using Pathfinding;
using UnityEngine;

public class Thief : MonoBehaviour
{
    private AIDestinationSetter destinationSetter;
    AIPath aiPath;
    EnemyGameplayBehavior behavior;

    float originSpeed = 3;
    public float speedChasePlayer = 8;
    public float attackIntervalChasePlayer = 1;

    float originAttackInterval;

    void Start()
    {
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnGainWater, CheckTrackPlayer);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnLoseWater, CheckTrackPlayer);

        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        originSpeed = aiPath.maxSpeed;

        behavior = GetComponent<EnemyGameplayBehavior>();
        originAttackInterval = behavior.AttackInterval;
    }

    void CheckTrackPlayer(int _amount)
    {
        if (ReferencesSingleton.Instance.player.WaterReservoir.Amount == 0)
        {
            destinationSetter.target = ReferencesSingleton.Instance.treeRef.transform;
            aiPath.maxSpeed = originSpeed;
            aiPath.endReachedDistance = 1;
            behavior.AttackInterval = originAttackInterval;
        }
        else
        {
            destinationSetter.target = ReferencesSingleton.Instance.player.Character.transform;
            aiPath.maxSpeed = speedChasePlayer;
            aiPath.endReachedDistance = 1.5f;
            behavior.AttackInterval = attackIntervalChasePlayer;
        }

    }
}
