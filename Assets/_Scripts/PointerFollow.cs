using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerFollow : MonoBehaviour
{

    public Camera myCamera;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = myCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3 (mousePos.x, mousePos.y);
        transform.position = mousePos;
    }
}
