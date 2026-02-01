using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _strikes;
    [SerializeField]
    private Button _accuseButton;
    [SerializeField]
    private Button _cancelButton;
    [SerializeField]
    private AudioSource _buzzerAudio;

    private void Start()
    {
        Game.AccusationFailed += UpdateStrikes;
        Game.ZoomInComplete += OnZoomedIn;
        Game.ZoomOutStarted += OnZoomOut;
    }

    public void MakeAccusation()
    {
        Game.AccusationMade?.Invoke(Game.SelectedPartyGoer);
    }

    public void CancelSelection()
    {
        Game.Camera.ReturnToHome();
        Game.SelectedPartyGoer = null;
    }

    private void UpdateStrikes()
    {
        _buzzerAudio.Play();
        for (int i = 0; i < _strikes.Count; i++)
        {
            _strikes[i].SetActive(i < Game.Strikes);
        }
    }

    private void OnZoomedIn()
    {
        _accuseButton.gameObject.SetActive(true);
        _cancelButton.gameObject.SetActive(true);
    }

    private void OnZoomOut()
    {
        _accuseButton.gameObject.SetActive(false);
        _cancelButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Game.AccusationFailed -= UpdateStrikes;
        Game.ZoomInComplete -= OnZoomedIn;
        Game.ZoomOutStarted -= OnZoomOut;
    }

}
