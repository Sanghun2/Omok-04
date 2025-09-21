using System;
using TMPro;
using UnityEngine;

public class TMP : MonoBehaviour
{
    public GameObject gameUIManger;
    public GameResultManager gameResultManager;
    TextMeshProUGUI textVar;
    void Start()
    {
        
        textVar = GetComponent<TextMeshProUGUI>();
    }

}
