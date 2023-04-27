using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using ZDelas.Rcv.Runtime;
using System.Collections.Generic;

namespace ZDelas.Rcv.Editor
{
    // Static helpers for component validation
    public static class RcvHelpers
    {
        // Get all attributes of type RequiredCustomComponent on specified class
        private static List<RequiredCustomComponent> GetRequiredComponentsForClass(Type classType)
        {
            var listOfRequiredComponents =
                // Get all attributes of type RequiredCustomComponent on specified class
                classType.GetCustomAttributes(typeof(RequiredCustomComponent), true)
                // Cast each attribute to RequiredCustomComponent
                .Select(component => (RequiredCustomComponent)component)
                // Convert from object[] to List<RequiredCustomComponent>
                .ToList();

            return listOfRequiredComponents;
        }

        // Verify that the specified game object contains all the required components for the specified class
        public static bool VerifyRequiredComponentsForClass(GameObject gameObjectWithComponents, Type controllerType)
        {
            var listOfRequiredComponents = GetRequiredComponentsForClass(controllerType);

            foreach (var component in listOfRequiredComponents)
            {
                var componentType = component.componentType;
                var attachedComponent = gameObjectWithComponents.GetComponent(componentType);

                if (attachedComponent == null)
                {
                    Debug.LogError($"Missing required component {componentType} on {gameObjectWithComponents.name}");
                    return false;
                }
            }

            Debug.Log($"Verified all components ({listOfRequiredComponents.Count}) on game object {gameObjectWithComponents.name} with {controllerType}");
            return true;
        }
    }

}
