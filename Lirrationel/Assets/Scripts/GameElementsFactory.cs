using Assets.Scripts.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public static class GameElementsFactory
    {
        public static GameElement Create(EElementType elementType)
        {
            GameElement gameElement = null;

            switch (elementType)
            {
                case EElementType.D_WaterHell:
                    gameElement = new WaterHell();
                    break;
                case EElementType.O_CarnivoreFruit:
                    gameElement = new CarnivoreFruit();
                    break;
                case EElementType.O_DominoSpider:
                    gameElement = new DominoSpider();
                    break;
                case EElementType.O_Mushroom:
                    gameElement = new Mushroom();
                    break;
                case EElementType.O_WoolBall:
                    gameElement = new WoolBall();
                    break;
                case EElementType.S_DesyncSound:
                    gameElement = new DesyncSoud();
                    break;
                case EElementType.F_MushroomJose:
                    gameElement = new MushroomJose();
                    break;
                default:
                    gameElement = null;
                    break;
            }

            return gameElement;
        }
    }
}
