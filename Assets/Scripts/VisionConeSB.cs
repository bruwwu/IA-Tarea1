using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using DebugManager;

public class VisionConeSB : SimpleMovementAI
{
    public Rigidbody rb;

     [Header("VisionCone_SteeringDam")]
    public Transform player;
    public Transform amogo;
    public float fovAngle = 15f;
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
        if(DebugGizmoManager.VisionCone){ //Ya todo probado, si se apaga VisionCone del debug no se ejecuta :p
            Utility.DrawVisionCone(amogo.position, fovAngle, visionDistance, isDetected, transform); //Basicamente los valores que se muestran en el insepctor, angulo y ditancia :D
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 amogusDirection = transform.forward; // Aqui calculamos el frente del Amogus, a donde esta yendo
        isDetected = false;
        // Revisamos si el agente está dentro del radio de tolerancia del objetivo actual
        if (Utilities.Utility.IsInsideCone(player.position, amogo.position, amogusDirection, fovAngle, visionDistance))
        {
            isDetected = true;
            Vector3 PosToTarget = PuntaMenosCola(player.transform.position, transform.position); // SEEK

        // Force o Acceleration nos dan lo mismo ahorita porque no vamos a modificar la masa.
            rb.AddForce(PosToTarget.normalized * maxAcceleration, ForceMode.Force);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            
        }
        
        
        if(!isDetected)
        {
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
        
}
