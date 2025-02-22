using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class FightingCamera : MonoBehaviour
{
    private CinemachineCamera cam;

    [SerializeField] List<GameObject> targets;

    private Vector2 topRightBound;
    private Vector2 bottomLeftBound;
    private Vector3 CameraFocusPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = gameObject.GetComponent<CinemachineCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        //cam.transform.position += new Vector3(0.0f, 0, 0.1f);

        if (targets.Count > 0)
        {
            cam.transform.position = targets[0].transform.position + new Vector3(0.0f, 0, -15f);
        }

        findCameraBounds();

        findCameraDistance();


    }

    // Finds the smallest necessary box to cover all characters in combat.
    // Determines minimum camera width and height.
    void findCameraBounds()
    {
        topRightBound = targets[0].transform.position;
        bottomLeftBound = targets[0].transform.position;

        foreach (GameObject target in targets)
        {
            if (target.transform.position.x > topRightBound.x)
                topRightBound.x = target.transform.position.x;
            if (target.transform.position.y > topRightBound.y)
                topRightBound.y = target.transform.position.y;
            
            if (target.transform.position.x < bottomLeftBound.x)
                bottomLeftBound.x = target.transform.position.x;
            if (target.transform.position.y < bottomLeftBound.y)
                bottomLeftBound.y = target.transform.position.y;

            
        }


        CameraFocusPoint = topRightBound / 2 + bottomLeftBound / 2;
        CameraFocusPoint.z = 0;
    }

    void findCameraDistance()
    {
        float height = topRightBound.y - bottomLeftBound.y;
        float width = topRightBound.x - bottomLeftBound.x;
        float cameraDistanceVertical = (height/2) / Mathf.Tan( Mathf.Deg2Rad * cam.Lens.FieldOfView/2 );
        float horizontalFOV = Mathf.Tan( Mathf.Deg2Rad * cam.Lens.FieldOfView / 2) * (16/16);
        Debug.Log(horizontalFOV * Mathf.Rad2Deg);
        float cameraDistanceHorizontal = (width/2) / (horizontalFOV) / 2;

        float cameraDistance;

        if (cameraDistanceHorizontal > cameraDistanceVertical)
        {
            cameraDistance = cameraDistanceHorizontal;
        }
        else
        {
            cameraDistance = cameraDistanceVertical;
        }

        cam.transform.position = CameraFocusPoint + new Vector3(0, 0, -cameraDistance);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(topRightBound, 0.15f);
        Gizmos.DrawSphere(bottomLeftBound, 0.15f);
    }
}
