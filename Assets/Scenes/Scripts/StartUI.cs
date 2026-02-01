using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField]
    private Button _playButton;

    public void StartGame()
    {
        Game.StartGame?.Invoke();
    }
}
