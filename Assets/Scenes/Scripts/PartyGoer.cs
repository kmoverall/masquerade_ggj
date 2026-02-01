using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;

public class PartyGoer : MonoBehaviour, IPointerClickHandler
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
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SpriteLibrary _spriteLibrary;
    [SerializeField]
    private List<SpriteLibraryAsset> _spriteVariations;

    private bool _hasTarget = false;
    private bool _reachedTarget = true;
    private bool _inspectionComplete = false;

    [SerializeField]
    private float WALK_SPEED = 0.67f;

    [SerializeField]
    private float WANDER_DISTANCE = 1.5f;

    [SerializeField]
    private float WANDER_WAIT_TIME = 5f;
    [SerializeField]
    private float WANDER_WAIT_TIME_VARIANCE = 1f;
    [SerializeField]
    private float INSPECT_TIME = 3f;

    private float _currentWaitTime = 0f;
    private float _targetWaitTime = 0f;

    private int MAX_RAND_ATTEMPTS = 100;

    public bool readyForDirection = true;

    public Transform InspectionTarget { get; private set; }
    public PartyGoer TalkTarget { get; private set; }

    public void SelectVisuals()
    {
        int index = Random.Range(0, _spriteVariations.Count);
        _spriteLibrary.spriteLibraryAsset = _spriteVariations[index];
    }

    public void SetWander()
    {
        CurrentState = State.Wander;
        _targetWaitTime = Random.Range(WANDER_WAIT_TIME - WANDER_WAIT_TIME_VARIANCE, WANDER_WAIT_TIME + WANDER_WAIT_TIME_VARIANCE);
        _currentWaitTime = Random.Range(0f, _targetWaitTime);
        readyForDirection = true;
    }

    public void SetPaused()
    {
        CurrentState = State.Idle;
        readyForDirection = true;
    }

    public void SetInspect(Transform target)
    {
        CurrentState = State.Inspect;
        readyForDirection = false;

        InspectionTarget = target;

        _target.transform.position = InspectionTarget.position;
        _reachedTarget = false;
        _hasTarget = true;
        _inspectionComplete = false;
    }

    public void SetTalk(PartyGoer target)
    {
        readyForDirection = false;
        CurrentState = State.Talk;
        TalkTarget = target;
        var pos = (transform.position + target.transform.position) * 0.5f;
        _target.transform.position = pos;
        _reachedTarget = false;
        _hasTarget = true;
    }

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
            _animator.SetBool("IsInspecting", false);
            _animator.SetBool("IsWalking", false);
            _currentWaitTime += Time.deltaTime;
            return;
        }

        if (!_hasTarget)
        {
            Vector3 point = GetPointAtDistanceAway(WANDER_DISTANCE);
            int attempts = 0;
            while (!Game.Room.IsInside(point) && attempts < MAX_RAND_ATTEMPTS)
            {
                point = GetPointAtDistanceAway(WANDER_DISTANCE);
            }

            if (attempts >= MAX_RAND_ATTEMPTS)
            {
                point = Vector3.zero;
            }

            _target.transform.position = point;
            _hasTarget = true;
            _reachedTarget = false;
        }

        _animator.SetBool("IsInspecting", false);
        _animator.SetBool("IsWalking", true);
        _animator.SetFloat("XVel", _target.transform.position.x - transform.position.x);
        _animator.SetFloat("YVel", _target.transform.position.z - transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, WALK_SPEED * Time.deltaTime);
    }

    private void MoveToTarget()
    {
        if (!_inspectionComplete && _reachedTarget)
        {
            _animator.SetBool("IsWalking", false);
            if (CurrentState == State.Inspect)
                _animator.SetBool("IsInspecting", true);

            if (_currentWaitTime < INSPECT_TIME)
            {
                _currentWaitTime += Time.deltaTime;
                return;
            }
            else
            {
                _inspectionComplete = true;
                if (CurrentState == State.Inspect)
                {
                    _animator.SetBool("IsInspecting", false);
                    Game.InteractionHappened?.Invoke(this, InspectionTarget);
                    SetWander();
                }
                else if (CurrentState == State.Talk)
                {
                    Game.ConversationHappened?.Invoke(this, TalkTarget);
                    SetWander();
                }
                return;
            }
        }

        if (!_hasTarget || _reachedTarget)
            return;

        _animator.SetBool("IsInspecting", false);
        _animator.SetBool("IsWalking", true);
        _animator.SetFloat("XVel", _target.transform.position.x - transform.position.x);
        _animator.SetFloat("YVel", _target.transform.position.z - transform.position.z);
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
        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsInspecting", CurrentState == State.Inspect);
    }

    private Vector3 GetPointAtDistanceAway(float distance)
    {
        Vector2 point2 = Random.insideUnitCircle.normalized * distance;
        Vector3 point3 = transform.position;

        Vector3 result = new();

        result.x = point3.x + point2.x;
        result.z = point3.z + point2.y;

        // Flip the point chosen around until a position is found
        if (!Game.Room.IsInside(result))
        {
            result.x = point3.x - point2.x;
        }
        if (!Game.Room.IsInside(result))
        {
            result.z = point3.z - point2.y;
        }
        if (!Game.Room.IsInside(result))
        {
            result.x = point3.x + point2.x;
        }

        return result;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Game.Camera.IsZoomed)
            return;

        Game.SelectedPartyGoer = this;
        Game.Camera.ZoomOnTarget(transform);
    }
}
