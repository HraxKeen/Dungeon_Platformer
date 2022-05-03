using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDestroyAudio : MonoBehaviour
{
    AudioSource fxSound;
    public AudioClip backMusic;

    public AudioSource s;
    private void Awake() 
    {
        //DontDestroyOnLoad(transform.gameObject);

    }
     void Start ()
 {
     // Audio Source responsavel por emitir os sons
     fxSound = GetComponent<AudioSource> ();
     fxSound.Play ();
 }
}
