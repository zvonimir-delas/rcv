using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using ZDelas.Rcv.Runtime;

namespace ZDelas.Rcv.Editor
{
    // Methods that define new entries into the Unity Editor menu
    public class RcvMenuButtons
    {
        [MenuItem("RCV/Validate required components")]
        public static void PerformComponentValidation()
        {
            RcvActions.ValidateRequiredComponentsForAllClasses();
        }
    }
}
