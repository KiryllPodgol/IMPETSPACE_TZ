using UnityEngine;

public class healpack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Character character = collider.GetComponent<Character>();

        if (character)
        {
            HealthBarSystem healthSystem = character.GetComponent<HealthBarSystem>();
            healthSystem.Heal(1);
            Destroy(gameObject); 
        }
    }
}