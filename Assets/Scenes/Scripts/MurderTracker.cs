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

    private void Start()
    {
        Game.InteractionHappened += CheckForMurderProgress;
        var murderSteps = _scenario.PrepSteps;
        murderSteps.Shuffle();
        murderSteps.Add(_scenario.BoobyTrap);

        Game.MurderSteps = murderSteps;
    }

    private void CheckForMurderProgress(PartyGoer partyGoer, Transform target)
    {
        if (partyGoer.role != PartyGoer.Role.Killer)
            return;

        if (target == Game.MurderSteps[Game.MurderProgress])
            Game.MurderProgress++;
    }

    private void OnDestroy()
    {
        Game.InteractionHappened -= CheckForMurderProgress;
    }
}
