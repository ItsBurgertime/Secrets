using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject poloPeer;
    public bool isEnabled;
    public AudioSource audioSource;
    public AudioClip marcoCall;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePeer();
        }
    }

    void TogglePeer()
    {
        isEnabled = !isEnabled;
        poloPeer.GetComponent<AudioSource>().enabled = isEnabled;
    }
}
