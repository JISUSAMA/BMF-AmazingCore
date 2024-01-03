using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectZoomControls : MonoBehaviour
{
    //[SerializeField] private InputActionAsset inputProvider;
    [SerializeField] private CinemachineFreeLook freeLookCameratoZoom;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float zoomAcceleration = 2.5f;
    [SerializeField] private float zoomInnerRange = 3f;
    [SerializeField] private float zoomOuterRange = 5.5f;

    private float currentMiddleRigRadius = 5.5f;
    private float newMiddleRigRadius = 5.5f;

    //[SerializeField] private float zoomYAxis = 0;

    private CameraControls cameraControls;
    private Coroutine zoomCoroutine;

    private void Awake()
    {
        cameraControls = new CameraControls();
    }
    private void OnEnable()
    {
        cameraControls.Enable();
    }

    private void OnDisable()
    {
        cameraControls.Disable();
    }

    private void Start()
    {
        cameraControls.FreeLookCameraControls.SecondaryTouchContact.started += _ => ZoomStart();
        cameraControls.FreeLookCameraControls.SecondaryTouchContact.canceled += _ => ZoomEnd();
    }

    private void ZoomStart()
    {
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }
    private void ZoomEnd()
    {
        StopCoroutine(zoomCoroutine);
    }

    IEnumerator ZoomDetection()
    {
        float previousDistance = 0f, distance = 0f;
        Debug.Log("ZoomDetection");
        while (true)
        {
            distance = Vector2.Distance(cameraControls.FreeLookCameraControls.PrimaryFingerPosition.ReadValue<Vector2>(),
                                        cameraControls.FreeLookCameraControls.SecondaryFingerPosition.ReadValue<Vector2>());            
            // Detection
            // Zoom out
            if (distance > previousDistance)
            {                
                currentMiddleRigRadius = Mathf.Lerp(currentMiddleRigRadius, newMiddleRigRadius + zoomSpeed, zoomAcceleration * Time.deltaTime);
                currentMiddleRigRadius = Mathf.Clamp(currentMiddleRigRadius, zoomInnerRange, zoomOuterRange);
                // m_Radius--
                freeLookCameratoZoom.m_Orbits[1].m_Radius = currentMiddleRigRadius;                     // middle
                freeLookCameratoZoom.m_Orbits[0].m_Radius = freeLookCameratoZoom.m_Orbits[1].m_Radius;  // top
                freeLookCameratoZoom.m_Orbits[2].m_Radius = freeLookCameratoZoom.m_Orbits[1].m_Radius; // bottom
                Debug.Log($"Zoom Out {freeLookCameratoZoom.m_Orbits[1].m_Radius}");
                //Vector3 targetPosition = cameraTrasform.position;
                //targetPosition.z += 1f;
                //cameraTrasform.position = Vector3.Slerp(cameraTrasform.position, targetPosition, Time.deltaTime * cameratSpeed);
            }
            // Zoom in
            else if (distance < previousDistance)
            {                
                currentMiddleRigRadius = Mathf.Lerp(currentMiddleRigRadius, newMiddleRigRadius - zoomSpeed, zoomAcceleration * Time.deltaTime);
                currentMiddleRigRadius = Mathf.Clamp(currentMiddleRigRadius, zoomInnerRange, zoomOuterRange);
                // m_Radius--
                freeLookCameratoZoom.m_Orbits[1].m_Radius = currentMiddleRigRadius;                     // middle
                freeLookCameratoZoom.m_Orbits[0].m_Radius = freeLookCameratoZoom.m_Orbits[1].m_Radius;  // top
                freeLookCameratoZoom.m_Orbits[2].m_Radius = freeLookCameratoZoom.m_Orbits[1].m_Radius; // bottom
                Debug.Log($"Zoom in {freeLookCameratoZoom.m_Orbits[1].m_Radius}");
                //Vector3 targetPosition = cameraTrasform.position;
                //targetPosition.z -= 1f;
                //cameraTrasform.position = Vector3.Slerp(cameraTrasform.position, targetPosition, Time.deltaTime * cameratSpeed);
            }

            previousDistance = distance;
            yield return null;
        }
    }
}
