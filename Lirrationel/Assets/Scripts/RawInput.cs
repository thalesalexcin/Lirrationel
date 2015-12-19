using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public enum EElementType
    {
        Unknown = 0,
        D_WaterHell = 1,
        S_DesyncSound = 101,
        O_CarnivoreFruit = 201,
        O_DominoSpider,
        O_Mushroom,
        O_WoolBall,
        F_MushroomJose
    }

    public class RawInput
    {
        public Vector2 Position { get; set; }
        public int PlayerId { get; set; }
        public EElementType ElementType { get; set; }
    }
}
