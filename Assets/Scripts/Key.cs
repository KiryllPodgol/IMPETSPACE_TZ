using UnityEngine;

public class Key : MonoBehaviour
{
    private KeyController keyController;

    public void SetController(KeyController controller)
    {
        keyController = controller;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character != null)
        {
            keyController.AddKey();
            Destroy(gameObject);
        }
    }
}