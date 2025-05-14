using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameObject healPackPrefab;
    [SerializeField] private Transform healPackPoint;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform keyPoint;
    [SerializeField] private KeyController keyController;

    private void Awake()
    {
        if (characterPrefab == null)
        {
            Debug.LogError("Character prefab is not assigned!");
            return;
        }

        if (playerSpawnPoint == null)
        {
            Debug.LogError("Player spawn point is not assigned!");
            return;
        }

        if (healPackPrefab == null)
        {
            Debug.LogError("Heal pack prefab is not assigned!");
            return;
        }

        if (keyPoint == null)
        {
            Debug.LogError("Key point is not assigned!");
            return;
        }
        
        GameObject character = Instantiate(characterPrefab, playerSpawnPoint.position, Quaternion.identity);
        SetupCharacter(character);
        SpawnHealPack();
        
        if (keyController != null)
        {
            keyController.Start();
        }
        else
        {
            Debug.LogError("KeyController is not assigned!");
        }
    }
    
    private void SpawnHealPack()
    {
        if (healPackPrefab == null)
        {
            Debug.LogError("Heal pack prefab is not assigned!");
            return;
        }

        if (healPackPoint == null)
        {
            Debug.LogError("Heal point is not assigned!");
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
                Debug.LogError("CameraControl script not found on Main Camera!");
            }
        }
    }
}