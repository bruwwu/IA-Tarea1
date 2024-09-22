using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugManager
{
    [CreateAssetMenu(fileName = "DebugGizmoManager", menuName = "Debug/Debug Gizmo Manager")] 
    //Se intentó hacer un scriptable object, pero al realizarlo dentro del inspector los bool al ser static no se van a mostrar a menos que se le haga
    //mas chanchuyo al script, entonces preferimos dejarlo de esta manera :D
    public class DebugGizmoManager : ScriptableObject
    {
        [SerializeField]
        public static bool VelocityLines = true;
        public static bool DesiredVectors = true;
        public static bool DetectionSphere = true;
        public static bool VisionCone = true;
    
    }
}