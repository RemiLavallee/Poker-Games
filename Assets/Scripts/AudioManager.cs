using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;

    public AudioClip shuffle;
    public AudioClip drawCard;
    public AudioClip hitDamage;
    public AudioClip pressButton;

    private void Start()
    {
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
}
