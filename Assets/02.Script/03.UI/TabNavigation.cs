using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TabNavigation : MonoBehaviour
{
    public List<TMP_InputField> inputFields;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;
            Debug.Log("1번");
            if (current == null) return;

            for (int i = 0; i < inputFields.Count; i++)
            {
                if (current == inputFields[i].gameObject)
                {
                    int nextIndex = (i + 1) % inputFields.Count;
                    inputFields[nextIndex].Select();
                    inputFields[nextIndex].ActivateInputField();
                    Debug.Log("3번");
                    break;
                }
                Debug.Log("2번");
            }
        }
    }
}