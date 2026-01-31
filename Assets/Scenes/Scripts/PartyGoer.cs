using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PartyGoer : MonoBehaviour
{
    public enum CurrentState
    {
        Wander
    }
    private CurrentState _state;
    [SerializeField]
    private Collider _target;

    private bool _hasTarget = false;

    private const float WANDER_DISTANCE = 1f;
    private const float WANDER_SPEED = 0.67f;
    private const float WANDER_WAIT_TIME = 1.5f;

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case CurrentState.Wander:
                Wander();
                break;
        }
    }

    private void Wander()
    {
        if (!_hasTarget)
        {
            Vector3 point = GetPointAtDistanceAway(WANDER_DISTANCE);
            while (!Game.Room.IsInside(point))
            {
                point = GetPointAtDistanceAway(WANDER_DISTANCE);
            }

            _target.transform.position = point;
            _hasTarget = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, WANDER_SPEED * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != _target || !_hasTarget)
            return;

        StartCoroutine(DelayRetarget(WANDER_WAIT_TIME));
    }

    private IEnumerator DelayRetarget(float delay)
    {
        Debug.Log("Reached Target");
        yield return new WaitForSeconds(delay);
        _hasTarget = false;
        Debug.Log("Retargeting");
    }

    private Vector3 GetPointAtDistanceAway(float distance)
    {
        Vector2 point2 = Random.insideUnitCircle.normalized * distance;
        Vector3 point3 = transform.position;

        point3.x += point2.x;
        point3.z += point2.y;

        return point3;
    }
}
