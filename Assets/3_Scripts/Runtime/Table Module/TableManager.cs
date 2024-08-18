using System;
using LevelEditor;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private LevelType levelType;
    private LevelDatas _levelDatas;
    private LevelData _levelData;
    private Element[] _tableElements;
    
    [Header("Table Creation")]
    [SerializeField] private Transform tableParent;
    [SerializeField] private TileBehaviour tileBehaviourPrefab;
    private void InitializeTable()
    {
        _levelDatas = SO_Manager.Get<LevelDatas>();
        _levelData = _levelDatas.GetLevelData(levelType);
        Vector2Int gridSize = _levelData.gridSize;
        _tableElements = _levelData.Elements;
        CreateTables(gridSize, 1.05f);
    }
    
    private void CreateTables(Vector2Int gridSize, float spacing)
    {
        //foreach (Transform child in tablesParent) Destroy(child.gameObject);
        //_tableBehaviours = new TableBehaviour[rows * columns];
        int totalTileIndex = 0;
        int activeTileIndex = 0;
        

        int maxRow = gridSize.x;
        int maxColumn = gridSize.y;
        int index = 1;
        
        for (int i = 0; i < maxColumn; i++) // Sol kenar (aşağıdan yukarıya)
        {
            int row = 0;
            int column = i;
            ProcessTile(row, column, index, gridSize, Quaternion.Euler(0,90,0));
            index++;
        }

        for (int i = 1; i < maxRow; i++) // Üst kenar (soldan sağa)
        {
            int row = i;
            int column = maxColumn - 1;
            ProcessTile(row, column, index, gridSize, Quaternion.Euler(0,180,0));
            index++;
        }

        for (int i = maxColumn - 2; i >= 0; i--) // Sağ kenar (yukarıdan aşağıya)
        {
            int row = maxRow - 1;
            int column = i;
            ProcessTile(row, column, index, gridSize, Quaternion.Euler(0,270,0));
            index++;
        }

        for (int i = maxRow - 2; i > 0; i--) // Alt kenar (sağdan sola)
        {
            int row = i;
            int column = 0;
            ProcessTile(row, column, index, gridSize, Quaternion.Euler(0,0,0));
            index++;
        }
    }
    
    private void ProcessTile(int row, int column, int tileIndex, Vector2Int gridSize, Quaternion rotation, float spacing = 1.05f)
    {
        SpriteData spriteData = SO_Manager.Get<SpriteData>();
        Element element = _tableElements[row * gridSize.x + column];
    
        if (element.isActive)
        {
            TileBehaviour tileBehaviourTemp = Instantiate(tileBehaviourPrefab, tableParent);
            tileBehaviourTemp.transform.position = new Vector3(row * spacing, -0.4f, column * spacing);
            tileBehaviourTemp.transform.rotation = rotation;
                
            int elementCount = element.elementCount;
            Sprite elementSprite = spriteData.GetSprite(element.selectedElement);
            
            if (element.selectedElement == SelectedElement.Null)
            {
                tileBehaviourTemp.InitializeTile(elementSprite, tileIndex, elementCount, true);
            }
            else
            {
                tileBehaviourTemp.InitializeTile(elementSprite, tileIndex, elementCount);
            }
            
            
        }
    }

    private void OnEnable()
    { 
        SO_Manager.Get<GameSignals>().OnGameStart += InitializeTable;
    }

    private void OnDisable()
    {
        SO_Manager.Get<GameSignals>().OnGameStart -= InitializeTable;
    }
}