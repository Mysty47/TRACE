using System;
using UnityEngine;

public class InstructionsController : MonoBehaviour
{
    [Header("Animator")]
    private Animator textAnimator;

    private void Start()
    {
        textAnimator = GetComponent<Animator>();
    }

    public void ShowText()
    {
        textAnimator.SetTrigger("ShowText");
    }

    public void HideText()
    {
        textAnimator.SetTrigger("HideText");
    }
}
