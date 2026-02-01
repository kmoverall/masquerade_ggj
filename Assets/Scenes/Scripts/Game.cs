using UnityEngine;
using System.Collections.Generic;
using System;

public static class Game
{
    public static Room Room;
    public static Spawner Spawner;
    public static List<PartyGoer> PartyGoers = new();
    public static PartyGoer Killer;
    public static PartyGoer Victim;
    public static Director Director;
    public static List<Transform> Interactables;
    public static MurderTracker MurderTracker;
    public static List<Transform> MurderSteps = new();
    public static int MurderProgress = 0;

    public static Action<PartyGoer, Transform> InteractionHappened;
}
