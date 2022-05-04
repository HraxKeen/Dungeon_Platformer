using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffects : MonoBehaviour
{
    public AudioSource Audio;

    public AudioClip pAttack;
    public AudioClip eAttack;
    public AudioClip dSound;
    public AudioClip pHit;
    public AudioClip eHit;
    public AudioClip heal;


    public static soundEffects sfxInstance;
    // Start is called before the first frame update
    private void Awake() 
    {
        if(sfxInstance != null && sfxInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        sfxInstance = this;
        DontDestroyOnLoad(this);

        
    }
}
