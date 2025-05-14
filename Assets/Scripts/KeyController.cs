using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] private KeyModel keyModel;
    [SerializeField] private KeyView keyView;

    public void Start()
    {
        SpawnKeys();
        UpdateKeysView();
    }

    public void AddKey()
    {
        keyModel.keysCollected++;
        UpdateKeysView();
    }

    private void UpdateKeysView()
    {
        keyView.UpdateKeysCount(keyModel.keysCollected);
    }

    private void SpawnKeys()
    {
        if (keyModel.keyPrefab == null || keyModel.keySpawnPoints == null)
        {
            Debug.LogError("Key prefab or spawn points not assigned!");
            return;
        }

        foreach (Transform spawnPoint in keyModel.keySpawnPoints)
        {
            if (spawnPoint != null)
            {
                GameObject key = Instantiate(keyModel.keyPrefab, spawnPoint.position, Quaternion.identity);
                key.GetComponent<Key>().SetController(this);
            }
        }
    }
}