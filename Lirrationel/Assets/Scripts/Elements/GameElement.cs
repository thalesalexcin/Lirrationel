using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Elements
{
    public abstract class GameElement : MonoBehaviour
    {
        public Vector2 ControlPosition { get; set; }

        public float EntryTime { get; set; }

        public bool Unplugged { get; set; }

        public EElementType ElementType { get; set; }

        public bool Fusioned { get; set; }
    }
}
