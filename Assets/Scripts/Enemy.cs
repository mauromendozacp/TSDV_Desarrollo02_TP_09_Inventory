using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public Transform player;

    Animator anim;

    public Vector3 circleOfMovement;
    Vector3 targetPos;

    float stateTimer;
    float minDistanceFromPlayer = 8f;

    public delegate void OnPlayerTouchedDelegate();
    public OnPlayerTouchedDelegate onPlayerTouched;

    public delegate void OnDieDelegate();
    public OnDieDelegate onDie;

    public enum State
    {
        Idle,
        Walking,
        FollowingPlayer
    }

    State actualState;

    void Start()
    {
        actualState = State.Idle;
        stateTimer = Random.Range(1, 4);

        moveSpeed = 4;

        anim = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        switch (actualState)
        {
            case State.Idle:
                SetIdle();
                break;
            case State.Walking:
                SetWalking();
                break;
            case State.FollowingPlayer:
                SetFollowingPlayer();
                break;
            default:
                break;
        }
    }

    void ResetTimer()
    {
        stateTimer = Random.Range(1, 4);
    }

    bool IsPlayerClose()
    {
        return Vector3.Distance(transform.position, player.position) < minDistanceFromPlayer;
    }

    void ChangeState(State newState, float animSpeed)
    {
        actualState = newState;
        ResetTimer();

        anim.SetFloat("Speed", animSpeed);

        if (newState == State.FollowingPlayer)
        {
            anim.SetBool("ChasingPlayer", true);
        }
        else
        {
            anim.SetBool("ChasingPlayer", false);
        }
    }

    void SetIdle()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer < 0)
        {
            targetPos = circleOfMovement + new Vector3(Random.Range(-circleOfMovement.y, circleOfMovement.y), 0, 
                                                       Random.Range(-circleOfMovement.y, circleOfMovement.y));
            targetPos.y = transform.position.y;

            direction = (targetPos - transform.position).normalized;

            ChangeState(State.Walking, 1f);
        }

        if (IsPlayerClose())
        {
            ChangeState(State.FollowingPlayer, 0f);
        }
    }

    bool IsCloseToMinimunPos()
    {
        return Vector3.Distance(transform.position, targetPos) < 0.1f;
    }

    void SetWalking()
    {
        if (IsCloseToMinimunPos())
        {
            ChangeState(State.Idle, 0f);
        }
        else
        {
            SetMovement(transform.forward, direction);
        }

        if (IsPlayerClose())
        {
            ChangeState(State.FollowingPlayer, 0f);
        }
    }

    void SetFollowingPlayer()
    {
        if (IsPlayerClose())
        {
            SetMovement(direction, (player.position - transform.position).normalized);
            direction = (player.position - transform.position).normalized;            
        }
        else
        {
            ChangeState(State.Idle, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            onPlayerTouched?.Invoke();
        }
    }
}
