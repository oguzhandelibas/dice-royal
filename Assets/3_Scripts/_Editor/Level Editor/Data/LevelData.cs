using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Data/Level/LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public Vector2Int gridSize;
        private Vector2Int _gridSizeHistory;
        public Element[] Elements = new Element[0];

        public bool HasPath;

        #region GET LEVEL DATA

        public SelectedElement GetSelectedElement(int index)
        {
            return Elements[index].hasElement ? Elements[index].selectedElement : SelectedElement.Null;
        }

        #endregion

        #region LEVEL DATA CREATION
        public void SetArray(int length)
        {
            Elements = new Element[length];
            ClearPath();
        }
        public int ArrayLength() => Elements.Length;
        public bool ElementIsAvailable(int index) => Elements[index].selectedElement == SelectedElement.Null;

        public void ActivateElement(int index, bool isActive)
        {
            Elements[index].isActive = isActive;
        }
        
        [Obsolete("Obsolete")]
        public void SetElement(int index, int elementCount, GUIContent guiContent, SelectedElement selectedElement)
        {
            if (!HasPath) HasPath = true;
            Elements[index].elementCount = elementCount;
            Elements[index].selectedElement = selectedElement;
            Elements[index].GuiContent = guiContent;
            Elements[index].hasElement = selectedElement != SelectedElement.Null;
            SetDirty();
        }
        public GUIContent GetContent(int index) => Elements[index].GuiContent;
        public Color GetColor(int index)
        {
            return Elements[index].Color;
        }
        
        public void ClearPath()
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i].Color = Color.white;
                Elements[i].GuiContent = new GUIContent("N/A");
                Elements[i].selectedElement = SelectedElement.Null;
            }

            HasPath = false;
        }
        public void ResetGrid()
        {
            ClearPath();
            gridSize = new Vector2Int(3,3);
        }

        #endregion

        private void OnValidate()
        {
            if (_gridSizeHistory != gridSize)
            {
                Elements = new Element[gridSize.x * gridSize.y];
                _gridSizeHistory = gridSize;
            }
            
        }
    }
}
