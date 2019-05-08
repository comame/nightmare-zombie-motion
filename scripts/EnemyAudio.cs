using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour {
    private AudioSource audioSource;
    private AudioClip[] zombieAudioClips = new AudioClip[3];
    private AudioClip dashAudioClip;
    private float deltaTime = 0.0f;
    private float SEInterval;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        zombieAudioClips[0] = Resources.Load("SE/www.vita-chi.net/zombie-1") as AudioClip;
        zombieAudioClips[1] = Resources.Load("SE/www.vita-chi.net/zombie-2") as AudioClip;
        zombieAudioClips[2] = Resources.Load("SE/www.vita-chi.net/zombie-3") as AudioClip;
        dashAudioClip = Resources.Load("SE/on-jin.com/dash-1") as AudioClip;

        SEInterval = Random.Range(4f, 8f);
    }

    void Update() {
        deltaTime += Time.deltaTime;
        if (deltaTime > SEInterval)
        {
            SEInterval = Random.Range(4.0f, 8.0f);
            deltaTime = 0;
            int random = Random.Range(0, zombieAudioClips.Length);
            audioSource.PlayOneShot(zombieAudioClips[random], 0.3f);
        }
    }

    public void soundRushAttack() {
        audioSource.PlayOneShot(dashAudioClip, 0.5f);
    }
}
