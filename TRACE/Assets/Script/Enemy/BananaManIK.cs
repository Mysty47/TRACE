using UnityEngine;

public class BananaManIK : MonoBehaviour
{
    protected Animator animator;

    public bool ikActive = true;
    public Transform leftFootTarget;
    public Transform rightFootTarget;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if(animator)
        {
            if(ikActive)
            {
                // LEFT FOOT
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTarget.position);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootTarget.rotation);

                // RIGHT FOOT
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootTarget.position);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootTarget.rotation);
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
            }
        }
    }
}