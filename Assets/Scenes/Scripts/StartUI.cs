using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _helpPanel;

    public void StartGame()
    {
        Time.timeScale = 1;
        Game.StartGame?.Invoke();
    }

    public void ShowHelp()
    {
        _helpPanel.SetActive(true);
    }
    public void CloseHelp()
    {
        _helpPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
