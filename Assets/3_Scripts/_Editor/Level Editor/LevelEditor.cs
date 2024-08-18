#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using LevelEditor;
using UnityEngine;
using UnityEditor;

namespace ODProjects.LevelEditor
{
    public class LevelEditor : EditorWindow
    {
        #region DATAS

        private LevelData[] _allLevelDatas;
        private LevelData _currentLevelData;
        private SpriteData _spriteData;

        #endregion
        
        #region VARIABLES
        
        private SelectedElement _selectedElement;
        private int _elementCount;
        
        private bool _targetColorsInitialized;
        private int _boxSize = 25;
        private bool _hasInitialize;
        private string[] _levelDataNames;
        private int _selectedOption = 0;

        #endregion

        #region MAIN FUNCTIONS

        [MenuItem("OD Projects/Mobile/LevelEditor", false, 1)]
        public static void ShowCreatorWindow()
        {
            LevelEditor window = GetWindow<LevelEditor>();

            window.titleContent = new GUIContent("Level Editor");
            window.titleContent.image = EditorGUIUtility.IconContent("d_Animation.EventMarker").image;
            window.titleContent.tooltip = "Collect Numbers Level Editor, by OD";
            window.Focus();
        }

        #endregion

        #region GUI FUNCTIONS

        private void LoadLevelDatas()
        {
            string levelDataFolder = "Assets/Resources/ScriptableObjects/Data/LevelData";
            if (Directory.Exists(levelDataFolder))
            {
                string[] assetPaths = Directory.GetFiles(levelDataFolder, "*.asset");
                List<LevelData> levelDataList = new List<LevelData>();
                List<string> levelDataNameList = new List<string>();

                foreach (string path in assetPaths)
                {
                    LevelData levelData = AssetDatabase.LoadAssetAtPath<LevelData>(path);
                    if (levelData != null)
                    {
                        levelDataList.Add(levelData);
                        levelDataNameList.Add(levelData.name);
                    }
                }

                _allLevelDatas = levelDataList.ToArray();
                _levelDataNames = levelDataNameList.ToArray();
                if (_currentLevelData == null) _currentLevelData = levelDataList[0];
            }
            else
            {
                _allLevelDatas = new LevelData[0];
            }
        }
        private Vector2 scrollPosition = Vector2.zero;

        private void OnGUI()
        {
            LoadLevelDatas();
            
            
            _boxSize = 45;

            GUI.color = Color.white;

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition); // Scrollview başlat

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (_currentLevelData != null) Content();
            CheckPathAndInitialization();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.EndScrollView();

            GUI.color = Color.green;
            
            /* NEW LEVEL BUTTON
            if (GUILayout.Button("CREATE NEW LEVEL", GUILayout.Height(40)))
            {
                _currentLevelData = ScriptableObject.CreateInstance<LevelData>();
                string levelDataFolder = "Assets/Resources/ScriptableObjects/Data/LevelData/";
                string[] assetPaths = Directory.GetFiles(levelDataFolder, "*.asset");
                _selectedOption = assetPaths.Length;

                string levelName = "LevelData" + (assetPaths.Length + 1);
                string path = levelDataFolder + levelName + ".asset";
                
                AssetDatabase.CreateAsset(_currentLevelData, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }*/
        }

        private void CheckPathAndInitialization()
        {
            if (!_hasInitialize)
            {
                _spriteData = Resources.Load<SpriteData>("ScriptableObjects/Data/SpriteData");
                if (!_currentLevelData.HasPath)
                {
                    _currentLevelData.SetArray(_currentLevelData.gridSize.x * _currentLevelData.gridSize.y);
                }
                _hasInitialize = true;
            }
        }
        
