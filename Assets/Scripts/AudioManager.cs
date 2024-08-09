using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip beepClip;
    [SerializeField] private AudioSource audioSource;
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Beeps() => audioSource.PlayOneShot(beepClip);
}