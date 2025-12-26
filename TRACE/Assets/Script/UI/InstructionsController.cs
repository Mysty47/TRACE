using System;
using UnityEngine;

public class InstructionsController : MonoBehaviour
{
    [Header("Settings")]
    public bool shouldShowAtStart = false;
    
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
            Invoke(nameof(HideText), 5f);
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
