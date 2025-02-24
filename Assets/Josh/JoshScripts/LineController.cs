//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class LineController : MonoBehaviour
//{
//    private LineRenderer lr;
//    private Transform[] points;
//    [SerializeField] GameObject grap;
//    private GrapplingHook test;
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    private void Awake()
//    {
//        lr = GetComponent<LineRenderer>();
//        test = grap.GetComponent<GrapplingHook>();
//    }

//    public void SetUpLine(Transform[] points)
//    {
//        lr.positionCount = points.Length;
//        this.points = points;
//    }

//    // Update is called once per frame
//    void Update()
//    {

        
//        for (int i = 0; i < points.Length; i++)
//        {
//            lr.SetPosition(i, points[i].position);
//        }
        
//    }
//}
