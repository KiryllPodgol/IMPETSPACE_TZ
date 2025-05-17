
using UnityEngine;

[System.Serializable]
public class KeyModel
{
    public int keysCollected = 0;
    public int keysRequired = 3;
    public GameObject keyPrefab;
    public Transform[] keySpawnPoints;
    
}