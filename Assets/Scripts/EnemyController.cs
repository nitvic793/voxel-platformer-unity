using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float health = 100f;
    public bool isDead = false;
    GameObject player = null;
    Animator animator;
    public float runSpeed = 3F;
    public float walkSpeed = 2F;
    bool isWalking = false;
    bool isRunning = false;
    bool isAttacking = false;
    bool isIdle = true;
    bool isPatrolling = true;
    float idleTime = 0F;
    float fromLastTargetTime = 0F;
    float hitTime = 0F;

    public GameObject patrolA;
    public GameObject patrolB;
    string currentPatrolTarget = "A";

    Transform target;
    public bool hasTarget = false;

    void Start()
    {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateEnemy();
        UpdateAnimation();
        fromLastTargetTime += Time.deltaTime;
        hitTime += Time.deltaTime;
    }

    void UpdateEnemy()
    {
        isWalking = isRunning = isAttacking = false;
        if (health <= 0 || isDead)
        {
            isDead = true;
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                GetComponent<CapsuleCollider>().enabled = false;
            }
            return;
        }
        
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 8F))
        {
            if (hit.transform.tag == "Player")
            {
                fromLastTargetTime = 0F;
                hasTarget = true;
                target = hit.transform;
            }
        }

        if (fromLastTargetTime > 2F)
        {
            hasTarget = false;
        }

        if (hasTarget && target!=null)
        {
            var player = target.GetComponent<PlayerController>();
            if (player!=null && player.isGrounded)
            {
                if (Mathf.Approximately(transform.position.y, target.position.y))
                    transform.LookAt(target);
            }

            if (Vector3.Distance(transform.position, target.position) < 2F)
            {
                isAttacking = true;
                if (player != null && hitTime>2F)
                {
                    if(player.health>0F)
                    player.health -= 50F;
                    hitTime = 0F;
                }

                if (player == null)
                {
                    var decoy = target.GetComponent<DecoyController>();
                    decoy.InflictDamage(this);
                }
            }
            else
            {
                isRunning = true;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    var pos = transform.position;
                    var nextPos = Vector3.MoveTowards(transform.position, target.position, runSpeed * Time.deltaTime);
                    pos.x = nextPos.x;
                    transform.position = pos;
                }
            }
        }
        else
        {
            isRunning = isAttacking = false;
            isPatrolling = true;
            isWalking = true;
            Transform patrolTarget = null;
            if (patrolA == null || patrolB == null)
            {
                isWalking = false;
                return;
            }
            if (currentPatrolTarget == "A")
            {
                patrolTarget = patrolA.transform;
            }
            else
            {
                patrolTarget = patrolB.transform;
            }

            var pos = transform.position;
            var nextPos = Vector3.MoveTowards(transform.position, patrolTarget.position, walkSpeed * Time.deltaTime);
            pos.x = nextPos.x;
            transform.position = pos;
            transform.LookAt(patrolTarget);
            if (Vector3.Distance(transform.position, patrolTarget.position) < 1F)
            {
                currentPatrolTarget = currentPatrolTarget == "A" ? "B" : "A";
            }
        }
    }

    void UpdateAnimation()
    {
        animator.SetBool("Dead", isDead);
        animator.SetBool("Walk", isWalking);
        animator.SetBool("Run", isRunning);
        animator.SetBool("Attack", isAttacking);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name != "Player")
        {
            hasTarget = false;
        }
    }
}
