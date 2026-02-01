using UnityEngine;
using System.Collections.Generic;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField]
    private Room _room;
    [SerializeField]
    private Spawner _spawner;
    [SerializeField]
    private Director _director;
    [SerializeField]
    private List<Transform> _interactables;
    [SerializeField]
    private MurderTracker _murderTracker;
    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private CameraController _camera;

    void Awake()
    {
        Time.timeScale = 0;

        Game.Camera = _camera;

        Game.UI = _uiManager;
        Game.Strikes = 0;
        Game.MurderProgress = 0;
        Game.Room = _room;
        Game.Interactables = _interactables;
        Game.Spawner = _spawner;
        Game.Director = _director;
        Game.MurderTracker = _murderTracker;
    }
}
