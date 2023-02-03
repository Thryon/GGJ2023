using System;
using UnityEngine;

public enum GlobalEventEnum
{
    OnGainWater,
    OnSprinkleTree,
    OnTreeLevelUp,
    OnLoseWater,
    OnShoot,
    OnInteractWithStand,
    OnInteractWithWaterPond,
    OnDeath,
    Size
}

public class GlobalEvents : MonoBehaviour
{
    Action[] NoParamEvents = new Action[(int)GlobalEventEnum.Size];

    public delegate void EventBool(bool _param);
    public delegate void EventInt(int _param);
    public delegate void EventString(string _param);

    EventBool[] BoolEvents = new EventBool[(int)GlobalEventEnum.Size];
    EventInt[] IntEvents = new EventInt[(int)GlobalEventEnum.Size];
    EventString[] StringEvents = new EventString[(int)GlobalEventEnum.Size];

    public static GlobalEvents Instance = null;

    void Start()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;

    }

    public void RegisterEvent(GlobalEventEnum _event, Action _action)
    {
        NoParamEvents[(int)_event] += _action;
    }

    public void RegisterEventBool(GlobalEventEnum _event, EventBool _action)
    {
        BoolEvents[(int)_event] += _action;
    }

    //public void RegisterEvent(GlobalEventEnum _event, Action _action)
    //{
    //    NoParamEvents[(int)_event] += _action;
    //}

    //public void RegisterEvent(GlobalEventEnum _event, Action _action)
    //{
    //    NoParamEvents[(int)_event] += _action;
    //}

    public void SendEvent(GlobalEventEnum _event)
    {
        if (NoParamEvents[(int)_event] != null)
            NoParamEvents[(int)_event]();
    }

    public void UnregisterEvent(GlobalEventEnum _event, Action _action)
    {
        NoParamEvents[(int)_event] -= _action;
    }

}
