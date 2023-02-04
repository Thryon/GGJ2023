using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int Seeds;
    public TMP_Text seedCounterText;

    public void Start()
    {
        Seeds = 0;
        UpdateSeedCounter();
    }

    public void UpdateSeedCounter()
    {
        seedCounterText.text = "" + Seeds;
    }

    public void AddSeeds(int amount)
    {
        Seeds += amount;
        UpdateSeedCounter();
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnGainSeed, ReferencesSingleton.Instance.player.Inventory.Seeds);
    }

    public void RemoveSeeds(int amount)
    {
        Seeds -= amount;
        UpdateSeedCounter();
        if (Seeds < 0)
            Seeds = 0;
    }
}
