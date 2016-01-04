using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class InputController {

    private IInputBridge _InputBridge;

    public InputController() 
    {
        //_InputBridge = new USBInput();
        _InputBridge = new InputSimulator();
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
