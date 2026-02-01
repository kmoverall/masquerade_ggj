using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Accumulate player's points
//Display the points to a UI object

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;

    //Assume first individual is always the Player
    public PointsIndividual[] individuals;

    //static-one shared copy of all instances of the variable

    //references to our win/lose text game objects

    void Start()
        {
            Instance = this;
        }

    // Start is called before the first frame update

    

    public void AddPoints(int pointValue, int individual)
    {
        Debug.LogFormat("Individual {0} got {1} points", individual, pointValue);
        individuals[individual].PointTotal += pointValue;
    }

    
    public bool DidPlayerWin() {
        
        if (individuals[0].PointTotal > individuals[1].PointTotal) {
            return true;
        }
        else {
            return false;
        }
    }

   

    

}
