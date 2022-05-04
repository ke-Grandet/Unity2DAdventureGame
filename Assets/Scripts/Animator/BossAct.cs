using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAct : StateMachineBehaviour
{

    private const string ATTACK = "boss_attack";

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    // 在动画开始时调用
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 在攻击动画开始时
        if (stateInfo.IsName(ATTACK))
        {
            // 调用攻击方法
            animator.gameObject.GetComponent<BossController>().Attack();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // 在动画开始和动画结束之间的每一帧被调用
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // 在动画结束时被调用
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 在攻击动画结束后
        if (stateInfo.IsName(ATTACK))
        {
            // 进行移动
            animator.gameObject.GetComponent<BossController>().RandomAct(0);
        }
    }


    // 下面两个用不到

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
