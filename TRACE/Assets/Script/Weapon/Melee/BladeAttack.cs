using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BladeAttack : MonoBehaviour
{
    [Header("References")] 
    public Camera fpsCam;
    
    [Header("Settings")] 
    private float attackSpeed = 0.4f;
    private float attackDelay = 1f;
    private float attackDistance = 3f;
    private float attackDamage = 20f;
    public bool isAttacking;
    public bool isReadyToAttack;
    
    [Header("Input")]
    public KeyCode attackBind =  KeyCode.Q;
    
    void Start()
    {
        isAttacking = false;
        isReadyToAttack = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(attackBind))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (isAttacking || !isReadyToAttack) return;
        
        isAttacking = true;
        isReadyToAttack = false;
        
        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRayCasting), attackDelay);
    }

    private void AttackRayCasting()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, attackDistance))
        {
            if (hit.transform.tag == "Enemy")
            {
                EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    private void ResetAttack()
    {
        isReadyToAttack =  true;
        isAttacking = false;
    }
}