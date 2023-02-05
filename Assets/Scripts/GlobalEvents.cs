using System;
using JetBrains.Annotations;
using UnityEngine;

public enum GlobalEventEnum
{
    OnGainWater,
    OnSprinkleTree,
    OnTreeLevelUp,
    OnLoseWater,
    OnShoot,
    OnInteractWithStand,
    OnEnterInteractWithWaterZone,
    OnExitInteractWithWaterZone,
    OnInteractWithWaterPond,
    OnDeath,
    OnWaterSourceEmpty,
    OnWaterSourceRefilled,
    OnGainSeed,
    SmallCameraShake,
    OnGemHit,
    OnGemDeath,
    BigCameraShake,
    OpenUpgradeMenu,
    CloseUpgradeMenu,
    OnUpgradeMenuOpened,
    OnUpgradeMenuClosed,
    Size
}

public class GlobalEvents : MonoBehaviour
{
    Action[] NoParamEvents = new Action[(int)GlobalEventEnum.Size];

    public delegate void EventBool(bool _param);
    public delegate void EventInt(int _param);
    public delegate void EventFloat(float _param);
    public delegate void EventString(string _param);
    public delegate void EventComponent(Component _param);
    public delegate void EventParticle(Component _param, ParticleSystem.Particle particle, Vector3 position, Vector3 direction);

    EventBool[] BoolEvents = new EventBool[(int)GlobalEventEnum.Size];
    EventInt[] IntEvents = new EventInt[(int)GlobalEventEnum.Size];
    EventFloat[] FloatEvents = new EventFloat[(int)GlobalEventEnum.Size];
    EventString[] StringEvents = new EventString[(int)GlobalEventEnum.Size];

    public event EventParticle OnParticleCollisionEnter;
    public void DispatchOnParticleCollisionEvent(Component collider, ParticleSystem.Particle particle, Vector3 position, Vector3 direction)
    {
        OnParticleCollisionEnter?.Invoke(collider, particle, position, direction);
    }

    public static GlobalEvents Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType<GlobalEvents>();
            return instance;
        }
    }

    static GlobalEvents instance = null;
    void Start()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;

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
    
    public void RegisterEvent(GlobalEventEnum _event, EventFloat _action)
    {
        FloatEvents[(int)_event] += _action;
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

    public void SendEvent(GlobalEventEnum _event, float _param)
    {
        if (FloatEvents[(int)_event] != null)
            FloatEvents[(int)_event](_param);
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

    public void UnregisterEvent(GlobalEventEnum _event, EventFloat _action)
    {
        FloatEvents[(int)_event] -= _action;
    }

    public void UnregisterEvent(GlobalEventEnum _event, EventString _action)
    {
        StringEvents[(int)_event] -= _action;
    }
}
