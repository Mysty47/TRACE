using UnityEngine;
using UnityEditor;


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
        if(animator && ikActive)
        {
            // LEFT FOOT
            Vector3 leftFootPos = GetFootPosition(leftFootTarget);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootTarget.rotation);

            // RIGHT FOOT
            Vector3 rightFootPos = GetFootPosition(rightFootTarget);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootTarget.rotation);
        }
    }

    
    Vector3 GetFootPosition(Transform footTarget)
    {
        RaycastHit hit;
        Vector3 origin = footTarget.position + Vector3.up * 0.5f; // започваме малко над крака
        if(Physics.Raycast(origin, Vector3.down, out hit, 2f))
        {
            // Връщаме точката на терена
            return hit.point;
        }
        else
        {
            // Ако няма терен под крака, връщаме текущата позиция
            return footTarget.position;
        }
    }

}