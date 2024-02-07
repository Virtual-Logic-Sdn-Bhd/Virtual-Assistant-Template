using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{

    // NavMeshAgent variable control player movement
    public NavMeshAgent playerNavMeshAgent;

    // A camera that follow the player movement
    public Camera playerCamera;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if the left button is clicked
        if (Input.GetMouseButton(0))
        {
            // Unity cast a ray from the position of mouse cursor on-screen toward the 3D scene
            Ray myRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit myRaycastHit;

            if (Physics.Raycast(myRay, out myRaycastHit))
            {
                // Assign ray hit point as Destination of NavMesh Agent (Player)
                playerNavMeshAgent.SetDestination(myRaycastHit.point);
            }
        }

    }
}
