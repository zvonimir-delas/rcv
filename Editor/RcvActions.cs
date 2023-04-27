using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using ZDelas.Rcv.Runtime;

namespace ZDelas.Rcv.Editor
{
    // Actions triggered by menu buttons
    public static class RcvActions
    {
        public static void ValidateRequiredComponentsForAllClasses()
        {
            // Get all objects in the scene and enumerate their components
            var allObjectsInScene =
                // Get all game objects in the scene
                UnityEngine.Object.FindObjectsOfType<GameObject>(true)
                // Replace list elements with an anonymous type containing the game object and its components for easier filtering
                .Select(x =>
                new
                {
                    gameObjectInScene = x,
                    components = x.GetComponents(typeof(Component))
                });

            // Get all classes with the RequiredCustomComponent attribute
            var allClassesUsingRequiredComponents = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.IsDefined(typeof(RequiredCustomComponent)))).ToList();

            // For each class with the RequiredCustomComponent attribute, get all objects in the scene that contain that class
            foreach (var classUsingRequiredComponents in allClassesUsingRequiredComponents)
            {
                var objectsContainingClass =
                    allObjectsInScene
                    // Filter out objects that don't contain the class
                    .Where(x => x.components
                        .Any(y => y.GetType() == classUsingRequiredComponents))
                    // Replace list elements with the game objects they contain
                    .Select(x => x.gameObjectInScene)
                    .ToList();

                // Validate that the objects in the scene that contain the class contain all the required components defined with the RequiredCustomComponent attribute
                objectsContainingClass.ForEach(sceneObject =>
                {
                    RcvHelpers.VerifyRequiredComponentsForClass(sceneObject, classUsingRequiredComponents);
                });
            }
        }
    }
}
