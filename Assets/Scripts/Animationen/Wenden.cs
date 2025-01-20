using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wenden : StateMachineBehaviour
{
    public GameEvent bewegungAbgeschlossen;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Vector3 position = animator.gameObject.transform.position;

        //90° Drehung
        animator.gameObject.transform.Rotate(0f, 0f, 180f);
        //Debug.Log("Position vor Bewegung: " + position);
        //Halbe Länge addieren
        position = position +
            (animator.gameObject.transform.up *
            (animator.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2f));
        animator.gameObject.transform.position = position;
        animator.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        bewegungAbgeschlossen.TriggerEvent();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion

    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
