using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;

public class InputSimulator : IInputBridge {

    private List<RawInput> _Inputs;


    public InputSimulator()
    {
        _Inputs = new List<RawInput>();
    }

    public IEnumerable<RawInput> GetRawInputs()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!_Inputs.Any(i => i.ElementType == EElementType.O_Mushroom))
                _Inputs.Add(new RawInput { ElementType = EElementType.O_Mushroom, PlayerId = 1, Position = new Vector2(1, 0) });
            else
                _Inputs.Remove(_Inputs.Single(i => i.ElementType == EElementType.O_Mushroom));
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (!_Inputs.Any(i => i.ElementType == EElementType.O_CarnivoreFruit))
                _Inputs.Add(new RawInput { ElementType = EElementType.O_CarnivoreFruit, PlayerId = 2, Position = new Vector2(1, 0) });
            else
                _Inputs.Remove(_Inputs.Single(i => i.ElementType == EElementType.O_CarnivoreFruit));
        }
            
        //inputs.Add(new RawInput { ElementType = EElementType.S_DesyncSound, PlayerId = 1, Position = new Vector2(0, 0) });
        
        //inputs.Add(new RawInput { ElementType = EElementType.O_CarnivoreFruit, PlayerId = 2, Position = new Vector2(1, 0) });
        //inputs.Add(new RawInput { ElementType = EElementType.D_WaterHell, PlayerId = 2, Position = new Vector2(1, 2) });
        return _Inputs;
    }
}
