using NUnit.Framework.Constraints;
using Unity.VisualScripting;
//using UnityEngine;

namespace PeckNSend.Models
{
    public abstract class UnityModelBaseClass : ModelBaseClass
    {
        public int ModelID { get; set; }

        public virtual void Update(float deltaTime)
        {
        }

        public virtual void FixedUpdate(float fixedDeltaTime)
        {
        }
    }
}
