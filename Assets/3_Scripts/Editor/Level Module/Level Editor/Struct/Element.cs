using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelEditor
{
    [Serializable]
    public struct Element
    {
        public SelectedElement selectedElement;
        public int elementCount;

        public bool hasElement;

        public GUIContent GuiContent;
        public Color Color;
    }
}
