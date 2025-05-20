using System;
using UnityEngine;

public class Weapon_Sway : MonoBehaviour {

    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float multiplier;
    public GrapplingGun grapple;

    private void Update()
    {
        if (!grapple.isGrappled)
        {
           // get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;
           
            // calculate target rotation
            Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
           
            Quaternion targetRotation = rotationX * rotationY;
           
            // rotate 
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime); 
        }
        
    }
}