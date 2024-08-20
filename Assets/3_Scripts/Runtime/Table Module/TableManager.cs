using System;
using System.Collections.Generic;
using LevelEditor;
using ODProjects;
using UnityEngine;
using Random = UnityEngine.Random;

public record TableCreationData
{
    public int Row;
    public int Column;
    public int TileIndex;
    public Vector2Int GridSize;
    public Quaternion Rotation;
    public float Spacing;
}

public record TileData
{
    public Vector3 Position;
    public SelectedElement SelectedElement;
    public int ElementCount;
}

public class TableManager : MonoBehaviour
{
    [Header("Level")] [SerializeField] private LevelType levelType;
    [SerializeField] private LevelDataType levelDataType;

    private LevelDatas _levelDatas;
    private LevelData _levelData;
    private Element[] _tableElements;

    private List<TileData> _tileDatas = new List<TileData>();

    [Header("Table Creation")] [SerializeField]
    private Transform tableParent;

    [SerializeField] private TileBehaviour tileBehaviourPrefab;

    private void InitializeTable()
    {
        _levelDatas = SO_Manager.Get<LevelDatas>();
        _levelData = _levelDatas.GetLevelData(levelType);
        Vector2Int gridSize = _levelData.gridSize;
        _tableElements = _levelData.Elements;
        CreateTables(gridSize, 1.05f);
    }

    /* Optimization Test on CreateTables Method
    private void CreateTables(Vector2Int gridSize, float spacing)
    {
        int maxRow = gridSize.x;
        int maxColumn = gridSize.y;
        int index = 1;

        // Döngü parametrelerini belirleyen dizi (başlangıç, bitiş, adım, dönüş açısı)
        (int startRow, int startColumn, int rowIncrement, int colIncrement, Quaternion rotation)[] loops =
        {
            (0, 0, 0, 1, Quaternion.Euler(0, 90, 0)),   // Sol kenar
            (1, maxColumn - 1, 1, 0, Quaternion.Euler(0, 180, 0)),  // Üst kenar
            (maxRow - 1, maxColumn - 2, 0, -1, Quaternion.Euler(0, 270, 0)),  // Sağ kenar
            (maxRow - 2, 0, -1, 0, Quaternion.Euler(0, 0, 0))   // Alt kenar
        };

        foreach (var (startRow, startColumn, rowIncrement, colIncrement, rotation) in loops)
        {
            int row = startRow;
            int column = startColumn;

            while (row >= 0 && row < maxRow && column >= 0 && column < maxColumn)
            {
                var tableCreationData = new TableCreationData
                {
                    row = row,
                    column = column,
                    tileIndex = index,
                    gridSize = gridSize,
                    rotation = rotation,
                    spacing = spacing
                };

                ProcessTile(tableCreationData);
                index++;
                row += rowIncrement;
                column += colIncrement;
            }
        }
    }
    */

    // ReSharper disable Unity.PerformanceAnalysis
    private void CreateTables(Vector2Int gridSize, float spacing)
    {
        int maxRow = gridSize.x;
        int maxColumn = gridSize.y;
        int index = 1;
        TableCreationData tableCreationData;
        for (int i = 0; i < maxColumn; i++) // Sol kenar (aşağıdan yukarıya)
        {
            int row = 0;
            int column = i;
            tableCreationData = new TableCreationData
            {
                Row = row,
                Column = column,
                TileIndex = index,
                GridSize = gridSize,
                Rotation = Quaternion.Euler(0, 90, 0),
                Spacing = spacing
            };
            ProcessTile(tableCreationData);
            index++;
        }
        
        PlayerSignals playerSignals = SO_Manager.Get<PlayerSignals>();
        if (levelType == LevelType.Line)
        {
            playerSignals.OnGameReadyToPlay?.Invoke(_tileDatas);
            return;
        }
        for (int i = 1; i < maxRow; i++) // Üst kenar (soldan sağa)
        {
            int row = i;
            int column = maxColumn - 1;
            tableCreationData = new TableCreationData
            {
                Row = row,
                Column = column,
                TileIndex = index,
                GridSize = gridSize,
                Rotation = Quaternion.Euler(0, 180, 0),
                Spacing = spacing
            };
            ProcessTile(tableCreationData);
            index++;
        }

        for (int i = maxColumn - 2; i >= 0; i--) // Sağ kenar (yukarıdan aşağıya)
        {
            int row = maxRow - 1;
            int column = i;
            tableCreationData = new TableCreationData
            {
                Row = row,
                Column = column,
                TileIndex = index,
                GridSize = gridSize,
                Rotation = Quaternion.Euler(0, 270, 0),
                Spacing = spacing
            };
            ProcessTile(tableCreationData);
            index++;
        }

        for (int i = maxRow - 2; i > 0; i--) // Alt kenar (sağdan sola)
        {
            int row = i;
            int column = 0;
            tableCreationData = new TableCreationData
            {
                Row = row,
                Column = column,
                TileIndex = index,
                GridSize = gridSize,
                Rotation = Quaternion.Euler(0, 0, 0),
                Spacing = spacing
            };
            ProcessTile(tableCreationData);
            index++;
        }
        playerSignals.OnGameReadyToPlay?.Invoke(_tileDatas);
    }

