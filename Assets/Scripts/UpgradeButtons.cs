using System;
using KinematicCharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public PlayerUpgrader.UpgradeType Type;
    public TMP_Text PriceText;
    public TMP_Text LevelText;
    public Button button;

    public delegate void UpgradeEvent(PlayerUpgrader.UpgradeType Type);

    public event UpgradeEvent OnUpgrade;
    
    private Player Player => ReferencesSingleton.Instance.player;

    public void Refresh()
    {
        int currentLevel = Player.PlayerUpgrader.GetLevel(Type);
        int maxLevel = Player.PlayerUpgrader.GetMaxLevel(Type);
        bool isMaxLevel = currentLevel >= maxLevel;
        if (isMaxLevel)
        {
            button.interactable = false;
            PriceText.gameObject.SetActive(false);
        }
        else
        {
            int price = Player.PlayerUpgrader.GetNextLevelPrice(Type);
            PriceText.gameObject.SetActive(true);
            PriceText.text = price.ToString();
            bool affordable = Player.Inventory.Seeds >= price;
            PriceText.color = affordable ? Color.white : Color.red;
            button.interactable = affordable;
        }

        LevelText.text = $"Lv. {currentLevel}/{isMaxLevel}";
    }

    public void Upgrade()
    {
        OnUpgrade?.Invoke(Type);
        Refresh();
    }
}
