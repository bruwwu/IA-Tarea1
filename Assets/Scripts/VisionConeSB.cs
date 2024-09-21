using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class VisionConeSB : SimpleMovementAI
{

     [Header("VisionCone")]
    public float fovAngle = 15f;
    public Transform amogo;
    public float visionDistance = 10f;
    bool isDetected = false;
    [Header("PawPatrol")] 
    public GameObject[] pawPatrol; 
    // Índice que rastrea el objetivo actual del patrullaje
    private int currentPawPatrol = 0; 
    // Radio de tolerancia que define cuándo el agente ha llegado suficientemente cerca al objetivo
    private float pawPatrolToleranceRadius = 3f; 


   void OnDrawGizmos()
{
    if (fovAngle <= 0f) return;

    float halfFovAngle = fovAngle / 2;

    Vector3 p1, p2;

    // Calculamos el cono de visión en 3D aplicando la rotación del agente
    p1 = transform.TransformDirection(visionCone(halfFovAngle, visionDistance));
    p2 = transform.TransformDirection(visionCone(-halfFovAngle, visionDistance));

    Gizmos.color = Color.green;
    Gizmos.DrawLine(amogo.position, amogo.position + p1);
    Gizmos.DrawLine(amogo.position, amogo.position + p2);
}

Vector3 visionCone(float angle, float distance)
{
    // Cambiamos a Vector3 para trabajar en 3D, ya que vector2 no interactua de la manera ideal en un entorno 3D
    //Solo se ha agregado le valor 0 (esto arregla un problema que habia con la rotacion)
    return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)) * distance;
}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isDetected = false;
        
         // Definir un área de aceptación para saber si el agente ha llegado al objetivo
        // Revisamos si el agente está dentro del radio de tolerancia del objetivo actual (patrullaje)
        if (Utilities.Utility.IsInsideRadius(pawPatrol[currentPawPatrol].transform.position, transform.position, pawPatrolToleranceRadius))
        {
            // Cambia al siguiente objetivo en el array
            currentPawPatrol++;
            
            // Usamos el operador módulo para ciclar el índice, volviendo al primer objetivo al alcanzar el final
            currentPawPatrol %= pawPatrol.Length; 
            
            // Ejemplo del funcionamiento de % con pawPatrol.Length = 4:
            // 0 % 4 = 0
            // 1 % 4 = 1
            // 2 % 4 = 2
            // 3 % 4 = 3
            // 4 % 4 = 0 (regresa al primer objetivo)

            /*
            Alternativa: Ciclar usando una condición if
            if(currentPawPatrol >= pawPatrol.Length)
            {
                currentPawPatrol = 0; // Reinicia el índice si llega al final
            }
            */
        }
        

        // Calculamos la dirección hacia el objetivo actual
        Vector3 PosToTarget = PuntaMenosCola(pawPatrol[currentPawPatrol].transform.position, transform.position);
        
        // Aumentamos la velocidad del agente hacia el objetivo, respetando la aceleración máxima
        Velocity += maxAcceleration * Time.deltaTime * PosToTarget.normalized;
        
        // Limitamos la velocidad para que no exceda el valor máximo permitido
        Velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);
        
        // Actualizamos la posición del agente en función de la velocidad calculada
        transform.position += Velocity * Time.deltaTime;

    if (PosToTarget.magnitude > 0.1f) // Solo rotar si el objetivo no está muy cerca
        {
            Quaternion targetRotation = Quaternion.LookRotation(PosToTarget); // Calcula la rotación hacia el objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Rotación suave
        }
    }
}
