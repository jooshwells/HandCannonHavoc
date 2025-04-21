using UnityEngine;

public class BookDropper3Lanes : MonoBehaviour
{
    public GameObject[] bookPrefabs;

    public Transform[] leftDropPoints;   // 2 Transforms (x = 0, x = 1)
    public Transform[] middleDropPoints; // 2 Transforms (x = 2, x = 3)
    public Transform[] rightDropPoints;  // 2 Transforms (x = 4, x = 5)

    public float dropInterval = 0.4f;
    public float laneSwitchInterval = 3f;

    private float dropTimer = 0f;
    private float switchTimer = 0f;
    private int currentLane = 0; // 0 = left, 1 = middle, 2 = right

    void Start()
    {
        PositionAtLane();
    }

    void Update()
    {
        dropTimer += Time.deltaTime;
        switchTimer += Time.deltaTime;

        if (dropTimer >= dropInterval)
        {
            DropBooks();
            dropTimer = 0f;
        }

        if (switchTimer >= laneSwitchInterval)
        {
            SwitchLane();
            PositionAtLane();
            switchTimer = 0f;
        }
    }

    void DropBooks()
    {
        Transform[] activeDropPoints = GetCurrentDropPoints();

        foreach (Transform dropPoint in activeDropPoints)
        {
            int index = Random.Range(0, bookPrefabs.Length);
            Instantiate(bookPrefabs[index], dropPoint.position, Quaternion.identity);
        }
    }

    void SwitchLane()
    {
        // Fixed rotation: Left -> Middle -> Right -> Left ...

        // Or for random switching (non-repeating), uncomment this:
        
        int newLane;
        do
        {
            newLane = Random.Range(0, 3);
        } while (newLane == currentLane);
        currentLane = newLane;
        
    }

    void PositionAtLane()
    {
        Transform[] lane = GetCurrentDropPoints();
        transform.position = lane[0].position;
    }

    Transform[] GetCurrentDropPoints()
    {
        switch (currentLane)
        {
            case 0: return leftDropPoints;
            case 1: return middleDropPoints;
            case 2: return rightDropPoints;
            default: return middleDropPoints;
        }
    }
}
