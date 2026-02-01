using UnityEngine;
using System.Collections.Generic;
using System;

public class MurderTracker : MonoBehaviour
{
    [Serializable]
    public struct MurderScenario
    {
        public List<Transform> PrepSteps;
        public Transform BoobyTrap;
    }

    [SerializeField]
    private MurderScenario _scenario;

    private Dictionary<Transform, bool> _checklist = new();

    private void Start()
    {
        Game.InteractionHappened += CheckForMurderProgress;
        foreach (var t in _scenario.PrepSteps)
        {
            _checklist.Add(t, false);
        }
    }

    private void CheckForMurderProgress(PartyGoer partyGoer, Transform target)
    {
        if (partyGoer.role != PartyGoer.Role.Killer)
            return;

        if (_scenario.PrepSteps.Contains(target))
        {
            _checklist[target] = true;
        }
    }

    private void OnDestroy()
    {
        Game.InteractionHappened -= CheckForMurderProgress;
    }
}
