using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace System.Xml
{
    public static class Extensions
    {
        public static XmlNode TryGetChildNode(this XmlNode node, string childNodeName)
        {
            XmlNode childNode = null;
            if (node != null)
                childNode = node[childNodeName];
            return childNode;
        }

        public static string TryGetAttributeValue(this XmlNode node, string attributeName)
        {
            string value = null;
            if (node != null)
            {
                XmlAttribute attribute = node.Attributes[attributeName];
                if (attribute != null)
                    value = attribute.Value;
            }
            return value;
        }
    }
}
