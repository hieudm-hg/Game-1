using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveController();
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        m_CellPosition = cell;
        transform.position = m_Board.CellToWorld(cell);
    }
    public void MoveController()
    {
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }
        if (hasMoved)
        {
            BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);
            if (cellData != null && cellData.Passable)
            {
                GameManager.Instance.m_Turn.Tick();
                if (cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget);
                }

                //m_CellPosition = newCellTarget;
                //transform.position = m_Board.CellToWorld(m_CellPosition);
                else if (cellData.ContainedObject.PlayerWantToEnter())
                {
                    MoveTo(newCellTarget);
                    cellData.ContainedObject.PlayerEnter();
                }
            }
        }

    }
    public void MoveTo(Vector2Int cell)
    {
        m_CellPosition = cell;
        transform.position = m_Board.CellToWorld(m_CellPosition);
    }


}
