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

        // Función de Cono de Visión.
        public static bool IsInsideCone(Vector3 inTargetPos, Vector3 inAmogusPos, Vector3 coneDirection, float inConeAngle, float inConeDistance)
        {
             // Hacemos un vector que inicia en el origen del amogus y que termine en TargetPos (punta menos cola)
            Vector3 playerVector = inTargetPos - inAmogusPos;

            //Se calcula el angula que hay entre la direccion del cono y del jugador
            float angleToTarget = Vector3.Angle(playerVector.normalized, coneDirection.normalized);
            
            //Se verifica si el objetivo se encuentra en el angulo del cono 
            if(angleToTarget < inConeAngle * 0.5f)
            {

                //Se verifica si el objetivo se encuentra en el angulo del cono 
                if(playerVector.magnitude < inConeDistance)
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