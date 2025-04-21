using UnityEngine;

public class BookDropperBlock : MonoBehaviour
{
    public GameObject[] bookPrefabs;
    public Transform[] leftDropPoints;  // 2 Transforms (x = 0, x = 1)
    public Transform[] rightDropPoints; // 2 Transforms (x = 2, x = 3)
    public float dropInterval = 0.4f;
    public float laneSwitchInterval = 3f;

    private float dropTimer = 0f;
    private float switchTimer = 0f;
    private bool isOnLeft = true;

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
            isOnLeft = !isOnLeft;
            PositionAtLane();
            switchTimer = 0f;
        }
    }

    void DropBooks()
    {
        Transform[] activeDropPoints = isOnLeft ? leftDropPoints : rightDropPoints;

        foreach (Transform dropPoint in activeDropPoints)
        {
            int index = Random.Range(0, bookPrefabs.Length);
            Instantiate(bookPrefabs[index], dropPoint.position, Quaternion.identity);
        }
    }

    void PositionAtLane()
    {
        // Update the visual position of the red block
        Vector3 newPosition = isOnLeft ? leftDropPoints[0].position : rightDropPoints[0].position;
        transform.position = newPosition; // Optional if you're using the red block to visually show it
    }
}
