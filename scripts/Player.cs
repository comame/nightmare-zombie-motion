using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {
    public float invincibilityTime = 1.0f;
    public int specialAttackRatio = 20;
    public int normalDamage = 1;
    public int specialDamage = 10;
    public int heal = 30;

    private GameObject HPBar;
    private PlayerAudio playerAudio;

    public float deltaTime = 0.0f;
    private int HP;

    void Start() {
        HPBar = GameObject.Find("HPBar");
        gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<PlayerAudio>();
        playerAudio = GetComponent<PlayerAudio>();

        HP = 100;
        gameObject.tag = "Player";
    }
    
    private void DecreaseHP(int value) {
        if (HP > value) {
            HP = HP - value;
        } else {
            HP = 0;
        }

        HPBar.GetComponent<Image>().fillAmount = (float)HP / 100;
    }

    private void Heal(int value) {
        HP = HP + value;
        if (HP > 100) {
            HP = 100;
        }

        HPBar.GetComponent<Image>().fillAmount = (float)HP / 100;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "heal") {
            Heal(heal);
            playerAudio.soundHeal();
        }
    }

    private void OnCollisionStay(Collision other) {
        // ダメージ後一定の無敵時間を設定
        if (deltaTime > invincibilityTime && other.gameObject.tag == "enemy") {
            deltaTime = 0.0f;
            switch (selectAttackType()) {
                case "normal":
                    DecreaseHP(normalDamage);
                    playerAudio.soundNormalDamage();
                    break;
                case "special":
                    DecreaseHP(specialDamage);
                    playerAudio.soundSpecialDamage();
                    break;
            }
        } else {
            deltaTime += Time.deltaTime;
        }

        if (HP == 0) {
            onDead();
            // GameObject.Destroy(gameObject);
        }
    }

    private string selectAttackType() {
        float random = Random.Range(0f, 1f);
        float ratio = (float)specialAttackRatio / 100;

        if (random <= ratio) {
            return "special";
        } else {
            return "normal";
        }

    }

    private void onDead() {
        Debug.LogWarning("Player.onDead is not implemented.");
    }
}
