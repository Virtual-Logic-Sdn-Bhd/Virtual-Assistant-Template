// Use this script if the robot is following the camera
// Related to RobotController.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Task
{
    public Transform targetLocation; // The position where the robot should move to
    public string taskName; // Name of the task
    [TextArea] public string dialogText; // Dialog text associated with the task
    public TextMeshProUGUI dialogTextUI; // Reference to the TextMeshProUGUI component
}

public class TargetController : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private List<Task> targets;
    private Task currentTarget;
    private int targetIndex = 0;

    private float journeyLength;
    private float startTime;

    public bool HasTargetsRemaining => targetIndex < targets.Count;

    public void Initialize()
    {
        if (targets.Count > 0)
        {
            currentTarget = targets[0];
            // Calculate the total distance and store the start time for the first target
            journeyLength = Vector3.Distance(transform.position, currentTarget.targetLocation.position);
            startTime = Time.time;
        }
    }

    public void SetNextTarget()
    {
        if (HasTargetsRemaining)
        {
            currentTarget = targets[targetIndex];
            // Calculate the total distance and store the start time for the new target
            journeyLength = Vector3.Distance(transform.position, currentTarget.targetLocation.position);
            startTime = Time.time;
        }
    }

    // New method to set the robot's destination
    public void SetDestination(Vector3 destination)
    {
        // Set the current target's location to the specified destination
        currentTarget.targetLocation.position = destination;
        // Recalculate the journey length and start time
        journeyLength = Vector3.Distance(transform.position, currentTarget.targetLocation.position);
        startTime = Time.time;
    }

    public void UpdateMovement(Transform robotTransform, float speed, float rotationSpeed, float reachDestinationThreshold)
    {
        if (currentTarget == null)
        {
            // No more targets to move to
            return;
        }

        float distanceCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distanceCovered / journeyLength;
        fractionOfJourney = Mathf.Clamp01(fractionOfJourney);

        robotTransform.position = Vector3.Lerp(robotTransform.position, currentTarget.targetLocation.position, fractionOfJourney);

        // Update the TextMeshProUGUI component with the dialog text
        if (currentTarget.dialogTextUI != null)
        {
            currentTarget.dialogTextUI.text = currentTarget.dialogText;
        }

        Quaternion targetRotation = Quaternion.LookRotation(currentTarget.targetLocation.position - robotTransform.position);
        robotTransform.rotation = Quaternion.Slerp(robotTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        float distanceToTarget = Vector3.Distance(robotTransform.position, currentTarget.targetLocation.position);
        if (distanceToTarget <= reachDestinationThreshold)
        {
            targetIndex++;
            if (HasTargetsRemaining)
            {
                SetNextTarget();
            }
            else
            {
                // No more targets, stop moving
                currentTarget = null;
            }
        }
    }
}
