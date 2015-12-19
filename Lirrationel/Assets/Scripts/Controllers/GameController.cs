using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Elements;
using Assets.Scripts.Controllers;
using System.Linq;
using System;

public class GameController : MonoBehaviour {

    private InputController _InputController;
    private Dictionary<EElementType, InputElement> _InputElements;
    private Dictionary<EElementType, GameObject> _Prefabs;
    private List<GameObject> _GameElements;

	void Start () 
    {
        _InputController = new InputController();
        _InputElements = new Dictionary<EElementType, InputElement>();
        _GameElements = new List<GameObject>();

        _LoadPrefabs();
	}

    private void _LoadPrefabs()
    {
        _Prefabs = new Dictionary<EElementType, GameObject>();
        _Prefabs.Add(EElementType.O_Mushroom, (GameObject)Resources.Load("Prefabs/Mushroom"));
        _Prefabs.Add(EElementType.O_DominoSpider, (GameObject)Resources.Load("Prefabs/DominoSpider"));
        _Prefabs.Add(EElementType.O_CarnivoreFruit, (GameObject)Resources.Load("Prefabs/CarnivoreFruit"));
        _Prefabs.Add(EElementType.O_WoolBall, (GameObject)Resources.Load("Prefabs/WoolBall"));
        _Prefabs.Add(EElementType.F_MushroomJose, (GameObject)Resources.Load("Prefabs/MushroomJose"));
    }
	
	void Update() 
    {
        var inputs = _InputController.GetInputs();

        _UpdateElements(inputs);

        //calculate fusions
        _FuseElements();

        //calculate interactions
	}

    private void _FuseElements()
    {
        var objects = _InputElements.Values.Where(e => (int) e.ElementType >= 201 && !e.Fusioned).ToList();
        foreach (var objectElement in objects)
        {
            var objectToFuseWith = objects.SingleOrDefault(e => e.ControlPosition == objectElement.ControlPosition && e != objectElement && !e.Fusioned);
            if (objectToFuseWith != null)
            {
                //Make Fusion
                EElementType firstElementType = objectElement.EntryTime < objectToFuseWith.EntryTime ? objectElement.ElementType : objectToFuseWith.ElementType;
                EElementType secondElementType = objectElement.EntryTime < objectToFuseWith.EntryTime ? objectToFuseWith.ElementType : objectElement.ElementType;

                var fusionType = FusionFactory.GetFusionType(firstElementType, secondElementType);
                if (fusionType != EElementType.Unknown)
                {
                    _InputElements[firstElementType].Fusioned = true;
                    _InputElements[secondElementType].Fusioned = true;
                    var fusionObject = _CreateElement(fusionType);
                    fusionObject.GetComponent<FusionElement>().FirstFusionElement = firstElementType;
                    fusionObject.GetComponent<FusionElement>().SecondFusionElement = secondElementType;
                    _RemoveElement(firstElementType);
                    _RemoveElement(secondElementType);
                }
            }
        }
    }

    private void _UpdateElements(IEnumerable<RawInput> inputs)
    {
        foreach (var element in _InputElements.Values)
            element.Unplugged = true;

        foreach (var input in inputs)
        {
            if (!_InputElements.ContainsKey(input.ElementType))
            {
                var inputElement = new InputElement();
                inputElement.ControlPosition = input.Position;
                inputElement.EntryTime = Time.time;
                inputElement.ElementType = input.ElementType;

                _InputElements.Add(input.ElementType, inputElement);

                if (_Prefabs.ContainsKey(input.ElementType))
                    _CreateElement(input.ElementType);
            }

            _InputElements[input.ElementType].Unplugged = false;
        }

        var unpluggeds = _InputElements.Values.Where(e => e.Unplugged).Select(e => e.ElementType).ToList();

        foreach (var unplugged in unpluggeds)
        {
            if (_InputElements[unplugged].Fusioned)
                _TryDefuse(unplugged);
            else
                _RemoveElement(unplugged);

            _InputElements.Remove(unplugged);
        }
    }

    private void _TryDefuse(EElementType unplugged)
    {
        //Defusion
        var fusionToDefuse = _GameElements.SingleOrDefault(g =>
        {
            var fusionElement = g.GetComponent<FusionElement>();
            return fusionElement != null && (fusionElement.FirstFusionElement == unplugged || fusionElement.SecondFusionElement == unplugged);
        });

        if (fusionToDefuse != null)
        {
            var fusionElement = fusionToDefuse.GetComponent<FusionElement>();
            _RemoveElement(fusionElement.ElementType);
            if (fusionElement.FirstFusionElement == unplugged)
                _CreateElement(fusionElement.SecondFusionElement);
            else
                _CreateElement(fusionElement.FirstFusionElement);

            _InputElements[fusionElement.FirstFusionElement].Fusioned = false;
            _InputElements[fusionElement.SecondFusionElement].Fusioned = false;
        }
    }

    private GameObject _CreateElement(EElementType elementType)
    {
        var element = Instantiate(_Prefabs[elementType]);
        element.transform.parent = this.transform;
        element.transform.position = Vector2.zero;
        element.GetComponent<GameElement>().ElementType = elementType;
        _GameElements.Add(element);
        return element;
    }

    private void _RemoveElement(EElementType elementType)
    {
        var element = _GameElements.Single(g => g.GetComponent<GameElement>().ElementType == elementType);
        Destroy(element);
        _GameElements.Remove(element);
    }
}
