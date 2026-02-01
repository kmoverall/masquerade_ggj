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
        if (PARTY_SIZE < 2)
        {
            Debug.LogError("Party Size must be at least 2!");
            return;
        }

        int killerIndex = Random.Range(0, PARTY_SIZE);
        int victimIndex = killerIndex;
        while (victimIndex == killerIndex)
        {
            victimIndex = Random.Range(0, PARTY_SIZE);
        }

        for (int i = 0; i < PARTY_SIZE; i++)
        {
            var pos = Game.Room.RandomPointInside;
            var newGO = Instantiate(_partyGoerPrefab);
            newGO.transform.position = pos;

            var newPartygoer = newGO.GetComponentInChildren<PartyGoer>();
            if (i == killerIndex)
            {
                newPartygoer.role = PartyGoer.Role.Killer;
                Game.Killer = newPartygoer;
            }
            if (i == victimIndex)
            {
                newPartygoer.role = PartyGoer.Role.Victim;
                Game.Victim = newPartygoer;
            }
            Game.PartyGoers.Add(newPartygoer);
        }
    }
}
