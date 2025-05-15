using System;
using UnityEngine;

public class Healpack : MonoBehaviour
{
    private void Awake()
    {
        if (!TryGetComponent(out HealpackAudio))
        {
            HealpackAudio = gameObject.AddComponent<AudioSource>();
            Debug.Log("AudioSource not found on Key. Adding one automatically.");
        }
    }

    AudioSource HealpackAudio;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Character character = collider.GetComponent<Character>();
        
        if (character != null)
        {
            
            HealthBarSystem healthSystem = character.GetComponentInChildren<HealthBarSystem>();
            
            if (healthSystem != null)
            {
                healthSystem.Heal(1);
                HealpackAudio.Play();
                Destroy(gameObject, HealpackAudio.clip.length);
            }
            else
            {
                Debug.LogError("HealthBarSystem not found on Character or its children!");
            }
        }
    }
}