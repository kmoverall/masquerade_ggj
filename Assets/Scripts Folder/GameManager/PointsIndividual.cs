using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsIndividual : MonoBehaviour
{
    public Text PointsText;
    int points = 0;
    public int PointTotal {
        get {
            return points;
        }
        set {
            points = value;
            PointsText.text = points.ToString();
        }
    }
    void Start() {
        PointsText.text = points.ToString();
    }
}
