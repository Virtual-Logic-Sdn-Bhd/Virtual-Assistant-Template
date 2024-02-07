using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform followTarget;
    public float followSpeed = 5;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = followTarget.position - transform.position;
    }

    private void FixedUpdate()
    {
        if (followTarget)
        {
            transform.position = followTarget.position - offset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
