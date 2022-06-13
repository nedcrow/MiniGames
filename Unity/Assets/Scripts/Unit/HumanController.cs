using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public enum States
{
    Spawn,
    Idle,
    Walk,
    Death
}

public class HumanController : MonoBehaviour
{
    public AnimationClip spawnAnimation;
    public AnimationClip idleAnimation;
    public AnimationClip walkAnimation;

    Vector3 firstPos = new Vector3(-0.2f, -1.2f, -4.2f);//-20~20, -3f+y;

    private StateMachine<States> FSM;
    void Awake()
    {
        FSM = new StateMachine<States>(this);

        FSM.ChangeState(States.Spawn);
    }

    protected virtual IEnumerator Spawn_Enter() {
        var duration = 1;
        yield return new WaitForSeconds(duration); 
        FSM.ChangeState(States.Idle);
        Debug.Log("Spawn");
    }

    protected virtual void Idle_Enter()
    {
        Debug.Log("Idle");
        GetComponent<Animator>().Play(idleAnimation.name);
    }

    protected virtual void Idle_FixedUpdate()
    {
        MovementComponent3D movement = transform.parent.GetComponent<MovementComponent3D>();
        if (movement != null)
        {
            Debug.Log(movement.currentSpeed);
            if (movement.currentSpeed > 0)
            {
                FSM.ChangeState(States.Walk);
            }
        }
    }

    protected virtual void Walk_Enter()
    {
        Debug.Log("Walk");
        GetComponent<Animator>().Play(walkAnimation.name);
    }
    protected virtual void Walk_Update()
    {
        MovementComponent3D movement = transform.parent.GetComponent<MovementComponent3D>();
        if (movement != null)
        {
            Debug.Log(movement.currentSpeed);
            if (movement.currentSpeed <= 0)
            {
                FSM.ChangeState(States.Idle);
            }
        }
    }
}
