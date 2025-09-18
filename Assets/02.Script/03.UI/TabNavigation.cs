using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TabNavigation : MonoBehaviour
{
    [SerializeField] List<TMP_InputField> inputFields;
    [SerializeField] Button loginButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HandleTabNavigation();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (loginButton != null && loginButton.interactable)
            {
                Debug.Log("Enter key");
                loginButton.onClick.Invoke();
            }
        }
    }
    
    private void HandleTabNavigation()
    {

        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current == null) return;

        for (int i = 0; i < inputFields.Count; i++)
        {
            if (current == inputFields[i].gameObject)
            {
                int nextIndex = (i + 1) % inputFields.Count;
                inputFields[nextIndex].Select();
                inputFields[nextIndex].ActivateInputField();
                break;
            }
        }

    }
}