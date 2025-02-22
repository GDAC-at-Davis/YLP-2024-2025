using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class FightingCamera : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> targets;

    [SerializeField]
    private Vector2 padding;

    [SerializeField]
    private Vector2 offset;

    [SerializeField]
    private float lerpingRate = 0.005f;
    [SerializeField]
    private float cameraVelocityAnticipation = 1;

    private CinemachineCamera cam;

    private Vector2 topRightBound;
    private Vector2 bottomLeftBound;
    private Vector3 cameraFocusPoint;

    private void Start()
    {
        cam = gameObject.GetComponent<CinemachineCamera>();
    }

    private void Update()
    {
        //cam.transform.position += new Vector3(0.0f, 0, 0.1f);

        if (targets.Count == 1)
        {
            cam.transform.position = targets[0].transform.position + new Vector3(0.0f, 0, -15f);
        }

        FindCameraBounds();

        FindCameraDistance();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(topRightBound, 0.15f);
        Gizmos.DrawSphere(bottomLeftBound, 0.15f);
    }

    /// <summary>
    ///     Finds the smallest necessary box to cover all characters in combat.
    ///     Determines minimum camera width and height.
    /// </summary>
    private void FindCameraBounds()
    {
        if (targets.Count == 0)
        {
            return;
        }

        topRightBound = targets[0].transform.position;
        bottomLeftBound = targets[0].transform.position;

        foreach (GameObject target in targets)
        {
            Vector3 focusPoint = target.transform.position;

            Rigidbody2D targetBody = target.GetComponent<Rigidbody2D>();
            if (targetBody)
            {
                Vector3 targetVel = targetBody.linearVelocity;
                focusPoint += targetVel * cameraVelocityAnticipation;
            }


            if (focusPoint.x > topRightBound.x)
            {
                topRightBound.x = focusPoint.x;
            }

            if (focusPoint.y > topRightBound.y)
            {
                topRightBound.y = focusPoint.y;
            }

            if (focusPoint.x < bottomLeftBound.x)
            {
                bottomLeftBound.x = focusPoint.x;
            }

            if (focusPoint.y < bottomLeftBound.y)
            {
                bottomLeftBound.y = focusPoint.y;
            }
        }

        // Add padding
        topRightBound += padding;
        bottomLeftBound -= padding;

        // Offset
        topRightBound += offset;
        bottomLeftBound += offset;

        cameraFocusPoint = topRightBound / 2 + bottomLeftBound / 2;
        cameraFocusPoint.z = 0;
    }

    private void FindCameraDistance()
    {
        float height = topRightBound.y - bottomLeftBound.y;
        float width = topRightBound.x - bottomLeftBound.x;
        float cameraDistanceVertical = height / 2 / Mathf.Tan(Mathf.Deg2Rad * cam.Lens.FieldOfView / 2);

        // Calculate tangent of horizontal half FOV
        float horizontalHalfFovTangent = Mathf.Tan(Mathf.Deg2Rad * cam.Lens.FieldOfView / 2) * (16f / 9f);
        float cameraDistanceHorizontal = width / 2 / horizontalHalfFovTangent;

        // Find the largest distance
        float cameraDistance;
        if (cameraDistanceHorizontal > cameraDistanceVertical)
        {
            cameraDistance = cameraDistanceHorizontal;
        }
        else
        {
            cameraDistance = cameraDistanceVertical;
        }

        cam.transform.position = cam.transform.position * (1-lerpingRate) + lerpingRate * (cameraFocusPoint + new Vector3(0, 0, -cameraDistance));
    }

    public void SetTargets(IEnumerable<GameObject> gameObjects)
    {
        targets.Clear();
        targets.AddRange(gameObjects);
    }
}