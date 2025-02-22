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

        if (targets.Count > 0)
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
        topRightBound = targets[0].transform.position;
        bottomLeftBound = targets[0].transform.position;

        foreach (GameObject target in targets)
        {
            if (target.transform.position.x > topRightBound.x)
            {
                topRightBound.x = target.transform.position.x;
            }

            if (target.transform.position.y > topRightBound.y)
            {
                topRightBound.y = target.transform.position.y;
            }

            if (target.transform.position.x < bottomLeftBound.x)
            {
                bottomLeftBound.x = target.transform.position.x;
            }

            if (target.transform.position.y < bottomLeftBound.y)
            {
                bottomLeftBound.y = target.transform.position.y;
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

        cam.transform.position = cameraFocusPoint + new Vector3(0, 0, -cameraDistance);
    }

    public void SetTargets(IEnumerable<GameObject> gameObjects)
    {
        targets.Clear();
        targets.AddRange(gameObjects);
    }
}