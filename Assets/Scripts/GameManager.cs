using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public BoardManager m_Board;
    public PlayerController c_Player;
    public UIDocument UIDoc;
    private Label m_FoodLabel;
    private int m_CurrentLevel = 1;

    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;

    private int m_FoodAmount = 20;

    public TurnManager m_Turn;

    private void Awake()
    {
        m_Turn = new TurnManager();
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");
        m_GameOverPanel.style.visibility = Visibility.Hidden;
        m_GameOverMessage.text = "";
        m_FoodLabel.text = "Food : " + m_FoodAmount;
        m_Turn.OnTick += OnTurnHappen;
        //m_Board.Init();
        //Debug.Log("Current amount of food : " + m_FoodAmount);
        //c_Player.Spawn(m_Board, new Vector2Int(1, 1));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {
        m_FoodAmount += amount;
        m_FoodLabel.text = "Food : " + m_FoodAmount;
        if (m_FoodAmount <= 0)
        {
            m_GameOverPanel.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = "Game Over!\n\nYou traveled through " + m_CurrentLevel + " levels";
        }
    }

    public void NewLevel()
    {
        m_Board.CleanMap();
        m_Board.Init();
        c_Player.Spawn(m_Board, new Vector2Int(1, 1));
        m_CurrentLevel++;
    }
}
