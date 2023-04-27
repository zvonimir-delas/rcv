using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZDelas.Rcv.Runtime
{
    // Defines a custom attribute that can be used to mark a class as requiring certain components
    // Example use: [RequiredCustomComponent(typeof(HealthComponent))]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredCustomComponent : Attribute
    {
        public Type componentType;

        public RequiredCustomComponent(Type componentType)
        {
            this.componentType = componentType;
        }
    }
}
