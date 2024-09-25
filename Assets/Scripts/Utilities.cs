using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utilities
{
    public class Utility
    {
        public static bool IsInsideRadius(Vector3 inTargetPos, Vector3 inSpherePos, float inSphereRadius)
        {
            // Para saber si un punto en el espacio (llamado TargetPos) está dentro o fuera de una esfera en el espacio,
            // hacemos un vector que inicia en el origen de la esfera y que termine en TargetPos (punta menos cola)
            Vector3 AgentPositionToTarget = inTargetPos - inSpherePos;
            // Y luego obtenemos la magnitud de dicho vector
            float VectorMagnitude = AgentPositionToTarget.magnitude;
            // y finalmente comparamos esa magnitud contra el radio de la esfera.
            // (usamos operadores de comparación: == , !=  , > < <= >=...)
            // Si el radio es mayor o igual que la magnitud de ese vector, entonces TargetPos está dentro de la esfera,
            if (inSphereRadius >= VectorMagnitude)
            {
                return true;
            }
            // de lo contrario, está fuera de la esfera
            else
            {
                return false;
            }
        }

        // El gizmo junto con el Vector3 se pasó a utilites.
        // Por ser una clase static, se le deben asignar los valores por defecto a cada una de estas clases
        // En este caso DrawVisionCone utiliza:
        // Vector3: El cual brindará la posición del objeto al método
        // Transform agentTransform: para dar un mejor contexto, se ha cambiado el nombre quitando "amogoTransform"
        public static void DrawVisionCone(Vector3 origin, float angle, float distance, bool isDetected, Transform agentTransform)
        {
            if (angle <= 0f) return;

            float halfFovAngle = angle / 2;

            Vector3 p1, p2;

            p1 = agentTransform.TransformDirection(visionCone(halfFovAngle, distance));
            p2 = agentTransform.TransformDirection(visionCone(-halfFovAngle, distance));

            Gizmos.color = isDetected ? Color.green : Color.red;
            Gizmos.DrawLine(origin, origin + p1);
            Gizmos.DrawLine(origin, origin + p2);
        }

        private static Vector3 visionCone(float angle, float distance)
        {
            // Mantiene la misma lógica, solamente que ahora es una clase static (esto por estar en Utilities).
            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)) * distance;
        }

        // Función de Cono de Visión.
        public static bool IsInsideCone(Vector3 inTargetPos, Vector3 inAmogusPos, Vector3 coneDirection, float inConeAngle, float inConeDistance)
        {
            // Hacemos un vector que inicia en el origen del amogus y que termine en TargetPos (punta menos cola).
            Vector3 playerVector = inTargetPos - inAmogusPos;

            // Se calcula el ángulo que hay entre la dirección del cono y el jugador.
            float angleToTarget = Vector3.Angle(playerVector.normalized, coneDirection.normalized);

            // Se verifica si el objetivo se encuentra en el ángulo del cono.
            if (angleToTarget < inConeAngle * 0.5f)
            {
                // Se verifica si el objetivo se encuentra dentro del rango de distancia del cono.
                if (playerVector.magnitude < inConeDistance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
