using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{

    private AudioSource _audioSource;
    public AudioClip[] splashAudio;
    public AudioClip bombAudio;

    public AudioClip[] katanaAudio;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlayBombAudioClip()
    {
        _audioSource.PlayOneShot(bombAudio,0.2f);
    }

    public void PlaySplashAudioClip(){
        _audioSource.PlayOneShot(splashAudio[Random.Range(0,splashAudio.Length)],0.6f);
    }

        public void PlayKatanaAudioClip(){
        _audioSource.PlayOneShot(katanaAudio[Random.Range(0,katanaAudio.Length)],0.2f);
    }
}
