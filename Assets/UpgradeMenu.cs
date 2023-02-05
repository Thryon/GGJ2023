using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private GameObject container;
    // Start is called before the first frame update
    void Start()
    {
        container.SetActive(false);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OpenUpgradeMenu, Open);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.CloseUpgradeMenu, Close);
    }

    private void Close()
    {
        container.SetActive(false);
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnUpgradeMenuClosed);
    }

    private void Open()
    {
        container.SetActive(true);
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnUpgradeMenuOpened);
    }
}
