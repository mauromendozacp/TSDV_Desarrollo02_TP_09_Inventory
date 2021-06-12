using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] Transform player;

    [SerializeField] Vector3 circleOfMovement; 
    Vector3 targetPos;

    float stateTimer;
    float minDistanceFromPlayer = 5f;    

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

    bool IsPlayerClose()
    {
        return Vector3.Distance(transform.position, player.position) < minDistanceFromPlayer;
    }

    void SetIdle()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer < 0)
        {
            targetPos = circleOfMovement + new Vector3(Random.Range(1, circleOfMovement.y), 0, Random.Range(1, circleOfMovement.y));
            targetPos.y = transform.position.y;

            direction = (targetPos - transform.position).normalized;

            actualState = State.Walking;
            stateTimer = Random.Range(2, 5);
        }

        if (IsPlayerClose())
        {
            actualState = State.FollowingPlayer;
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
            actualState = State.Idle;
            stateTimer = Random.Range(2, 5);
        }
        else
        {
            SetMovement(transform.forward, direction);
        }

        if (IsPlayerClose())
        {
            actualState = State.FollowingPlayer;
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
            actualState = State.Idle;
            stateTimer = Random.Range(2, 5);
        }        
    }
}
