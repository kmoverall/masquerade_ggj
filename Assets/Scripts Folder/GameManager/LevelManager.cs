using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MonoBehavior allows us to attach a script to a SCENE object.
//    -which means it gets deleted when loading into another scene

//It is not a MonoBehavior.  So it is not attached to anything
//    -and it is not destroyed.



public class LevelManager 
{
   //Constructor: a function used to initialize an instance of a class
   LevelManager() {
       
   }

   //static variable
   public static List<string> levelsBeaten;

    //static constructor
    static LevelManager() {
        levelsBeaten = new List<string>();
    }

}
