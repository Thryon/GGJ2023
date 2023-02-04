using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int Seeds;

    public void AddSeeds(int amount)
    {
        Seeds += amount;
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnGainSeed, ReferencesSingleton.Instance.player.Inventory.Seeds);
    }

    public void RemoveSeeds(int amount)
    {
        Seeds -= amount;
        if (Seeds < 0)
            Seeds = 0;
    }
}
