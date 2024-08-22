using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelEditor
{
    public class LevelData : ScriptableObject
    {
        public Vector2Int _gridSize;
        public Vector2Int GridSize
        {
            get => _gridSize;
            set
            {
                if (_gridSize != value)
                {
                    _gridSize = value;
                    elements = new Element[value.x * value.y];
                }
            }
        }

        [HideInInspector] public Element[] elements;

        [HideInInspector] public bool hasPath;

        #region GET LEVEL DATA

        public SelectedElement GetSelectedElement(int index)
        {
            return elements[index].hasElement ? elements[index].selectedElement : SelectedElement.Null;
        }

        #endregion

        #region LEVEL DATA CREATION
        public void SetArray(int length)
        {
            elements = new Element[length];
            ClearPath();
        }
        public int ArrayLength() => elements.Length;
        public bool ElementIsAvailable(int index) => elements[index].selectedElement == SelectedElement.Null;

        public void ActivateElement(int index, bool isActive)
        {
            elements[index].isActive = isActive;
        }
        
        [Obsolete("Obsolete")]
        public void SetElement(int index, int elementCount, GUIContent guiContent, SelectedElement selectedElement)
        {
            if (!hasPath) hasPath = true;
            elements[index].elementCount = elementCount;
            elements[index].selectedElement = selectedElement;
            elements[index].GuiContent = guiContent;
            elements[index].hasElement = selectedElement != SelectedElement.Null;
            SetDirty();
        }
        public GUIContent GetContent(int index) => elements[index].GuiContent;
        public Color GetColor(int index)
        {
            return elements[index].Color;
        }
        
        public void ClearPath()
        {
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].Color = Color.white;
                elements[i].GuiContent = new GUIContent("N/A");
                elements[i].selectedElement = SelectedElement.Null;
            }

            hasPath = false;
        }
        public void ResetGrid()
        {
            ClearPath();
            GridSize = new Vector2Int(3,3);
        }

        #endregion

        private void OnValidate()
        {
            
        }
    }
    
}
