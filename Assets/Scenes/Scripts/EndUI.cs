using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _reason;

    [SerializeField]
    private Button _replay;

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
