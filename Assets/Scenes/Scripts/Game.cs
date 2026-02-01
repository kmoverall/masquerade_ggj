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
    public static int Strikes = 0;
    public static PartyGoer SelectedPartyGoer;
    public static UIManager UI;

    public static CameraController Camera;

    public static Action StartGame;
    public static Action<PartyGoer> AccusationMade;
    public static Action AccusationSuccess;
    public static Action AccusationFailed;
    public static Action StrikeOut;
    public static Action MurderOccured;
    public static Action<PartyGoer, Transform> InteractionHappened;
    public static Action<PartyGoer, PartyGoer> ConversationHappened;
    public static Action ZoomInComplete;
    public static Action ZoomOutStarted;
}
