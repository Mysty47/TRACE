using System;
using UnityEngine;

public class InstructionsController : MonoBehaviour
{
    [Header("Settings")]
    public bool shouldShowAtStart = false;
    public float timeToHide = 5f;
    
    [Header("Animator")]
    private Animator textAnimator;
    
    [Header("Constants")]
    private const string animationNameShowText = "ShowText";
    private const string animationNameHideText = "HideText";

    private void Start()
    {
        textAnimator = GetComponent<Animator>();
        if (textAnimator != null && shouldShowAtStart)
        {
            ShowText();
            Invoke(nameof(HideText), timeToHide);
        }
    }

    public void ShowText()
    {
        if(textAnimator != null)
            textAnimator.SetTrigger(animationNameShowText);
    }

    public void HideText()
    {
        if(textAnimator != null)
            textAnimator.SetTrigger(animationNameHideText);
    }
}
