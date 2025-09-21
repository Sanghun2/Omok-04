using System;
using TMPro;
using UnityEngine;

public class TMP : MonoBehaviour
{
    public GameObject gameUIManger;
    TextMeshProUGUI textVar;
    void Start()
    {
        
        textVar = GetComponent<TextMeshProUGUI>();

        textVar.text = "this";
    }

}
