using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LevelEditor;
using UnityEngine;
using Random = UnityEngine.Random;

#region Table & Tile Data

public record TableCreationData
{
    public int Row;
    public int Column;
    public int TileIndex;
    public Vector2Int GridSize;
    public Quaternion Rotation;
    public float Spacing;

    public TableCreationData(int row, int column, int tileIndex, Vector2Int gridSize, Quaternion rotation,
        float spacing)
    {
        Row = row;
        Column = column;
        TileIndex = tileIndex;
        GridSize = gridSize;
        Rotation = rotation;
        Spacing = spacing;
    }
}

public record TileData
{
    public TileBehaviour TileBehaviour;
    public Vector3 Position;
    public SelectedElement SelectedElement;
    public int ElementCount;
}

#endregion

#region JSON

[Serializable]
public class JSONGridSize
{
    public int x;
    public int y;
}

[Serializable]
public class JSONElement
{
    public int selectedElement;
    public int elementCount;
}

[Serializable]
public class JSONLevelData
{
    public string levelName;
    public JSONGridSize gridSize;
    public List<JSONElement> Elements;
}

#endregion

public class TableManager : MonoBehaviour
{
    #region FIELDS

    [Header("Level")] [SerializeField] private LevelType levelType;
    [SerializeField] private LevelDataType levelDataType;

    private LevelDatas _levelDatas;
    private LevelData _levelData;
    private Element[] _tableElements;
    private JSONLevelData _jsonLevelData;

    private List<TileData> _tileDatas = new List<TileData>();

    [Header("Table Creation")] [SerializeField]
    private Transform tableParent;

    [SerializeField] private TileBehaviour tileBehaviourPrefab;

    #endregion

    private void InitializeTable()
    {
        Vector2Int gridSize;
        if (levelDataType == LevelDataType.Random || levelDataType == LevelDataType.ScriptableObject)
        {
            _levelDatas = SO_Manager.Get<LevelDatas>();
            _levelData = _levelDatas.GetLevelData(levelType);
            gridSize = _levelData.gridSize;
            _tableElements = _levelData.Elements;
        }
        else
        {
            gridSize = LoadJson();
        }

        CreateTables(gridSize, 1.05f);
    }

    private Vector2Int LoadJson()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("Json/LevelData");
        _jsonLevelData = JsonUtility.FromJson<JSONLevelData>(jsonText.text);

