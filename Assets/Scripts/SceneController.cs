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
    [SerializeField] private Door _door;
  
    private bool _isDoorUnlocked;
    private HealthBarSystem _healthSystem;

    private void Awake()
    {
        if (characterPrefab == null || playerSpawnPoint == null || healPackPrefab == null || keyPoint == null)
        {
            Debug.LogError("One or more required fields are not assigned!");
            return;
        }
        
        if (_door == null)
        {
            Debug.LogWarning("Door reference is not assigned in SceneController!");
        }

        GameObject character = Instantiate(characterPrefab, playerSpawnPoint.position, Quaternion.identity);
        SetupCharacter(character);
        SpawnHealPack();
        if (_door != null)
            _door.OnDoorEntered += RestartScene;
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
        if (_door != null)
            _door.OnDoorEntered -= RestartScene;
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
