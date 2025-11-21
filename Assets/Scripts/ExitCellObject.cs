using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitCellObject : CellObject
{
    public Tile EndTile;
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
        GameManager.Instance.m_Board.SetCellTile(cell, EndTile);
    }
    public override void PlayerEnter()
    {
        Debug.Log("Exit game");
        GameManager.Instance.NewLevel();
    }

}
