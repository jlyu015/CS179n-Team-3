using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundaries : MonoBehaviour
{
    private Vector2 bounds;

    // Start is called before the first frame update
    void Start()
    {
        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, bounds.x * -1, bounds.x);
        viewPos.y = Mathf.Clamp(viewPos.y, bounds.y * -1, bounds.y);
        transform.position = viewPos;
    }
}
