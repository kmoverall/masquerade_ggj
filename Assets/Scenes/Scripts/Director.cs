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
        if (DIRECTIONS_PER_INTERVAL > Game.PartyGoers.Count / 2)
        {
            Debug.LogError("Directions per interval must be half or less of the total partygoers!");
        }
        _directionTimer += Time.deltaTime;
        if (_directionTimer < DIRECTION_INTERVAL)
            return;

        _directionTimer = 0;

        List<int> targets = new();
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
            int rand = Random.Range(0, _wanderWeight + _inspectWeight + _talkWeight);
            if (rand < _wanderWeight)
            {
                Game.PartyGoers[targets[i]].SetWander();
            }
            else if (rand < _wanderWeight + _inspectWeight)
            {
                int objRand = Random.Range(0, Game.Interactables.Count);
                Game.PartyGoers[targets[i]].SetInspect(Game.Interactables[objRand]);
            }
            else if (i != 0)
            {
                Game.PartyGoers[targets[i]].SetTalk(Game.PartyGoers[i - 1]);
                Game.PartyGoers[i - 1].SetTalk(Game.PartyGoers[targets[i]]);
            }
            else
            {
                Game.PartyGoers[targets[i]].SetWander();
            }
        }
    }
}
