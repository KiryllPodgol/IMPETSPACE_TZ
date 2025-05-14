
using UnityEngine;
using TMPro;

public class KeyView : MonoBehaviour
{
    [SerializeField] private TMP_Text keysCounterText;

    public void UpdateKeysCount(int count)
    {
        if (keysCounterText != null)
        {
            keysCounterText.text = $"{count}";
        }
    }
}