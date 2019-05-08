using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    public float attackMotionDistance = 2.0F;
    public float maxRushAttackDuration = 2.0f;
    public float startRushAttackMotionDistance = 10f;
    public float endRushAttackMotionDistance = 1f;
    public float waitTimeFromLastRushAttack = 5.0f;
    public int rushAttackRatio = 1;
    public int bulletDamage = 20;
    public int bombDamage = 100;

    private GameObject player;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private CapsuleCollider capsuleCollider;
    private EnemyAudio enemyAudio;
    
    private int HP = 100;
    private bool isRushing = false;
    private float rushingTime = 0f;
    private float rushInterval = 0f;
    private bool isKicking = false;
    private float kickingTime = 0f;

    void Start(){
        gameObject.AddComponent<EnemyAudio>();
        gameObject.AddComponent<CapsuleCollider>();
        gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<NavMeshAgent>();

        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        enemyAudio = GetComponent<EnemyAudio>();

        gameObject.tag = "enemy";
        animator.runtimeAnimatorController = 
            RuntimeAnimatorController.Instantiate(
                Resources.Load("Animations/Attack")
            ) as RuntimeAnimatorController;
        navMeshAgent.speed = 1;
        capsuleCollider.radius = 0.4f;
        capsuleCollider.height = 2f;
        capsuleCollider.center = new Vector3(0, 0.7f, 0);
        
        if (player == null) {
            Debug.LogError("Player Object must be named \"Player\".");
        }
    }

    void Update() {
        if (player != null) {
            navMeshAgent.destination = player.transform.position;

            float distanceFromPlayer = Vector3.Distance(
                gameObject.transform.position,
                player.transform.position
            );

            // ダッシュ攻撃
            if (isRushing) {
                rushingTime += Time.deltaTime;
            } else {
                rushInterval += Time.deltaTime;
            }
            if (
                !isRushing && distanceFromPlayer <= startRushAttackMotionDistance &&
                distanceFromPlayer >= endRushAttackMotionDistance && 
                ifStartsRushAttack() && 
                rushInterval >= waitTimeFromLastRushAttack
            ) {
                enemyAudio.soundRushAttack();
                animator.SetBool("rush", true);
                isRushing = true;
            }
            if (
                isRushing &&
                (distanceFromPlayer <= endRushAttackMotionDistance || rushingTime >= maxRushAttackDuration)
            ) {
                animator.SetBool("rush", false);
                isRushing = false;
                rushingTime = 0f;
                rushInterval = 0f;
                if (distanceFromPlayer <= endRushAttackMotionDistance) {
                    // ダッシュしてプレーヤーに近づいたらキックする
                    animator.SetBool("kick", true);
                    isKicking = true;
                }
            }
            // キック後の待機モーションをカット
            if (isKicking && kickingTime <= 1.5f) {
                kickingTime += Time.deltaTime;
            } else if (isKicking) {
                animator.SetBool("kick", false);
                isKicking = false;
                kickingTime = 0f;
            }
            
            // 通常攻撃
            if (!isRushing && distanceFromPlayer <= attackMotionDistance) {
                animator.SetBool("attack", true);
            } else {
                animator.SetBool("attack", false);
            }
        } else {
            // プレイヤーが死亡したら攻撃を停止
            animator.SetBool("attack", false);
            animator.SetBool("rush", false);
            isRushing = false;
            rushingTime = 0f;
        }
    }

    // 強制的にダッシュ攻撃を発動
    public void ForceStartRushAttack() {
        enemyAudio.soundRushAttack();
        animator.SetBool("attack", false);
        animator.SetBool("rush", true);
        isRushing = true;
        rushingTime = 0f;
        rushInterval = 0f;
    }

    private bool ifStartsRushAttack() {
        int random = Random.Range(0, 100);
        if (random >= rushAttackRatio) {
            return true;
        } else {
            return false;
        }
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "bullet") {
            damage(bulletDamage);
        } else if (collision.gameObject.tag == "bomb") {
            damage(bombDamage);
        }
        if (HP == 0) {
            onBeforeDead();
            GameObject.Destroy(gameObject);
        }
    }

    private void damage(int damage) {
        if (HP > damage) {
            HP = HP - damage;
        } else {
            HP = 0;
        }
    }

    private void onBeforeDead() {
        Debug.LogWarning("Enemy.onBeforeDead is not implemented.");
    }
}
