using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    private AudioSource audioSource;
    private AudioClip[] damageAudioClips = new AudioClip[3];
    private AudioClip[] specialDamageAudioClips = new AudioClip[1];
    private AudioClip healAudioClip;

    void Start() {
        damageAudioClips[0] = Resources.Load("SE/on-jin.com/damage-1") as AudioClip;
        damageAudioClips[1] = Resources.Load("SE/on-jin.com/damage-2") as AudioClip;
        damageAudioClips[2] = Resources.Load("SE/on-jin.com/damage-3") as AudioClip;
        specialDamageAudioClips[0] = Resources.Load("SE/on-jin.com/special-damage-1") as AudioClip;
        healAudioClip = Resources.Load("SE/soundeffect-lab.info/heal-1") as AudioClip;
        audioSource = GetComponent<AudioSource>();
    }

    public void soundNormalDamage() {
        int random = Random.Range(0, damageAudioClips.Length);
        audioSource.PlayOneShot(damageAudioClips[random], 0.3f);
    }

    public void soundSpecialDamage() {
        int random = Random.Range(0, specialDamageAudioClips.Length);
        audioSource.PlayOneShot(specialDamageAudioClips[random], 0.5f);
    }

    public void soundHeal() {
        audioSource.PlayOneShot(healAudioClip, 0.5f);
    }
}
