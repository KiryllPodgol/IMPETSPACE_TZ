using UnityEngine;

public class Healpack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Character character = collider.GetComponent<Character>();
        
        if (character != null)
        {
            
            HealthBarSystem healthSystem = character.GetComponentInChildren<HealthBarSystem>();
            
            if (healthSystem != null)
            {
                healthSystem.Heal(1);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("HealthBarSystem not found on Character or its children!");
            }
        }
    }
}