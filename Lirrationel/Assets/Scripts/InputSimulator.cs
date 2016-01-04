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
            AddOrDelete(EElementType.O_Mushroom, new Vector2(1,0));

        if (Input.GetKeyDown(KeyCode.F2))
            AddOrDelete(EElementType.O_CarnivoreFruit, new Vector2(1,0));
            
        return _Inputs;
    }

    private void AddOrDelete(EElementType type, Vector2 position)
    {
        if (!_Inputs.Any(i => i.ElementType == type))
            _Inputs.Add(new RawInput { ElementType = type, PlayerId = 1, Position = position });
        else
            _Inputs.Remove(_Inputs.Single(i => i.ElementType == type));
    }
}
