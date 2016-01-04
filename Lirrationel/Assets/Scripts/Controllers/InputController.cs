using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class InputController {

    private IInputBridge _InputBridge;

    public InputController(bool useSimulator) 
    {
        if(useSimulator)
            _InputBridge = new InputSimulator();
        else
            _InputBridge = new USBInput();
	}

    public IEnumerable<RawInput> GetInputs() 
    {
        return _InputBridge.GetRawInputs();
	}

    internal void OnGUI()
    {
        _InputBridge.OnGUI();
    }
}
