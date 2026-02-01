using UnityEngine;
using System.Collections.Generic;

public static class Game
{
    public static Room Room;
    public static Spawner Spawner;
    public static List<PartyGoer> PartyGoers = new();
    public static Director Director;
    public static List<Transform> Interactables;
}
