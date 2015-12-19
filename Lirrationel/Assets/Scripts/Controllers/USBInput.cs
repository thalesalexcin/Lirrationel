using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using USBLib;

namespace Assets.Scripts
{
    public class USBCoord
    {
        public int PlayerId { get; set; }
        public Vector2 Position { get; set; }
    }

    public class USBHub 
    {
        public Dictionary<int, USBCoord> PortMapping { get; set; }

        public USBHub()
        {
            PortMapping = new Dictionary<int,USBCoord>();
        }
    }

    public class USBInput : IInputBridge
    {
        private Dictionary<string, EElementType> _Devices;
        private Dictionary<string, USBHub> _Hubs;

        public USBInput()
        {
            _Devices = new Dictionary<string, EElementType>();
            _Hubs = new Dictionary<string, USBHub>();
            _ReadDevicesConfig();
        }

        private void _ReadDevicesConfig()
        {
            XmlDocument document = _LoadDocument();
            _ReadDevices(document);
            _ReadHubs(document);
        }

        private XmlDocument _LoadDocument()
        {
            XmlDocument document = new XmlDocument();

            var releasePath = Path.Combine(Application.dataPath, Path.Combine("Plugins", "devices.config.dll"));
            var debugPath = Path.Combine(Application.dataPath, Path.Combine("Resources", "devices.config.dll"));

            if (File.Exists(releasePath))
                document.Load(releasePath);
            else
                document.Load(debugPath);
            return document;
        }

        private void _ReadHubs(XmlDocument document)
        {
            var hubs = document.DocumentElement.TryGetChildNode("Hubs");
            if (hubs != null)
            {
                foreach (XmlNode hub in hubs.ChildNodes)
                {
                    if (hub is XmlElement)
                    {
                        var hubKey = hub.TryGetAttributeValue("Key");
                        var playerId = Int32.Parse(hub.TryGetAttributeValue("Player"));

                        if (hub != null)
                        {
                            if (!_Hubs.ContainsKey(hubKey))
                                _Hubs.Add(hubKey, new USBHub());

                            var usbHub = _Hubs[hubKey];

                            foreach (XmlElement position in hub.ChildNodes)
                            {
                                var positionX = Int32.Parse(position.TryGetAttributeValue("X"));
                                var positionY = Int32.Parse(position.TryGetAttributeValue("Y"));
                                var portNumber = Int32.Parse(position.TryGetAttributeValue("PortNumber"));

                                var usbCoord = new USBCoord();
                                usbCoord.PlayerId = playerId;
                                usbCoord.Position = new Vector2(positionX, positionY);
                                usbHub.PortMapping.Add(portNumber, usbCoord);
                            }
                        }
                    }
                }
            }
        }

        private void _ReadDevices(XmlDocument document)
        {
            XmlNode devices = document.DocumentElement["Devices"];

            if (devices != null)
            {
                foreach (XmlNode child in devices.ChildNodes)
                {
                    if (child is XmlElement)
                    {
                        XmlAttribute keyAttribute = child.Attributes["Key"];
                        string key = null;
                        if (keyAttribute != null)
                            key = keyAttribute.Value;

                        XmlAttribute valueAttribute = child.Attributes["Value"];
                        EElementType valueType = EElementType.Unknown;
                        if (valueAttribute != null)
                        {
                            string value = valueAttribute.Value;
                            var names = Enum.GetNames(typeof(EElementType));
                            if (names.Any(n => n.ToUpperInvariant() == value.ToUpperInvariant()))
                                valueType = (EElementType)Enum.Parse(typeof(EElementType), value, true);
                        }

                        if (!string.IsNullOrEmpty(key) && valueType != EElementType.Unknown)
                            _Devices.Add(key, valueType);
                    }
                }
            }
        }

        public IEnumerable<RawInput> GetRawInputs()
        {
            List<RawInput> inputs = new List<RawInput>();

            var hostCtrls = USB.GetHostControllers();

            foreach (var hostCtrl in hostCtrls)
            {
                var hub = hostCtrl.GetRootHub();
                AddInputDevices(hub, hostCtrl, inputs);
            }
            
            return inputs;
        }

        private void AddInputDevices(USB.USBHub hub, USB.USBController hostCtrl, List<RawInput> inputs)
        {
            foreach (var port in hub.GetPorts())
            {
                if (port.IsDeviceConnected && port.IsHub)
                {
                    var portHub = port.GetHub();
                    AddInputDevices(portHub, hostCtrl, inputs);
                }
                else if (port.IsDeviceConnected && !port.IsHub)
                {
                    var device = port.GetDevice();

                    string hubKey = hub.DriverKey;
                    if(hub.IsRootHub)
                        hubKey = hostCtrl.DriverKeyName;

                    if (_Devices.ContainsKey(device.DeviceDriverKey) && _Hubs.ContainsKey(hubKey))
                    {
                        var input = new RawInput();

                        int portNumber = device.PortNumber;
                        var usbHub = _Hubs[hubKey];
                        var mapping = usbHub.PortMapping[portNumber];

                        input.ElementType = _Devices[device.DeviceDriverKey];
                        input.Position = mapping.Position;
                        input.PlayerId = mapping.PlayerId;

                        inputs.Add(input);
                    }
                }
            }
        }
    }
}
