using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUpgrader : MonoBehaviour
{
    public int SeedCount
    { 
        get => ReferencesSingleton.Instance.player.Inventory.Seeds;
        set => ReferencesSingleton.Instance.player.Inventory.Seeds = value;
    }

    public enum UpgradeType
    {
        Power,
        Capacity,
        FireRate
    }

    [Header("Power")]
    public bool levelUpPower;
    public bool levelDownPower;
    public int GunPowerLevel;
    public int MaxGunPowerLevel;
    public int MinGunPowerPrice;
    public int MaxGunPowerPrice;
    public float MinGunPowerStartSpeedValue;
    public float MaxGunPowerStartSpeedValue;
    public float MinGunPowerCollisionPowerValue;
    public float MaxGunPowerCollisionPowerValue;
    [Space]
    [Header("Cleave")]
    public bool levelUpWidth;
    public bool levelDownWidth;
    public int GunWidthLevel;
    public int MaxGunWidthLevel;
    public int MinGunWidthPrice;
    public int MaxGunWidthPrice;
    public float MinGunWidthRealValue;
    public float MaxGunWidthRealValue;
    [Space]
    [Header("Capacity")]
    public bool levelUpCapacity;
    public bool levelDownCapacity;
    public int GunCapacityLevel;
    public int MaxGunCapacityLevel;
    public int MinGunCapacityPrice;
    public int MaxGunCapacityPrice;
    public int MinGunCapacityRealValue;
    public int MaxGunCapacityRealValue;
    [Space]
    [Header("FireRate")]
    public bool levelUpFireRate;
    public bool levelDownFireRate;
    public int GunFireRateLevel;
    public int MaxGunFireRateLevel;
    public int MinGunFireRatePrice;
    public int MaxGunFireRatePrice;
    public int MinGunFireRateRealValue;
    public int MaxGunFireRateRealValue;
    
    public ParticleSystem gunParticleSystem;

    private IEnumerator Start()
    {
        yield return null;
        RefreshGunPower();
        RefreshGunCleave();
        RefreshGunCapacity();
        RefreshGunFireRate();
    }

    private void Update()
    {
        if (levelUpPower)
        {
            UpgradePower();
        }

        if (levelDownPower)
        {
            GunPowerLevel--;
            levelDownPower = false;
        }

        if (levelUpWidth)
        {
            UpgradeCleave(1);
            levelUpWidth = false;
        }

        if (levelDownPower)
        {
            GunWidthLevel--;
            levelDownPower = false;
        }
        
        if (levelUpCapacity)
        {
            UpgradeCapacity();
            levelUpCapacity = false;
        }

        if (levelDownCapacity)
        {
            GunCapacityLevel--;
            levelDownCapacity = false;
        }
        
        if (levelUpFireRate)
        {
            UpgradeFireRate();
            levelUpFireRate = false;
        }

        if (levelDownFireRate)
        {
            GunFireRateLevel--;
            levelDownFireRate = false;
        }
        
    }

    public int GetNextLevelPrice(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Power:
                return GetNextPowerLevelPrice();
            case UpgradeType.Capacity:
                return GetNextCapacityLevelPrice();
            case UpgradeType.FireRate:
                return GetNextFireRateLevelPrice();
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void Upgrade(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Power:
                UpgradePower();
                break;
            case UpgradeType.Capacity:
                UpgradeCapacity();
                break;
            case UpgradeType.FireRate:
                UpgradeFireRate();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public int GetNextPowerLevelPrice()
    {
        float t = (GunPowerLevel / (float)MaxGunPowerLevel);
        return (int)Mathf.Lerp(MinGunPowerPrice, MaxGunPowerPrice, t);
    }
    
    public int GetNextWidthLevelPrice()
    {
        float t = (GunWidthLevel / (float)MaxGunWidthLevel);
        return (int)Mathf.Lerp(MinGunWidthPrice, MaxGunWidthPrice, t);
    }
    
    public int GetNextCapacityLevelPrice()
    {
        float t = (GunCapacityLevel / (float)MaxGunCapacityLevel);
        return (int)Mathf.Lerp(MinGunCapacityPrice, MaxGunCapacityPrice, t);
    }
    
    public int GetNextFireRateLevelPrice()
    {
        float t = (GunFireRateLevel / (float)MaxGunFireRateLevel);
        return (int)Mathf.Lerp(MinGunFireRatePrice, MaxGunFireRatePrice, t);
    }

    public void UpgradePower()
    {
        GunPowerLevel++;
        ReferencesSingleton.Instance.player.Inventory.RemoveSeeds(GetNextPowerLevelPrice());
        RefreshGunPower();
    }

    public void UpgradeCleave(int upgradeCost)
    {
        GunWidthLevel++;
        ReferencesSingleton.Instance.player.Inventory.RemoveSeeds(upgradeCost);
        RefreshGunCleave();
    }
    
    public void UpgradeCapacity()
    {
        GunCapacityLevel++;
        ReferencesSingleton.Instance.player.Inventory.RemoveSeeds(GetNextCapacityLevelPrice());
        RefreshGunCapacity();
    }
    
    public void UpgradeFireRate()
    {
        GunFireRateLevel++;
        ReferencesSingleton.Instance.player.Inventory.RemoveSeeds(GetNextFireRateLevelPrice());
        RefreshGunFireRate();
    }

    void RefreshGunPower()
    {
        var maintruc = gunParticleSystem.main;
        float t = (GunPowerLevel / (float)MaxGunPowerLevel);
        maintruc.startSpeed = Mathf.Lerp(MinGunPowerStartSpeedValue, MaxGunPowerStartSpeedValue, t);
        var collisionModule = gunParticleSystem.collision;
        collisionModule.colliderForce = Mathf.Lerp(MinGunPowerCollisionPowerValue, MaxGunPowerCollisionPowerValue, t);
    }

    void RefreshGunCleave()
    {
        var shape = gunParticleSystem.shape;
        float t = (GunWidthLevel / (float)MaxGunWidthLevel);
        shape.radius = Mathf.Lerp(MinGunWidthRealValue, MaxGunWidthRealValue, t);
    }

    void RefreshGunCapacity()
    {
        float t = (GunCapacityLevel / (float)MaxGunCapacityLevel);
        int maxAmount = (int)Mathf.Lerp(MinGunCapacityRealValue, MaxGunCapacityRealValue, t);;
        ReferencesSingleton.Instance.player.WaterReservoir.SetMaxAmount(maxAmount);
    }

    void RefreshGunFireRate()
    {
        float t = (GunFireRateLevel / (float)MaxGunFireRateLevel);
        float fireRate = Mathf.Lerp(MinGunFireRateRealValue, MaxGunFireRateRealValue, t);;
        ReferencesSingleton.Instance.player.PlayerGun.SetFireRate(fireRate);
    }

    public int GetLevel(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Power:
                return GunPowerLevel;
            case UpgradeType.Capacity:
                return GunCapacityLevel;
            case UpgradeType.FireRate:
                return GunFireRateLevel;
        }
        return -1;
    }

    public int GetMaxLevel(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Power:
                return MaxGunPowerLevel;
            case UpgradeType.Capacity:
                return MaxGunCapacityLevel;
            case UpgradeType.FireRate:
                return MaxGunFireRateLevel;
        }
        return -1;
    }
}
