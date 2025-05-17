
using UnityEngine;

public class DeathZone : MonoBehaviour
{
     private SceneController _sceneController;
    private void Awake()
    {
        _sceneController = GetComponentInParent<SceneController>(); 

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Character>() != null)
        {
            _sceneController.RestartScene();
        }
    }
}
