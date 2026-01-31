using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _partyGoerPrefab;

    [SerializeField]
    private int PARTY_SIZE;

    public void Init()
    {
        int killerIndex = Random.Range(0, PARTY_SIZE);
        for (int i = 0; i < PARTY_SIZE; i++)
        {
            var pos = Game.Room.RandomPointInside;
            var newGO = Instantiate(_partyGoerPrefab);
            newGO.transform.position = pos;

            var newPartygoer = newGO.GetComponentInChildren<PartyGoer>();
            newPartygoer.isKiller = i == killerIndex;
            Game.PartyGoers.Add(newPartygoer);
        }
    }
}
