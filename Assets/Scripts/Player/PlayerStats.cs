using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    [SerializeField] private GameObject deathChunkParticle, deathBloodParticle;

    public float currentHealth;

    private GameManager GM;

    public healthBar healthBar;

    public bool key = false;
    
    public Image imageKey;

    private void Start() 
    {
        imageKey.enabled = false;

        currentHealth = maxHealth;

        healthBar.SetMaxHealth(maxHealth);

    }
    private void Update()
    {
        
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);
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
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Heal"))
        {
            other.gameObject.SetActive(false);
            currentHealth += 30;
            healthBar.SetHealth(currentHealth);
            soundEffects.sfxInstance.Audio.PlayOneShot(soundEffects.sfxInstance.heal);
            StartCoroutine(VisualIndicator(Color.green));
        }
        if(other.gameObject.CompareTag("Key"))
        {

            other.gameObject.SetActive(false);

            imageKey.enabled = true;

            key = true;
            soundEffects.sfxInstance.Audio.PlayOneShot(soundEffects.sfxInstance.key);
            StartCoroutine(VisualIndicator(Color.yellow));
        }
        
    }
}
