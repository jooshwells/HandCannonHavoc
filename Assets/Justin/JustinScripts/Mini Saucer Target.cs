using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSaucerTarget : MonoBehaviour
{
    Transform transform;
    [SerializeField] Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(parent.position.x, parent.position.y + 5f);
    }
}
