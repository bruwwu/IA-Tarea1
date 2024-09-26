using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class VisionConeSB_Player : MonoBehaviour
{

     [Header("VisionCone_SteeringDam")]
    public Transform amogo;
    public Transform player;
    public float fovAngle = 15f;
    public float visionDistance = 10f;
    
    bool isDetected = false;

    Vector3 visionCone(float angle, float distance)
    {
        // Cambiamos a Vector3 para trabajar en 3D, ya que vector2 no interactua de la manera ideal en un entorno 3D
        //Solo se ha agregado le valor 0 (esto arregla un problema que habia con la rotacion)
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)) * distance;
    }

    void OnDrawGizmos()
    {
        if (fovAngle <= 0f) return;

        float halfFovAngle = fovAngle / 2;

        Vector3 p1, p2;

        // Calculamos el cono de visión en 3D aplicando la rotación del agente
        p1 = transform.TransformDirection(visionCone(halfFovAngle, visionDistance));
        p2 = transform.TransformDirection(visionCone(-halfFovAngle, visionDistance));

        Gizmos.color = isDetected ? Color.green : Color.red;
        Gizmos.DrawLine(player.position, player.position + p1);
        Gizmos.DrawLine(player.position, player.position + p2);
    }
    
    void Update()
    {
        Vector3 playerDirection = transform.forward; // Aqui calculamos el frente del Amogus, a donde esta yendo
        isDetected = false;
        // Revisamos si el agente está dentro del radio de tolerancia del objetivo actual
        if (Utilities.Utility.IsInsideCone(amogo.position, player.position, playerDirection, fovAngle, visionDistance))
        {
            isDetected = true;
        }
        
    }
}
