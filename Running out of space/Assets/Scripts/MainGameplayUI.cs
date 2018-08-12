using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainGameplayUI : MonoBehaviour
{
    public static MainGameplayUI Instance;

    public TextMeshProUGUI ItemCountText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void SetItemCount(int count)
    {
        ItemCountText.text = count.ToString();
    }
}
