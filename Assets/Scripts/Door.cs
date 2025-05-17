using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _doorSprite;
    [SerializeField] private Collider2D _doorTrigger;
    [SerializeField] private KeyController _keyController;
    public event Action OnDoorEntered;
    private bool _isUnlocked;

    private void Awake()
    {
        if (_doorTrigger != null)
            _doorTrigger.enabled = false;

        if (_doorSprite != null)
            _doorSprite.color = Color.red;

        if (_keyController != null)
            _keyController.OnAllKeysCollected += UnlockDoor;
    }
    private void UnlockDoor()
    {
        _isUnlocked = true;

        if (_doorTrigger != null)
            _doorTrigger.enabled = true;

        if (_doorSprite != null)
            _doorSprite.color = Color.green;

        // Debug.Log("Door unlocked!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isUnlocked)
            return;

        if (other.GetComponent<Character>() != null)
        {
            // Debug.Log("Player entered the door!");
            OnDoorEntered?.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (_keyController != null)
            _keyController.OnAllKeysCollected -= UnlockDoor;
    }
}