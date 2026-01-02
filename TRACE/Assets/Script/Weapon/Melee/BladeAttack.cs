using UnityEngine;
using EZCameraShake;

public class BladeAttack : MonoBehaviour
{
    [Header("References")] 
    public Animator animator;
    public TrailRenderer trailRenderer;
    
    [Header("Settings")] 
    private float attackSpeed = 0.1f;
    private float attackDistance = 3f;
    private float attackDamage = 50f;
    
    public bool isAttacking;
    public bool isReadyToAttack;
    public float delay = 1f;
    private int animatorCounter = 0;
    private float trailDelay = 1f;

    [Header("Constants")] 
    private const string EnemyTag = "Enemy";
    
    [Header("Input")]
    public KeyCode attackBind =  KeyCode.Q;

    [Header("Audio")] 
    public AudioSource[] sounds;
    
    void Start()
    {
        isAttacking = false;
        isReadyToAttack = true;
        trailRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(attackBind))
        {
            Attack();
        }
        
        if (delay > 0)
        {
            delay -= Time.deltaTime;
            if(delay < 0f) delay = 0f;
        }
        
    }

    private void Attack()
    {
        if (isAttacking || !isReadyToAttack) return;
        
        PlayRandom();
        
        CameraShaker.Instance.ShakeOnce(2f, 2f, 0.1f, 1f);
        
        trailRenderer.enabled = true;
        isAttacking = true;
        isReadyToAttack = false;
        
        if (animator != null)
        {
            animatorCounter++;
            if(animatorCounter > 2) animatorCounter = 1;
            animator.SetInteger("Attack", animatorCounter);
        }
        
        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(DisableTrail), trailDelay);
    }
    
    private void ResetAttack()
    {
        isReadyToAttack = true;
        isAttacking = false;
        delay = 0f;
    }

    private void DisableTrail()
    {
        trailRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(EnemyTag) && isAttacking)
        {
            EnemyHealth enemyHealth = collider.transform.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log("Sliced");
            }
        }
    }
    
    private void PlayRandom()
    {
        int index = Random.Range(0, sounds.Length);
        sounds[index].Play();
    }
}