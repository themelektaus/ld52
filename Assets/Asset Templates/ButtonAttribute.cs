using System;
using UnityEngine;

namespace AssetTemplates
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ButtonAttribute : PropertyAttribute
    {
        public readonly string text;
        public readonly string method;

        public ButtonAttribute(string text = null, string method = null)
        {
            this.text = text;
            this.method = method;
        }
    }
}