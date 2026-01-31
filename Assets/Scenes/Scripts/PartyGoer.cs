using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PartyGoer : MonoBehaviour
{
    public enum CurrentState
    {
        Wander,
        Inspect,
        Talk,
        Interrogate
    }
    private CurrentState _state;
    [SerializeField]
    private CapsuleCollider _target;

    private bool _hasTarget = false;
    private bool _reachedTarget = false;

    private const float WANDER_DISTANCE = 1.5f;
    private const float WANDER_SPEED = 0.67f;
    private const float WANDER_WAIT_TIME = 2f;

    public void SetWander()
    {
        _state = CurrentState.Wander;
    }
    public void SetInspect(Transform target)
    {
        _state = CurrentState.Inspect;
        _target.transform.position = target.position;
    }
    public void SetTalk(PartyGoer target)
    {
        _state = CurrentState.Talk;
        var pos = (transform.position + target.transform.position) * 0.5f;
        _target.transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case CurrentState.Wander:
                Wander();
                break;
            case CurrentState.Inspect:
            case CurrentState.Talk:
                MoveToTarget();
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

    private void MoveToTarget()
    {
        if (!_hasTarget || _reachedTarget)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, WANDER_SPEED * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != _target || !_hasTarget)
            return;

        _reachedTarget = true;

        if (_state == CurrentState.Wander)
            StartCoroutine(DelayRetarget(WANDER_WAIT_TIME));
        else
            _hasTarget = false;
    }

    private IEnumerator DelayRetarget(float delay)
    {
        yield return new WaitForSeconds(delay);
        _reachedTarget = false;
        _hasTarget = false;
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
