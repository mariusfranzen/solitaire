using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameUiScript : MonoBehaviour
{
    public BoardScript BoardScript;

    private UIDocument _uiDocument;
    private Button _restartButton;

    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        _restartButton = _uiDocument.rootVisualElement.Q<Button>("RestartButton");

        _restartButton.RegisterCallback<ClickEvent>(OnClickRestart);
    }

    void OnClickRestart(ClickEvent evt)
    {
        Debug.Log("Restarting");
        BoardScript.RestartGame();
    }
}
