// This script will control the movement of the robot to reach the target
// Dependent on TargetController.cs to set the location of target

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    [Header("Robot Settings")]
    [SerializeField, Range(0.1f, 10f)] private float speed = 1f;
    [SerializeField, Range(0.1f, 10f)] private float rotationSpeed = 2.5f;
    [SerializeField] private float reachDestinationThreshold = 0.5f;

    [Header("Robot Components")]
    [SerializeField] private Transform robotTransform;
    [SerializeField] private Transform cameraTransform;

    [Header("Follow Settings")]
    [SerializeField] private bool shouldFollowPlayer = false;
    [SerializeField] private float followDistance = 3f;

    private TargetController targetController; // Reference to the TargetController script

    private Transform playerTransform;

    private void Start()
    {
        UnityEngine.Debug.Log("Robot is started");
        playerTransform = cameraTransform;

        // Get the TargetController script from the GameObject with the TargetController component
        targetController = GetComponent<TargetController>();

        if (targetController == null)
        {
            UnityEngine.Debug.LogError("TargetController script not found on the GameObject.");
        }
        else
        {
            // Initialize the TargetController
            targetController.Initialize();
        }
    }

    private void Update()
    {
        //UnityEngine.Debug.Log("Robot is updating");

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 cameraMovement = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime;
        cameraTransform.Translate(cameraMovement);

        if (shouldFollowPlayer)
        {
            Vector3 moveDirection = (playerTransform.position - robotTransform.position).normalized;
            Vector3 targetPosition = playerTransform.position - (moveDirection * followDistance);
            Vector3 newPosition = targetPosition + (cameraTransform.forward * followDistance);
            robotTransform.position = Vector3.Lerp(robotTransform.position, newPosition, speed * Time.deltaTime);
            robotTransform.rotation = Quaternion.LookRotation(cameraTransform.forward);
        }
        else
        {
            // Use TargetController's Update method to handle movement between targets
            if (targetController != null)
            {
               targetController.UpdateMovement(robotTransform, speed, rotationSpeed, reachDestinationThreshold);
            }
        }
    }
}
