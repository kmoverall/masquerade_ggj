using UnityEngine;
using System.Collections.Generic;
using System;

public class MurderTracker : MonoBehaviour
{
    public enum MurderMethod
    {
        Chandelier,
        Poison
    }

    [Serializable]
    public struct MurderScenario
    {
        public List<Transform> PrepSteps;
        public Transform BoobyTrap;
        public MurderMethod MurderMethod;
    }

    [SerializeField]
    private List<MurderScenario> _scenarios;

    private MurderScenario _scenario;

    private void Start()
    {
        _scenario = _scenarios.GetRandom();
        Game.MurderMethod = _scenario.MurderMethod;
        Game.InteractionHappened += CheckForMurderProgress;
        Game.ConversationHappened += CheckForMurderProgress;
        Game.AccusationMade += AccuseTarget;
    }
    
    public void SetupMurder()
    {
        var murderSteps = _scenario.PrepSteps;
        murderSteps.Shuffle();
        murderSteps.Add(_scenario.BoobyTrap);
        murderSteps.Add(Game.Victim.transform);
        murderSteps.Add(_scenario.BoobyTrap);

        Game.MurderSteps = murderSteps;
    }

    public bool IsPreparing
    {
        get {
            return Game.MurderProgress < Game.MurderSteps.Count - 2;
        }
    }
    public bool IsVictimNext
    {
        get {
            return Game.MurderProgress == Game.MurderSteps.Count - 2;
        }
    }
    public bool IsKillNext
    {
        get {
            return Game.MurderProgress == Game.MurderSteps.Count - 1;
        }
    }

    public Transform NextTarget
    {
        get {
            if (Game.MurderProgress < Game.MurderSteps.Count)
                return Game.MurderSteps[Game.MurderProgress];

            return null;
        }
    }

    private void CheckForMurderProgress(PartyGoer partyGoer, Transform target)
    {
        if (!((partyGoer.role == PartyGoer.Role.Killer && !IsKillNext) ||
            (partyGoer.role == PartyGoer.Role.Victim && IsKillNext)))
            return;


        if (target == Game.MurderSteps[Game.MurderProgress])
            Game.MurderProgress++;
    }
    private void CheckForMurderProgress(PartyGoer partyGoer, PartyGoer target)
    {
        if (partyGoer.role != PartyGoer.Role.Killer)
            return;

        if (target.transform == Game.MurderSteps[Game.MurderProgress])
            Game.MurderProgress++;
    }

    private void AccuseTarget(PartyGoer target)
    {
        if (target == Game.Killer)
        {
            Game.AccusationSuccess?.Invoke();
        }
        else
        {
            Game.Strikes++;
            Game.AccusationFailed?.Invoke();
            if (Game.Strikes >= 3)
            {
                Game.StrikeOut?.Invoke();
            }
        }
    }

    private void OnDestroy()
    {
        Game.InteractionHappened -= CheckForMurderProgress;
        Game.ConversationHappened -= CheckForMurderProgress;
        Game.AccusationMade -= AccuseTarget;
    }
}
