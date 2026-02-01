using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField]
    private Button _playButton;

    public void StartGame()
    {
        Time.timeScale = 1;
        Game.StartGame?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
