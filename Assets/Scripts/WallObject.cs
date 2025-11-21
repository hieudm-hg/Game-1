using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{

    public Tile ObstacleTile;
    public int MaxHealth = 3;
    public int m_HealthPoin;
    public Tile m_OriginalTile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        m_HealthPoin = MaxHealth;
        m_OriginalTile = GameManager.Instance.m_Board.GetCellTile(cell);
        GameManager.Instance.m_Board.SetCellTile(cell, ObstacleTile);
    }

    public override bool PlayerWantToEnter()
    {
        m_HealthPoin -= 1;
        if (m_HealthPoin > 0)
        {
            return false;
        }
        GameManager.Instance.m_Board.SetCellTile(m_Cell, m_OriginalTile);
        Destroy(gameObject);
        return true;
    }
}
