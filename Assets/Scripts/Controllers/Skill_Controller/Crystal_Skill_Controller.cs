using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Player player;
    
    private float crystalExistTimer;
    private bool canExplode;
    private bool canMoveToEnemy;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;

    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMoveToEnemy, float _moveSpeed, Transform _closestTarget, Player _player)
    {
        player = _player;
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMoveToEnemy = _canMoveToEnemy;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
    }

    public void ChooseRandomEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, SkillManager.instance.blackhole.GetBlackholeRadius(), whatIsEnemy);
        
        if(colliders.Length > 0)
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            FinishCrytal();
        }

        if (canMoveToEnemy)
        {
            if (closestTarget == null)
                return;

            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, closestTarget.position) < 1){
                FinishCrytal();
                canMoveToEnemy = false;
            }
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());
                ItemData_Equipment equippedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
                if (equippedAmulet)
                {
                    equippedAmulet.Effect(hit.transform);
                }
            }
        }
    }

    public void FinishCrytal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}
