using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    private Player player;

    private List<Image> healthPoints;    
    [SerializeField] private Image healthPoint1;
    [SerializeField] private Image healthPoint2;
    [SerializeField] private Image healthPoint3;

    private int hitTaken = 0;


    private void Start()
    {
        EventManager.hittingOnPlayer += UpdateHealth;
        EventManager.settingPlayerHealth += SetHealth;

        player = Locator.player;

        healthPoints = new List<Image>() { healthPoint1, healthPoint2, healthPoint3 };
        foreach (Image healthpoint in healthPoints) {
            healthpoint.enabled = true;
        }
    }

    private void UpdateHealth()
    {
        hitTaken++;
        if (hitTaken <= healthPoints.Count) {
            healthPoints[healthPoints.Count - hitTaken].enabled = false;
        }
    }

    public void SetHealth()
    {
        foreach (Image healthpoint in healthPoints) {
            healthpoint.enabled = false;
        }
        for (int i = 0; i < player.health; i++) {
            healthPoints[i].enabled = true;
        }
        hitTaken = player.maxHealth - player.health;
    }

    private void OnDestroy()
    {
        EventManager.hittingOnPlayer -= UpdateHealth;
        EventManager.settingPlayerHealth += SetHealth;
    }
}

