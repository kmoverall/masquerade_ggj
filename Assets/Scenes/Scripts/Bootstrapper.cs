using UnityEngine;
using System.Collections.Generic;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField]
    private Room _room;
    [SerializeField]
    private List<PartyGoer> _partyGoers;

    void Awake()
    {
        Game.Room = _room;
        Game.PartyGoers = _partyGoers;
    }
}
