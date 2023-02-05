using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrader : MonoBehaviour
{
    public int SeedCount
    { 
        get => ReferencesSingleton.Instance.player.Inventory.Seeds;
        set => ReferencesSingleton.Instance.player.Inventory.Seeds = value;
    }

    public int GunPower;
    public int GunWidth;

    public void UpgradePower(int upgradeCost)
    {
        GunPower += 10;
        ReferencesSingleton.Instance.player.Inventory.RemoveSeeds(upgradeCost);
    }

    public void UpgradeCleave(int upgradeCost)
    {
        GunWidth += 5;
        ReferencesSingleton.Instance.player.Inventory.RemoveSeeds(upgradeCost);
    }
}
