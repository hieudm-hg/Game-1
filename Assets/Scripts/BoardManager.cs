using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap m_Tilemap;
    private Grid m_Grid;

    public PlayerController Player;
    public FoodObject[] FoodFrefab;
    public WallObject WallPrefab;
    public List<Vector2Int> m_EmptyCellsList;
    public ExitCellObject ExitCell;

    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject;
    }

    private CellData[,] m_BoardData;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    private void Awake()
    {
        //m_Tilemap = GetComponentInChildren<Tilemap>();
        //m_Grid = GetComponentInChildren<Grid>();
        //m_BoardData = new CellData[Width, Height];
        //m_EmptyCellsList = new List<Vector2Int>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();
        //ExitCell = GetComponentInChildren<ExitCellObject>();
        m_BoardData = new CellData[Width, Height];
        m_EmptyCellsList = new List<Vector2Int>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        //Debug.Log($"Tilemap found? {m_Tilemap != null}, Grid found? {m_Grid != null}");
        //Debug.Log($"Player reference: {(Player == null ? "NULL ❌" : "Found ✅")}");

        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                Tile tile;
                m_BoardData[x, y] = new CellData();
                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    tile = WallTiles[Random.Range(0, WallTiles.Length)];
                    m_BoardData[x, y].Passable = false;
                }
                else
                {
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_BoardData[x, y].Passable = true;
                    m_EmptyCellsList.Add(new Vector2Int(x, y));
                }
                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
            //Debug.Log($"WallTiles: {(WallTiles == null ? "NULL ❌" : WallTiles.Length.ToString() + " tiles ✅")}");
            //Debug.Log($"GroundTiles: {(GroundTiles == null ? "NULL ❌" : GroundTiles.Length.ToString() + " tiles ✅")}");

        }

        //m_EmptyCellsList.Remove(new Vector2Int(1, 1));

        Player.Spawn(this, new Vector2Int(1, 1));
        m_EmptyCellsList.Remove(new Vector2Int(1, 1));

        Vector2Int endCoord = new Vector2Int(Width - 2, Height - 2);
        AddObject(Instantiate(ExitCell), endCoord);
        m_EmptyCellsList.Remove(endCoord);

        GenerateWall();
        GenerateFood();
    }

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= Width || cellIndex.y < 0 || cellIndex.y >= Height)
        {
            return null;
        }
        return m_BoardData[cellIndex.x, cellIndex.y];
    }
    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_Tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }


    public void GenerateFood()
    {
        int foodCount = 5;
        for (int i = 0; i < foodCount; ++i)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];
            m_EmptyCellsList.RemoveAt(randomIndex);

            int randomFoodIndex = Random.Range(0, FoodFrefab.Length);
            FoodObject selectFood = FoodFrefab[randomFoodIndex];

            CellObject newFood = Instantiate(selectFood);
            newFood.transform.position = CellToWorld(coord);

            CellData data = m_BoardData[coord.x, coord.y];
            data.ContainedObject = newFood;
        }
    }

    void GenerateWall()
    {
        int wallCount = Random.Range(6, 10);
        for (int i = 0; i < wallCount; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            CellData data = m_BoardData[coord.x, coord.y];
            WallObject newWall = Instantiate(WallPrefab);

            AddObject(newWall, coord);

            //newWall.Init(coord);

            //newWall.transform.position = CellToWorld(coord);
            //data.ContainedObject = newWall;
        }
    }

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }

    void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = m_BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }

    public void CleanMap()
    {
        if (m_BoardData == null)
        {
            return;
        };

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var cellData = m_BoardData[x, y];
                if (cellData.ContainedObject != null)
                {
                    Destroy(cellData.ContainedObject.gameObject);
                }

                SetCellTile(new Vector2Int(x, y), null);
            }
        }
    }

}
