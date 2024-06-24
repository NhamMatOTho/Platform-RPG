using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    public ItemEffect[] itemEffects;

    [Header("Major Stat")]
    public int strength; // increase damage and % crit power
    public int vitality; //increase health
    public int intelligence; // increase magic damage and magic resistance
    public int agility; // increase evasion and % crit chance

    [Header("Offensive Stat")]
    public int damage;
    public int critChance;
    public int critPower;

    [Header("Defensive Stat")]
    public int maxHealth;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic Stat")]
    public int iceDamage;
    public int fireDamage;
    public int lightningDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftMaterials;

    public void ExecuteItemEffect()
    {
        foreach(var item in itemEffects)
        {
            item.ExecuteEffect();
        }
    }

    public void AddModifiers()
    {
        PlayerStats playerStat = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStat.strength.AddModifier(strength);
        playerStat.vitality.AddModifier(vitality);
        playerStat.intelligence.AddModifier(intelligence);
        playerStat.agility.AddModifier(agility);

        playerStat.damage.AddModifier(damage);
        playerStat.critChance.AddModifier(critChance);
        playerStat.critPower.AddModifier(critPower);

        playerStat.maxHealth.AddModifier(maxHealth);
        playerStat.armor.AddModifier(armor);
        playerStat.evasion.AddModifier(evasion);
        playerStat.magicResistance.AddModifier(magicResistance);

        playerStat.iceDamage.AddModifier(iceDamage);
        playerStat.fireDamage.AddModifier(fireDamage);
        playerStat.lightningDamage.AddModifier(lightningDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStat = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStat.strength.RemoveModifier(strength);
        playerStat.vitality.RemoveModifier(vitality);
        playerStat.intelligence.RemoveModifier(intelligence);
        playerStat.agility.RemoveModifier(agility);

        playerStat.damage.RemoveModifier(damage);
        playerStat.critChance.RemoveModifier(critChance);
        playerStat.critPower.RemoveModifier(critPower);

        playerStat.maxHealth.RemoveModifier(maxHealth);
        playerStat.armor.RemoveModifier(armor);
        playerStat.evasion.RemoveModifier(evasion);
        playerStat.magicResistance.RemoveModifier(magicResistance);

        playerStat.iceDamage.RemoveModifier(iceDamage);
        playerStat.fireDamage.RemoveModifier(fireDamage);
        playerStat.lightningDamage.RemoveModifier(lightningDamage);
    }
}
