using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameObject healPackPrefab;
    [SerializeField] private Transform healPackPoint;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform keyPoint;
    [SerializeField] private KeyController keyController;
    [SerializeField] private Collider2D _doorTrigger;  
    [SerializeField] private SpriteRenderer _doorSprite;
    private bool _isDoorUnlocked;
    private HealthBarSystem _healthSystem;

    private void Awake()
    {
        if (characterPrefab == null || playerSpawnPoint == null || healPackPrefab == null || keyPoint == null)
        {
            Debug.LogError("One or more required fields are not assigned!");
            return;
        }

        GameObject character = Instantiate(characterPrefab, playerSpawnPoint.position, Quaternion.identity);
        SetupCharacter(character);
        SpawnHealPack();

        if (keyController != null)
        { 
            keyController.OnAllKeysCollected += UnlockDoor;
        }
        _doorTrigger.enabled = false;
        if (_doorSprite != null)
            _doorSprite.color = Color.red;
    }
    private void UnlockDoor()
    {
        _isDoorUnlocked = true;
        _doorTrigger.enabled = true;
        
        if (_doorSprite != null)
            _doorSprite.color = Color.green; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isDoorUnlocked && other.TryGetComponent<Character>(out _))
        {
            RestartScene();
        }
    }

    private void SpawnHealPack()
    {
        if (healPackPrefab == null || healPackPoint == null)
        {
            Debug.LogError("Heal pack prefab or heal point is not assigned!");
            return;
        }

        Instantiate(healPackPrefab, healPackPoint.position, Quaternion.identity);
    }

    private void SetupCharacter(GameObject character)
    {
        Character characterScript = character.GetComponent<Character>();

        if (characterScript == null)
        {
            Debug.LogError("Character script not found on prefab!");
            return;
        }

        characterScript.Initialize(respawnPoint);

        SetupCamera(character.transform);
        
        HealthBarSystem healthSystem = character.GetComponentInChildren<HealthBarSystem>();
        if (healthSystem != null)
        {
            _healthSystem = healthSystem;
            _healthSystem.OnDeath += HandlePlayerDeath;
        }
        else
        {
            Debug.LogError("Health System missing!");
        }
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Player died! Restarting scene...");

        if (_healthSystem != null)
        {
            _healthSystem.OnDeath -= HandlePlayerDeath;
        }

       RestartScene();
    }

    private void OnDestroy()
    {
        if (_healthSystem != null)
        {
            _healthSystem.OnDeath -= HandlePlayerDeath;
        }
        
        if (keyController != null)
        {
            keyController.OnAllKeysCollected -= UnlockDoor;
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetupCamera(Transform target)
    {
        if (Camera.main != null)
        {
            CameraControl cameraControl = Camera.main.GetComponent<CameraControl>();
            if (cameraControl != null)
            {
                cameraControl.target = target;
            }
            else
            {
                Debug.LogError("Camera script not found on Main Camera!");
            }
        }
    }
}
