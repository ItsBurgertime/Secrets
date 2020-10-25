using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class SoundEffectCollider : MonoBehaviour
{
    public AudioClip[] soundEffects;

    private AudioSource src;

    [SerializeField]
    [Range(0,100)]
    private int soundChance;


    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            int rnd = Random.Range(0, 100);
            Debug.Log(rnd);
            if (soundChance >= rnd)
            {
                Debug.Log("played sound");
                src.PlayOneShot(soundEffects[Random.Range(0, soundEffects.Length)]);
            }
        }
    }
}
