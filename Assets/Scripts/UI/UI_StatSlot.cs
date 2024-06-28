using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private StatType statType;
    [SerializeField] private string statName;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if(statNameText != null)
        {
            statNameText.text = statName;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateStatValueUI();

        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if(playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).getValue().ToString();

            if (statType == StatType.health)
                statValueText.text = playerStats.GetMaxHealthValue().ToString();

            if (statType == StatType.damage)
                statValueText.text = (playerStats.damage.getValue() + playerStats.strength.getValue()).ToString();

            if (statType == StatType.critPower)
                statValueText.text = (playerStats.critPower.getValue() + playerStats.strength.getValue()).ToString();

            if (statType == StatType.critChance)
                statValueText.text = (playerStats.critChance.getValue() + playerStats.agility.getValue()).ToString();

            if (statType == StatType.evasion)
                statValueText.text = (playerStats.evasion.getValue() + playerStats.agility.getValue()).ToString();

            if (statType == StatType.magicResistance)
                statValueText.text = (playerStats.magicResistance.getValue() + playerStats.intelligence.getValue()).ToString();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltip.HideStatTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTooltip.ShowStatTooltip(statDescription);
    }
}
