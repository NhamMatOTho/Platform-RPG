using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strengh; // increase damage and % crit power
    public Stat vitality; //increase health
    public Stat intelligence; // increase magic damage and magic resistance
    public Stat agility; // increase evasion and % crit chance

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;

    public Stat damage;
    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth.getValue();
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.getValue() + strengh.getValue();
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }


    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth < 0)
            Die();
    }

    protected virtual void Die()
    {
        
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.getValue() + _targetStats.agility.getValue();
        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.getValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
}
