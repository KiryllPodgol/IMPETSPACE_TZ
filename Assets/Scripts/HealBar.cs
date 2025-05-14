using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject[] _hearts;
    public void SetHealth(int currentHealth)
    {
    
        if (_hearts == null || _hearts.Length == 0)
        {
            Debug.LogError("Hearts array is not assigned or empty!");
            return;
        }
        
        for (int i = 0; i < _hearts.Length; i++)
        {
            _hearts[i].SetActive(i < currentHealth);
        }
    }
}