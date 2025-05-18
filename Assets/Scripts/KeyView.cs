
using UnityEngine;
using TMPro;

public class KeyView : MonoBehaviour
{
    [SerializeField] private TMP_Text keysCounterText;

    public void UpdateKeysCount(int collected, int required)
    {
        if (keysCounterText != null)
        {
            keysCounterText.text = $"Собрано {collected}/{required} ключей";
        }
    }
}