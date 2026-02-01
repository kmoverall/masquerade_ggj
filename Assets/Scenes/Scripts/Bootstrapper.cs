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

    void Awake()
    {
        Game.Room = _room;
        Game.Interactables = _interactables;
        Game.Spawner = _spawner;
        Game.Director = _director;
        Game.MurderTracker = _murderTracker;

        _spawner.Init();
        _director.InitializePartyGoers();
    }
}
