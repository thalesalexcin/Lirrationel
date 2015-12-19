using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;

public interface IInputBridge {
    
    /// <summary>
    /// Get the raw inputs from the console
    /// </summary>
    /// <returns></returns>
    IEnumerable<RawInput> GetRawInputs();

}
