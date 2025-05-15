using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Character character = collider.GetComponent<Character>();

        if (character)
        {
            HealthBarSystem healthSystem = character.GetComponentInChildren<HealthBarSystem>();
            healthSystem.TakeDamage(1);
        }
    }
}

