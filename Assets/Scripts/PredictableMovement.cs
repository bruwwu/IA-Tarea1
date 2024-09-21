using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PredictableMovement : SimpleMovementAI
{
    // Array de objetos a los que el agente se moverá en secuencia
    [SerializeField] public GameObject[] pawPatrol; 
    
    // Índice que rastrea el objetivo actual del patrullaje
    private int currentPawPatrol = 0; 
    
    // Radio de tolerancia que define cuándo el agente ha llegado suficientemente cerca al objetivo
    private float pawPatrolToleranceRadius = 2f; 

    // Método Start: Se llama al inicio, pero aquí no tiene lógica adicional
    void Start()
    {
        // Podría inicializar valores o referencias si fuera necesario
    }

    // Método Update: Se llama una vez por cuadro
    void Update()
    {
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
    }
}
