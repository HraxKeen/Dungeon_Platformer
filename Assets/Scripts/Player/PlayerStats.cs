using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject deathChunkParticle, deathBloodParticle;

    private float currentHealth;

    private GameManager GM;

    private void Start() 
    {
        currentHealth = maxHealth;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        soundEffects.sfxInstance.Audio.PlayOneShot(soundEffects.sfxInstance.pHit);
        StartCoroutine(VisualIndicator(Color.red));

        if(currentHealth <= 0.0f)
        {
            soundEffects.sfxInstance.Audio.PlayOneShot(soundEffects.sfxInstance.dSound);
            Die();
        }

    }
    private void Die()
    {
        Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        GM.Respawn();
        Destroy(gameObject);

    }
    private IEnumerator VisualIndicator(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
