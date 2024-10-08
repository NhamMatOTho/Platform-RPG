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

    [Header("Unique effect")]
    public float itemCooldown;
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

    private int descriptionLength;

    public void Effect(Transform _enemyPosition)
    {
        foreach(var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
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

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "Strength");
        AddItemDescription(vitality, "Vitality");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(agility, "Agility");

        AddItemDescription(damage, "damage");
        AddItemDescription(critChance, "critChance");
        AddItemDescription(critPower, "critPower");

        AddItemDescription(maxHealth, "HP");
        AddItemDescription(armor, "armor");
        AddItemDescription(evasion, "evasion");
        AddItemDescription(magicResistance, "magicRes");

        AddItemDescription(iceDamage, "iceDmg");
        AddItemDescription(fireDamage, "FireDmg");
        AddItemDescription(lightningDamage, "lightningDmg");

        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].effectDescription.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Unique: " + itemEffects[i].effectDescription);
                descriptionLength++;
            }
        }

        if(descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if(_value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (_value > 0)
                sb.Append("+ " + _value + " " + _name);

            descriptionLength++;
        }
    }
}
