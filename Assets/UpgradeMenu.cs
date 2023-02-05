using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private TMP_Text UpgradeText;
    // Start is called before the first frame update
    void Start()
    {
        container.SetActive(false);
        UpgradeText.gameObject.SetActive(true);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OpenUpgradeMenu, Open);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.CloseUpgradeMenu, Close);
    }

    private void Close()
    {
        container.SetActive(false);
        UpgradeText.gameObject.SetActive(true);
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnUpgradeMenuClosed);
    }

    private void Open()
    {
        container.SetActive(true);
        UpgradeText.gameObject.SetActive(false);
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnUpgradeMenuOpened);

    }

    public void OnUpgradePowerClicked()
    {
        ReferencesSingleton.Instance.player.PlayerUpgrader.UpgradePower();
    }
}
