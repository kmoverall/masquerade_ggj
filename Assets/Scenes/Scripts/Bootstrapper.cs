using UnityEngine;
using System.Collections.Generic;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField]
    private Room _room;
    [SerializeField]
    private Spawner _spawner;

    void Awake()
    {
        Game.Room = _room;
        Game.Spawner = _spawner;
        _spawner.Init();
    }
}