        return new Vector2Int(1, _jsonLevelData.Elements.Count);
    }

    private async Task CreateTables(Vector2Int gridSize, float spacing)
    {
        int maxRow = gridSize.x;
        int maxColumn = gridSize.y;
        int index = 1;
        int delay = 50;

        for (int i = 0; i < maxColumn; i++) // Sol kenar (aşağıdan yukarıya)
        {
            ProcessTile(new TableCreationData(0, i, index, gridSize, Quaternion.Euler(0, 90, 0), spacing));
            index++;
            await Task.Delay(delay);
        }

        PlayerSignals playerSignals = SO_Manager.Get<PlayerSignals>();
        if (levelType == LevelType.Line)
        {
            playerSignals.OnGameReadyToPlay?.Invoke(_tileDatas);
            return;
        }

        for (int i = 1; i < maxRow; i++) // Üst kenar (soldan sağa)
        {
            ProcessTile(new TableCreationData(i, maxColumn - 1, index, gridSize, Quaternion.Euler(0, 180, 0), spacing));
            index++;
            await Task.Delay(delay);
        }

        for (int i = maxColumn - 2; i >= 0; i--) // Sağ kenar (yukarıdan aşağıya)
        {
            ProcessTile(new TableCreationData(maxRow - 1, i, index, gridSize, Quaternion.Euler(0, 270, 0), spacing));
            index++;
            await Task.Delay(delay);
        }

        for (int i = maxRow - 2; i > 0; i--) // Alt kenar (sağdan sola)
        {
            ProcessTile(new TableCreationData(i, 0, index, gridSize, Quaternion.Euler(0, 0, 0), spacing));
            index++;
            await Task.Delay(delay);
        }

        playerSignals.OnGameReadyToPlay?.Invoke(_tileDatas);
    }

    #region Process Partials

    private void ProcessTile(TableCreationData tableCreationData)
    {
        SpriteData spriteData = SO_Manager.Get<SpriteData>();

        switch (levelDataType)
        {
            case LevelDataType.JSON:
                ProcessTiles_JSON(tableCreationData, spriteData);
                break;
            case LevelDataType.ScriptableObject:
                ProcessTiles_SO(tableCreationData, spriteData);
                break;
            case LevelDataType.Random:
                ProcessTiles_Random(tableCreationData, spriteData);
                break;
        }
    }

    private void ProcessTiles_SO(TableCreationData tableCreationData, SpriteData spriteData)
    {
        Element element = _tableElements
            [tableCreationData.Row * tableCreationData.GridSize.x + tableCreationData.Column];
        if (!element.isActive) return;

        SelectedElement selectedElement = element.selectedElement;
        Sprite elementSprite = spriteData.GetSprite(selectedElement);
        bool isEmpty = selectedElement == SelectedElement.Null;
        
        CreateTile(tableCreationData, selectedElement, element.elementCount, elementSprite, isEmpty);
    }

    private void ProcessTiles_Random(TableCreationData tableCreationData, SpriteData spriteData)
    {
        SelectedElement selectedElement = (SelectedElement)Random.Range(0, 4);
        Sprite elementSprite = spriteData.GetSprite(selectedElement);
        int elementCount = Random.Range(1, 26);
        bool isEmpty = selectedElement == SelectedElement.Null || Random.Range(0, 3) == 0;
        
        CreateTile(tableCreationData, selectedElement, elementCount, elementSprite, isEmpty);
    }
    
    private void ProcessTiles_JSON(TableCreationData tableCreationData, SpriteData spriteData)
    {
        int elementIndex = tableCreationData.Row * tableCreationData.GridSize.x + tableCreationData.Column;

        if (elementIndex >= _jsonLevelData.Elements.Count)
        {
            Debug.LogError("JSON Elements out of the array");
            return;
        }

        JSONElement jsonElement = _jsonLevelData.Elements[elementIndex];
        SelectedElement selectedElement = (SelectedElement)jsonElement.selectedElement;
        Sprite elementSprite = spriteData.GetSprite(selectedElement);
        bool isEmpty = selectedElement == (int)SelectedElement.Null;


        CreateTile(tableCreationData, selectedElement, jsonElement.elementCount, elementSprite, isEmpty);
    }
    
    
    #endregion

    #region TILE CREATION
    
   
    private IEnumerator AnimateTileMovementAndScale(TileBehaviour tile, float startY, float endY, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = tile.transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, endY, startPosition.z);

        Vector3 startScale = Vector3.zero;  // Başlangıçta scale değeri 0
        Vector3 endScale = Vector3.one;     // Bitişte scale değeri 1

        while (elapsedTime < duration)
        {
            float newY = Mathf.Lerp(startY, endY, elapsedTime / duration);
            tile.transform.position = new Vector3(startPosition.x, newY, startPosition.z);
            
            tile.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        tile.transform.position = endPosition;
        tile.transform.localScale = endScale;
    }



    private void CreateTile(TableCreationData tableCreationData, SelectedElement selectedElement, int elementCount,
        Sprite elementSprite, bool isEmpty)
    {
        Vector3 position = CalculatePosition(tableCreationData);
        
        TileBehaviour tileBehaviourTemp = Instantiate(tileBehaviourPrefab, position, tableCreationData.Rotation, tableParent);
        tileBehaviourTemp.InitializeTile(elementSprite, tableCreationData.TileIndex, elementCount, isEmpty);
        StartCoroutine(AnimateTileMovementAndScale(tileBehaviourTemp, -0.6f, -0.4f, 0.5f));
        
        AddTileData(tileBehaviourTemp, position, selectedElement, elementCount);
    }
    
    private Vector3 CalculatePosition(TableCreationData tableCreationData)
    {
        float spacing = tableCreationData.Spacing;
        float x = tableCreationData.Row * spacing;
        float z = tableCreationData.Column * spacing;
        return new Vector3(x, -0.4f, z);
    }
    
    private void AddTileData(TileBehaviour tileBehaviour, Vector3 position, SelectedElement selectedElement, int elementCount)
    {
        _tileDatas.Add(new TileData
        {
            TileBehaviour = tileBehaviour,
            Position = position,
            SelectedElement = selectedElement,
            ElementCount = elementCount
        });
    }

    #endregion

    #region EVENT SUBSCRIPTION

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