        private void Content()
        {
            EditorGUILayout.Space();

            GUILayout.Label("Level Editor", EditorStyles.boldLabel);

            EditorGUILayout.Space();

            LevelDropdown();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box", GUILayout.Width(400));
            _currentLevelData = (LevelData)EditorGUILayout.ObjectField("Level Data", _currentLevelData, typeof(LevelData), false);
            _spriteData = (SpriteData)EditorGUILayout.ObjectField("Sprite Data", _spriteData, typeof(SpriteData), false);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("box", GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginVertical("box", GUILayout.Width(300));
            _selectedElement = (SelectedElement)EditorGUILayout.EnumPopup("Selected Element", _selectedElement);
            
            _elementCount = EditorGUILayout.IntField("Element Count", _elementCount);
            
            EditorGUILayout.Space();

            GridArea();
            CreateGrid();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(50);
            

            GUI.color = Color.red;
            if (GUILayout.Button("CLEAR LEVEL", GUILayout.Height(30)))
            {
                _currentLevelData.ClearPath();
                _hasInitialize = false;
            }


            GUI.color = Color.white;
            EditorUtility.SetDirty(_currentLevelData);
        }

        private void LevelDropdown()
        {
            GUILayout.Label("Level Dropdown", EditorStyles.boldLabel);

            int newSelectedOption = EditorGUILayout.Popup("Select a Level:", _selectedOption, _levelDataNames);

            if (_selectedOption != newSelectedOption)
            {
                _selectedOption = newSelectedOption;
                _currentLevelData = _allLevelDatas[_selectedOption];
                _hasInitialize = false;
            }
        }
        
        private void GridArea()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Grid Area", GUILayout.Width(75));
            Vector2Int newGridSize = _currentLevelData.gridSize;
            EditorGUILayout.EndHorizontal();


            if (_currentLevelData.gridSize.x != newGridSize.x || _currentLevelData.gridSize.y != newGridSize.y)
            {
                _currentLevelData.gridSize.x = newGridSize.x;
                _currentLevelData.gridSize.y = newGridSize.y;

                _hasInitialize = false;
                _currentLevelData.ClearPath();
                CheckPathAndInitialization();
            }

            EditorGUILayout.Space();

        }

        private void CreateGrid()
        {
            float totalWidth = _currentLevelData.gridSize.x * _boxSize;
            float startX = (position.width - totalWidth) / 2;
            GUIContent content = new GUIContent(_elementCount.ToString());
            
            GUI.color = Color.white;

            for (int y = _currentLevelData.gridSize.y - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space((position.width - totalWidth) / 2);
    
                for (int x = 0; x < _currentLevelData.gridSize.x; x++)
                {
                    int index = x * _currentLevelData.gridSize.y + y;

                    if (index >= 0 && index < _currentLevelData.ArrayLength())
                    {
                        bool isInnerCell = y > 0 && y < _currentLevelData.gridSize.y - 1 &&
                                           x > 0 && x < _currentLevelData.gridSize.x - 1;

                        if (_currentLevelData.gridSize.y > 1 && isInnerCell)
                        {
                            GridButton(new GUIContent(""), index, false);
                        }
                        else
                        {
                            GridButton(content, index, true);
                        }
                    }
                }

                GUI.color = Color.white;
                GUILayout.Space((position.width - totalWidth) / 2);
                EditorGUILayout.EndHorizontal();
            }
            
        }

        private void GridButton(GUIContent content, int index, bool active)
        {
            _currentLevelData.ActivateElement(index,active);
            GUI.color = _currentLevelData.GetColor(index);
            content = _currentLevelData.GetContent(index);

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.LowerRight,
            };
            if (!active)
            {
                GUIStyle transparentButtonStyle = new GUIStyle(GUI.skin.button);
                Color originalColor = GUI.color;
                
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

                content.text = "";
                GUI.Button(GUILayoutUtility.GetRect(_boxSize, _boxSize), content, transparentButtonStyle);
                
                GUI.color = originalColor;
            }
            else if (GUI.Button(GUILayoutUtility.GetRect(_boxSize, _boxSize), content, buttonStyle))
            {
                if (_selectedElement == SelectedElement.Null) // ERASE
                {
                    content.text = "N/A";
                    ChangeButtonState(content, 0, index, SelectedElement.Null);
                }
                else  // ADD
                {
                    if (_selectedElement != SelectedElement.Null && _elementCount == 0) _elementCount = 1;
                    content.text = _elementCount.ToString();
                    
                    ChangeButtonState(content, _elementCount, index, _selectedElement);
                }
            }
        }

        private void ChangeButtonState(GUIContent content, int elementCount, int index, SelectedElement selectedElement)
        {
            content.image = _spriteData.GetTexture(selectedElement);
            //content.image.width = 30;
            //content.image.height = 30;
            Debug.Log($"Index: {index} Element Count: {elementCount} Selected Element: {selectedElement}");
            _currentLevelData.SetElement(index, elementCount, content, selectedElement);
            string temp1 = content.text;
            content.text = temp1;
            /*
            List<int> indexes = new List<int>();
            if (_currentLevelData.ElementIsAvailable(index)) indexes.Add(index);
            else indexes.Clear();
            
            if (indexes.Count > 0)
            {
                
            }*/
        }

        #endregion
    }
}
#endif