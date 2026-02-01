
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private StartUI _startMenu;
    [SerializeField]
    private GameUI _gameUI;
    [SerializeField]
    private EndUI  _victoryScreen;
    [SerializeField]
    private EndUI _gameOverScreen;

    private void Start()
    {
        _startMenu.gameObject.SetActive(true);
        _gameUI.gameObject.SetActive(false);
        _victoryScreen.gameObject.SetActive(false);
        _gameOverScreen.gameObject.SetActive(false);

        Game.StartGame += StartGame;
        Game.StrikeOut += GameOverStrikeOut;
        Game.MurderOccured += GameOverMurder;
        Game.AccusationSuccess += Victory;
    }

    private void StartGame()
    {
        _startMenu.gameObject.SetActive(false);
        _gameUI.gameObject.SetActive(true);
    }

    private void Victory()
    {
        Time.timeScale = 0;
        _victoryScreen.gameObject.SetActive(true);
        _victoryScreen.PlayAudio(0);

    }

    private void GameOverStrikeOut()
    {
        Time.timeScale = 0;
        _gameOverScreen.gameObject.SetActive(true);
        _gameOverScreen.SetReason("The killer has escaped notice");

    }
    private void GameOverMurder()
    {
        Time.timeScale = 0;
        _gameOverScreen.gameObject.SetActive(true);

        if (Game.MurderMethod == MurderTracker.MurderMethod.Chandelier)
        {
            _gameOverScreen.SetReason("The victim has been crushed by a chandelier");
            _victoryScreen.PlayAudio(0);
        }
        else
        {
            _gameOverScreen.SetReason("The victim has died of poisoning");
            _victoryScreen.PlayAudio(1);
        }
    }

    private void OnDestroy()
    {
        Game.StartGame -= StartGame;
        Game.StrikeOut -= GameOverStrikeOut;
        Game.MurderOccured -= GameOverMurder;
    }
}
