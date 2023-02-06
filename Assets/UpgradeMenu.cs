using TMPro;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private TMP_Text UpgradeText;
    [SerializeField] private UpgradeButton[] upgradeButtons;

    void Start()
    {
        container.SetActive(false);
        UpgradeText.gameObject.SetActive(true);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OpenUpgradeMenu, Open);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.CloseUpgradeMenu, Close);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnGainSeed, OnGainSeed);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnLoseSeed, OnLoseSeed);

        foreach (var upgradeButton in upgradeButtons)
        {
            upgradeButton.OnUpgrade -= OnButtonUpgradeClicked;
            upgradeButton.OnUpgrade += OnButtonUpgradeClicked;
        }
    }

    private void OnButtonUpgradeClicked(PlayerUpgrader.UpgradeType type)
    {
        ReferencesSingleton.Instance.player.PlayerUpgrader.Upgrade(type);
        RefreshButtons();
    }

    private bool isOpen = false;
    private void OnLoseSeed(int amount)
    {
        if(isOpen)
            RefreshButtons();
    }

    private void OnGainSeed(int amount)
    {
        if(isOpen)
            RefreshButtons();
    }

    void RefreshButtons()
    {
        foreach (var upgradeButton in upgradeButtons)
        {
            upgradeButton.Refresh();
        }
    }

    private void Close()
    {
        isOpen = false;
        container.SetActive(false);
        UpgradeText.gameObject.SetActive(true);
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnUpgradeMenuClosed);
        RefreshButtons();
    }

    private void Open()
    {
        isOpen = true;
        container.SetActive(true);
        UpgradeText.gameObject.SetActive(false);
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnUpgradeMenuOpened);
        RefreshButtons();
    }
}
