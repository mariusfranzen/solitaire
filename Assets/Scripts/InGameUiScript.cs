using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InGameUiScript : MonoBehaviour
{
    public BoardScript BoardScript;

    public void OnClickRestart()
    {
        BoardScript.RestartGame();
    }

    public void OnClickMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
