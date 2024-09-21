using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugManager;

public class SimpleMovementAI : MonoBehaviour
{
    // Variables públicas de velocidad y target
    [Header("Velocidades")]
    public float ContadorDeCuadros; // Contador para saber el número de cuadros renderizados
    public Vector3 Velocity = Vector3.zero; // Velocidad actual del objeto
    protected float PursuitTimePrediction = 0f; // Tiempo de predicción para la persecución
    public float maxSpeed = 5f; // Velocidad máxima del objeto
    public float maxAcceleration = 1f; // Aceleración máxima del objeto

    [Header("Targets")]
    public GameObject targetGameObject; // Objeto objetivo a perseguir

    // Método Start: Se ejecuta cuando el objeto es instanciado
    protected void Start()
    {
        Debug.Log("Se está ejecutando Start. " + gameObject.name); // Imprime el nombre del objeto en la consola

        // Aquí podrías inicializar referencias o valores si es necesario
        // debugConfigManagerRef = GameObject.FindAnyObjectByType<DebugConfigManager>();
        return;
    }

    // Método Update: Se llama una vez por cada cuadro
    public void Update()
    {
        ContadorDeCuadros++; // Aumenta el contador de cuadros

        // Calcula la dirección hacia el objetivo (Seek)
        Vector3 PosToTarget = PuntaMenosCola(targetGameObject.transform.position, transform.position);
        
        // Calcula la velocidad actual del objetivo
        Vector3 currentVelocity = targetGameObject.GetComponent<SimpleMovementAI>().Velocity;
        
        // Calcula el tiempo de predicción basado en la velocidad máxima y la distancia al objetivo
        PursuitTimePrediction = CalculatedPredictedPos(maxSpeed, transform.position, targetGameObject.transform.position);
        
        // Predice la posición futura del objetivo
        Vector3 PredictionPosition = PredictPos(targetGameObject.transform.position, currentVelocity, PursuitTimePrediction);

        // Calcula la fuerza de dirección y la aplica a la velocidad actual
        Velocity += maxAcceleration * Time.deltaTime * PosToTarget.normalized;
        
        // Limita la magnitud de la velocidad para no superar la velocidad máxima
        Velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);
        
        // Actualiza la posición del objeto según la velocidad calculada
        transform.position += Velocity * Time.deltaTime;
    }

    // Función que calcula la diferencia entre dos posiciones (dirección)
    public Vector3 PuntaMenosCola(Vector3 Punta, Vector3 Cola)
    {
        float X = Punta.x - Cola.x;
        float Y = Punta.y - Cola.y;
        float Z = Punta.z - Cola.z;

        return new Vector3(X ,Y, Z); // Devuelve el vector resultante de la resta
    }

    // Función que predice la posición futura de un objeto basándose en su posición inicial, velocidad y tiempo de predicción
    Vector3 PredictPos(Vector3 InitiaPos, Vector3 Velocity, float TimePrediction)
    {
        return InitiaPos + Velocity * TimePrediction; // Retorna la posición futura
    }

    // Función que calcula el tiempo de predicción basado en la distancia entre el objeto y el objetivo
    float CalculatedPredictedPos(float maxSpeed, Vector3 InitialPos, Vector3 TargetPos)
    {
        float Distance = PuntaMenosCola(TargetPos, InitialPos).magnitude; // Calcula la distancia entre las posiciones
        return Distance / maxSpeed; // Calcula el tiempo de predicción basado en la velocidad máxima
    }

    // Función OnDrawGizmos: Dibuja representaciones visuales en la escena para depuración
    public void OnDrawGizmos()
    {
        // Dibuja una línea amarilla que representa la velocidad actual del objeto
        if(DebugGizmoManager.VelocityLines)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + Velocity);
        }

        // Dibuja una línea azul que representa la dirección hacia el objetivo
        if (DebugGizmoManager.DesiredVectors && gameObject != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + (targetGameObject.transform.position - transform.position));
        }

        // Dibuja un cubo negro en la posición predicha del objetivo
        if(targetGameObject != null)
        {
            Vector3 currentVelocity = targetGameObject.GetComponent<SimpleMovementAI>().Velocity;
            Vector3 PredictionPosition = PredictPos(targetGameObject.transform.position, currentVelocity, PursuitTimePrediction);

            Gizmos.color = Color.black;
            Gizmos.DrawCube(PredictionPosition, Vector3.one); // Dibuja un cubo en la posición predicha
        }
    }

    // Método de ejemplo que retorna un entero
    int RetornarInt()
    {
        return 0; // Retorna 0
    }

}
