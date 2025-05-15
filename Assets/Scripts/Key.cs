using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Key : MonoBehaviour
{
    AudioSource keySound;
    private KeyController keyController;

    private void Awake()
    {
        if (!TryGetComponent(out keySound))
        {
            keySound = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("AudioSource not found on Key. Adding one automatically.");
        }
    }

    public void SetController(KeyController controller)
    {
        keyController = controller;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character != null)
        {
            keyController.AddKey();
            keySound.Play();
            Destroy(gameObject, keySound.clip.length);
        }
    }
}