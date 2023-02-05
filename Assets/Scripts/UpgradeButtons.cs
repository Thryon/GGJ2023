using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public PlayerUpgrader PlayerUpgrader;
    public Button upgradeButton;

    private void Start()
    {
        upgradeButton.onClick.AddListener(UpgradePower);
    }

    private void UpgradePower()
    {
        PlayerUpgrader.UpgradePower(10);
    }
}