    private void ProcessTile(TableCreationData tableCreationData)
    {
        SpriteData spriteData = SO_Manager.Get<SpriteData>();

        switch (levelDataType)
        {
            case LevelDataType.JSON:
                ProcessTiles_SO(tableCreationData, spriteData);
                break;
            case LevelDataType.ScriptableObject:
                ProcessTiles_SO(tableCreationData, spriteData);
                break;
            case LevelDataType.Random:
                ProcessTiles_Random(tableCreationData, spriteData);
                break;
        }
    }

    /// <summary>
    /// Creates tile according to Scriptable Object data.
    /// </summary>
    private void ProcessTiles_SO(TableCreationData tableCreationData, SpriteData spriteData)
    {
        Element element = _tableElements[tableCreationData.Row * tableCreationData.GridSize.x + tableCreationData.Column];
        if (!element.isActive) return;

        Sprite elementSprite = spriteData.GetSprite(element.selectedElement);
        bool isEmpty = element.selectedElement == SelectedElement.Null;
        Vector3 position = new Vector3(tableCreationData.Row * tableCreationData.Spacing, -0.4f,
            tableCreationData.Column * tableCreationData.Spacing);

        _tileDatas.Add(new TileData
        {
            Position = position,
            SelectedElement = element.selectedElement,
            ElementCount = element.elementCount
        });

        TileBehaviour tileBehaviourTemp = Instantiate(tileBehaviourPrefab, position, tableCreationData.Rotation, tableParent);
        tileBehaviourTemp.InitializeTile(elementSprite, tableCreationData.TileIndex, element.elementCount, isEmpty);
    }


    /// <summary>
    /// Creates tile according to Random.
    /// </summary>
    private void ProcessTiles_Random(TableCreationData tableCreationData, SpriteData spriteData)
    {
        SelectedElement selectedElement = (SelectedElement)Random.Range(0, 4);
        Sprite elementSprite = spriteData.GetSprite(selectedElement);
        int elementCount = Random.Range(1, 26);
        bool isEmpty = selectedElement == SelectedElement.Null || Random.Range(0, 3) == 0;
        Vector3 position = new Vector3(tableCreationData.Row * tableCreationData.Spacing, -0.4f,
            tableCreationData.Column * tableCreationData.Spacing);
        _tileDatas.Add(new TileData
        {
            Position = position,
            SelectedElement = selectedElement,
            ElementCount = elementCount
        });

        TileBehaviour tileBehaviourTemp =
            Instantiate(tileBehaviourPrefab, position, tableCreationData.Rotation, tableParent);
        tileBehaviourTemp.InitializeTile(elementSprite, tableCreationData.TileIndex, elementCount, isEmpty);
    }


    /// <summary>
    /// Creates tile according to JSON Data.
    /// </summary>
    private void ProcessTiles_JSON(TableCreationData tableCreationData, SpriteData spriteData)
    {
        throw new NotImplementedException();
    }

    #region EVETN SUBSCRIPTION

    private void OnEnable()
    {
        GameSignals gameSignals = SO_Manager.Get<GameSignals>();
        gameSignals.OnGameStart += InitializeTable;
    }

    private void OnDisable()
    {
        GameSignals gameSignals = SO_Manager.Get<GameSignals>();
        gameSignals.OnGameStart -= InitializeTable;
    }

    #endregion
}