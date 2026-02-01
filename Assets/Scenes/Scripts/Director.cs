using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Director : MonoBehaviour
{
    [SerializeField]
    private float INSPECT_DISTANCE;
    [SerializeField]
    private float TALK_DISTANCE;

    [SerializeField]
    private float DIRECTION_INTERVAL;
    [SerializeField]
    private int DIRECTIONS_PER_INTERVAL;
    private float _directionTimer = 0;

    [SerializeField]
    private int _wanderWeight;
    [SerializeField]
    private int _inspectWeight;
    [SerializeField]
    private int _talkWeight;

    public void InitializePartyGoers()
    {
        for (int i = 0; i < Game.PartyGoers.Count; i++)
        {
            var current = Game.PartyGoers[i];
            if (current.CurrentState != PartyGoer.State.Idle)
                continue;

            foreach (var interactable in Game.Interactables)
            {
                if (Vector3.Distance(current.transform.position, interactable.position) < INSPECT_DISTANCE)
                {
                    current.SetInspect(interactable);
                    break;
                }
            }
            if (current.CurrentState != PartyGoer.State.Idle)
                continue;

            for (int j = i + 1; j < Game.PartyGoers.Count; j++)
            {
                var other = Game.PartyGoers[j];
                if (Vector3.Distance(current.transform.position, other.transform.position) < TALK_DISTANCE)
                {
                    current.SetTalk(other);
                    other.SetTalk(current);
                    break;
                }
            }
            if (current.CurrentState != PartyGoer.State.Idle)
                continue;


            Game.PartyGoers[i].SetWander();
        }
    }

    private void Update()
    {
        // No Partygoers, we assume the game hasn't started
        if (Game.PartyGoers.Count == 0)
            return;

        if (DIRECTIONS_PER_INTERVAL > Game.PartyGoers.Count / 2)
        {
            Debug.LogError("Directions per interval must be half or less of the total partygoers!");
        }
        _directionTimer += Time.deltaTime;
        if (_directionTimer < DIRECTION_INTERVAL)
            return;

        _directionTimer = 0;

        List<int> targets = new();
        List<int> activated = new();
        for (int i = 0; i < DIRECTIONS_PER_INTERVAL; i++)
        {
            int rand = Random.Range(0, Game.PartyGoers.Count);
            while (targets.Contains(rand))
            {
                rand = Random.Range(0, Game.PartyGoers.Count);
            }

            targets.Add(rand);
        }

        for (int i = 0; i < targets.Count; i++)
        {
            DirectGuest(i, activated);
        }
    }

    private void DirectGuest(int target, List<int> activated)
    {
        if (activated.Contains(target))
            return;

        PartyGoer guest = Game.PartyGoers[target];
        if (!guest.readyForDirection)
        {
            activated.Add(target);
            return;
        }

        int rand = Random.Range(0, _wanderWeight + _inspectWeight + _talkWeight);
        if (rand < _wanderWeight)
        {
            guest.SetWander();
        }
        else if (rand < _wanderWeight + _inspectWeight && Game.PartyGoers[target].CurrentState != PartyGoer.State.Inspect)
        {
            if (guest.role == PartyGoer.Role.Killer && Game.MurderTracker.IsPreparing)
            {
                guest.SetInspect(Game.MurderTracker.NextTarget);
            }
            else if (guest.role == PartyGoer.Role.Victim && Game.MurderTracker.IsKillNext)
            {
                guest.SetInspect(Game.MurderTracker.NextTarget);
            }
            else
            {
                int objRand = Random.Range(0, Game.Interactables.Count);
                guest.SetInspect(Game.Interactables[objRand]);
            }
        }
        else if (Game.PartyGoers[target].CurrentState != PartyGoer.State.Talk)
        {
            if (guest.role == PartyGoer.Role.Killer && Game.MurderTracker.IsVictimNext)
            {
                guest.SetTalk(Game.Victim);
                Game.Victim.SetTalk(guest);
                
                for (int i = 0; i < Game.PartyGoers.Count; i++)
                {
                    if (Game.PartyGoers[i] == Game.Victim)
                    {
                        activated.Add(i);
                        break;
                    }
                }
            }
            else
            {
                int attempts = 0;
                int talkTarget = target;
                while ((talkTarget == target || activated.Contains(talkTarget) || !Game.PartyGoers[talkTarget].readyForDirection) && attempts < 50)
                {
                    talkTarget = Random.Range(0, Game.PartyGoers.Count);
                }

                if (attempts >= 50)
                {
                    guest.SetWander();
                }
                else
                {
                    guest.SetTalk(Game.PartyGoers[talkTarget]);
                    Game.PartyGoers[talkTarget].SetTalk(guest);

                    activated.Add(talkTarget);
                }
            }
        }
        else
        {
            guest.SetWander();
        }

        activated.Add(target);
    }
}
