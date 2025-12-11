using System;
using UnityEngine;

public class InstructionsController : MonoBehaviour
{
    [Header("Animator")]
    private Animator textAnimator;
    
    [Header("Constants")]
    private const string animationNameShowText = "ShowText";
    private const string animationNameHideText = "HideText";

    private void Start()
    {
        textAnimator = GetComponent<Animator>();
    }

    public void ShowText()
    {
        textAnimator.SetTrigger(animationNameShowText);
    }

    public void HideText()
    {
        textAnimator.SetTrigger(animationNameHideText);
    }
}
