using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class EndUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _reason;

    [SerializeField]
    private Button _replay;
    [SerializeField]
    private List<AudioSource> _audioSources;

    public void PlayAudio(int index)
    {
        _audioSources[index].Play();
    }

    public void SetReason(string text)
    {
        if (_reason == null)
            return;
        _reason.text = text;
    }

    public void Replay()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
