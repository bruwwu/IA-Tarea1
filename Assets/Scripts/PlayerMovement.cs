using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using DebugManager;

public class PlayerMovement : MonoBehaviour
{
    [Header("Velocidad")]
    public float MoveSpeed;
    private CharacterController characterController;

    [Header("VisionCone")]
    public Transform detectedObject;
    public Transform coneUser;
    public float fovAngle = 15f;
    public float visionDistance = 10f;
    
    bool isDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); 
        
        characterController.Move(move * Time.deltaTime * MoveSpeed);
        
    }

     void OnDrawGizmos()
    {
        if(DebugGizmoManager.VisionCone){ //Ya todo probado, si se apaga VisionCone del debug no se ejecuta :p
            Utility.DrawVisionCone(coneUser.position, fovAngle, visionDistance, isDetected, transform); //Basicamente los valores que se muestran en el insepctor, angulo y ditancia :D
        }
    }
}
