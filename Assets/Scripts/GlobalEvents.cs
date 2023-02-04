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

    public void RegisterEvent(GlobalEventEnum _event, EventBool _action)
    {
        BoolEvents[(int)_event] += _action;
    }

    public void RegisterEvent(GlobalEventEnum _event, EventInt _action)
    {
        IntEvents[(int)_event] += _action;
    }

    public void RegisterEvent(GlobalEventEnum _event, EventString _action)
    {
        StringEvents[(int)_event] += _action;
    }


    public void SendEvent(GlobalEventEnum _event)
    {
        if (NoParamEvents[(int)_event] != null)
            NoParamEvents[(int)_event]();
    }

    public void SendEvent(GlobalEventEnum _event, bool _param)
    {
        if (BoolEvents[(int)_event] != null)
            BoolEvents[(int)_event](_param);
    }

    public void SendEvent(GlobalEventEnum _event, int _param)
    {
        if (IntEvents[(int)_event] != null)
            IntEvents[(int)_event](_param);
    }

    public void SendEvent(GlobalEventEnum _event, string _param)
    {
        if (StringEvents[(int)_event] != null)
            StringEvents[(int)_event](_param);
    }

    public void UnregisterEvent(GlobalEventEnum _event, Action _action)
    {
        NoParamEvents[(int)_event] -= _action;
    }

    public void UnregisterEvent(GlobalEventEnum _event, EventBool _action)
    {
        BoolEvents[(int)_event] -= _action;
    }

    public void UnregisterEvent(GlobalEventEnum _event, EventInt _action)
    {
        IntEvents[(int)_event] -= _action;
    }

    public void UnregisterEvent(GlobalEventEnum _event, EventString _action)
    {
        StringEvents[(int)_event] -= _action;
    }
}