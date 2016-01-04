using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;

public class InputSimulator : IInputBridge {

    private List<RawInput> _Inputs;
    private int _Position;
    private float _Distance;


    public InputSimulator()
    {
        _Inputs = new List<RawInput>();
        _Position = 0;
        _Distance = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().DistanceBetweenObjects;
    }

    public IEnumerable<RawInput> GetRawInputs()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            AddOrDelete(EElementType.O_Mushroom, new Vector2(_Position, 1));

        if (Input.GetKeyDown(KeyCode.F2))
            AddOrDelete(EElementType.O_CarnivoreFruit, new Vector2(_Position, 1));
        
        if (Input.GetKeyDown(KeyCode.F3))
            AddOrDelete(EElementType.O_WoolBall, new Vector2(_Position, 1));

        if (Input.GetKeyDown(KeyCode.F4))
            AddOrDelete(EElementType.O_DominoSpider, new Vector2(_Position, 1));

        if (Input.GetKeyDown(KeyCode.B))
            AddOrDelete(EElementType.D_WaterHell, new Vector2(1, 2));

        if (Input.GetKeyDown(KeyCode.S))
            AddOrDelete(EElementType.S_DesyncSound, new Vector2(0, 0));

        if (Input.GetKeyDown(KeyCode.Alpha1))
            _Position = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            _Position = 1;

        return _Inputs;
    }

    private void AddOrDelete(EElementType type, Vector2 position)
    {
        if (!_Inputs.Any(i => i.ElementType == type))
            _Inputs.Add(new RawInput { ElementType = type, PlayerId = 1, Position = position });
        else
            _Inputs.Remove(_Inputs.Single(i => i.ElementType == type));
    }


    public void OnGUI()
    {
        GUI.Box(new Rect(Camera.main.WorldToScreenPoint(new Vector2((-_Distance / 2) + _Position * _Distance, 3)), new Vector2(1, 1)), "Position: " + _Position);
    }
}
