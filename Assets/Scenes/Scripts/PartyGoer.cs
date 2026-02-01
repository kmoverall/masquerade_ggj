using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PartyGoer : MonoBehaviour
{
    public enum Role
    {
        None,
        Killer,
        Victim
    }

    public enum State
    {
        Idle,
        Wander,
        Inspect,
        Talk
    }

    public Role role = Role.None;
    public State CurrentState { get; private set; }

    [SerializeField]
    private CapsuleCollider _target;

    private bool _hasTarget = false;
    private bool _reachedTarget = true;

    [SerializeField]
    private float WALK_SPEED = 0.67f;

    [SerializeField]
    private float WANDER_DISTANCE = 1.5f;

    [SerializeField]
    private float WANDER_WAIT_TIME = 5f;
    [SerializeField]
    private float WANDER_WAIT_TIME_VARIANCE = 1f;

    private float _currentWaitTime = 0f;
    private float _targetWaitTime = 0f;

    public Transform InspectionTarget { get; private set; }

    public void SetWander()
    {
        CurrentState = State.Wander;
        _targetWaitTime = Random.Range(WANDER_WAIT_TIME - WANDER_WAIT_TIME_VARIANCE, WANDER_WAIT_TIME + WANDER_WAIT_TIME_VARIANCE);
        _currentWaitTime = Random.Range(0f, _targetWaitTime);
    }
    public void SetPaused()
    {
        CurrentState = State.Idle;
    }
    public void SetInspect(Transform target)
    {
        CurrentState = State.Inspect;
        InspectionTarget = target;

        _target.transform.position = target.position;
        _reachedTarget = false;
        _hasTarget = true;
    }
    public void SetTalk(PartyGoer target)
    {
        CurrentState = State.Talk;
        var pos = (transform.position + target.transform.position) * 0.5f;
        _target.transform.position = pos;
        _reachedTarget = false;
        _hasTarget = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case State.Wander:
                Wander();
                break;
            case State.Inspect:
            case State.Talk:
                MoveToTarget();
                break;
            case State.Idle:
                break;
        }
    }

    private void Wander()
    {
        if (_currentWaitTime < _targetWaitTime)
        {
            _currentWaitTime += Time.deltaTime;
            return;
        }

        if (!_hasTarget)
        {
            Vector3 point = GetPointAtDistanceAway(WANDER_DISTANCE);
            while (!Game.Room.IsInside(point))
            {
                point = GetPointAtDistanceAway(WANDER_DISTANCE);
            }

            _target.transform.position = point;
            _hasTarget = true;
            _reachedTarget = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, WALK_SPEED * Time.deltaTime);
    }

    private void MoveToTarget()
    {
        if (!_hasTarget || _reachedTarget)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, WALK_SPEED * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != _target || !_hasTarget)
            return;

        _currentWaitTime = 0;
        _targetWaitTime = Random.Range(WANDER_WAIT_TIME - WANDER_WAIT_TIME_VARIANCE, WANDER_WAIT_TIME + WANDER_WAIT_TIME_VARIANCE);
        _reachedTarget = true;
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
