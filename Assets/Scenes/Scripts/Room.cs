using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public Bounds Bounds;

    public Vector3 RandomPointInside => new Vector3(
        Random.Range(Bounds.min.x, Bounds.max.x),
        Random.Range(Bounds.min.y, Bounds.max.y),
        Random.Range(Bounds.min.z, Bounds.max.z)
    );

    public bool IsInside(Vector3 point)
    {
        return Bounds.Contains(point);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3[] corners = new Vector3[4]
        {
            new Vector3(Bounds.min.x, 0.1f, Bounds.min.z), 
            new Vector3(Bounds.min.x, 0.1f, Bounds.max.z),
            new Vector3(Bounds.max.x, 0.1f, Bounds.max.z),
            new Vector3(Bounds.max.x, 0.1f, Bounds.min.z)
        };
        Gizmos.DrawLineStrip(corners, true);
    }
}
