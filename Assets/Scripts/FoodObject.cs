public class FoodObject : CellObject
{
    public int AmountGranted = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void PlayerEnter()
    {
        //Debug.Log("Food Increased");
        Destroy(gameObject);
        GameManager.Instance.ChangeFood(AmountGranted);
    }
}
