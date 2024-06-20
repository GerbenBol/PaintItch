using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCamera : MonoBehaviour
{
    [SerializeField] private List<Transform> checkpoints;
    [SerializeField] private float speed;

    private int targetIndex = 0;
    private bool firstRotationDone = false;
    private Transform previousCheck;
    private Transform targetCheck;

    private float timeToCheck;
    private float elapsedTime = .0f;

    private void Start()
    {
        previousCheck = checkpoints[^1];
        targetCheck = checkpoints[targetIndex]; 
    }

    private void Update()
    {
        if (Time.timeScale > .0f)
        {
            elapsedTime += Time.deltaTime;
            float elapsedPercentage = elapsedTime / timeToCheck;
            elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
            Vector3 newPos = Vector3.Lerp(previousCheck.position, targetCheck.position, elapsedPercentage);
            Quaternion newRot = Quaternion.Lerp(previousCheck.rotation, targetCheck.rotation, elapsedPercentage);
            transform.SetPositionAndRotation(newPos, newRot);

            if (elapsedPercentage >= 1)
                TargetNextCheckpoint();
        }
    }

    private void TargetNextCheckpoint()
    {
        if (firstRotationDone)
            speed = 3;

        previousCheck = targetCheck;

        if (targetIndex + 1 < checkpoints.Count)
            targetIndex++;
        else
        {
            firstRotationDone = true;
            targetIndex = 0;
        }

        targetCheck = checkpoints[targetIndex];
        elapsedTime = .0f;
        float distanceToCheck = Vector3.Distance(previousCheck.position, targetCheck.position);
        timeToCheck = distanceToCheck / speed;
    }
}
