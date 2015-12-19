using Assets.Scripts.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public static class FusionFactory
    {
        private static Dictionary<KeyValuePair<EElementType, EElementType>, EElementType> _FusionTable;


        static FusionFactory()
        {
            _FusionTable = new Dictionary<KeyValuePair<EElementType, EElementType>, EElementType>();
            _FusionTable.Add(new KeyValuePair<EElementType, EElementType>(EElementType.O_CarnivoreFruit, EElementType.O_Mushroom), EElementType.F_MushroomJose);
        }


        public static EElementType GetFusionType(EElementType firstElement, EElementType secondElement)
        {
            EElementType fusionType;
            _FusionTable.TryGetValue(new KeyValuePair<EElementType, EElementType>(firstElement, secondElement), out fusionType);

            return fusionType;
        }
    }
}
