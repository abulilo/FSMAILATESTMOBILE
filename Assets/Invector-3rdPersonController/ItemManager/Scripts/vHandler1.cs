using UnityEngine;
using System.Collections.Generic;
namespace Invector
{
    [System.Serializable]
    public class vHandler1
    {
        public Transform defaultHandler;
        public List<Transform> customHandlers;
        public vHandler1()
        {
            customHandlers = new List<Transform>();
        }
    }
}