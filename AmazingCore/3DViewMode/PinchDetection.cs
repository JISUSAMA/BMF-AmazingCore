using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class PinchDetection : MonoBehaviour
{

    private InputManager inputManager;

    public float cameratSpeed = 4f;    
    private Coroutine zoomCoroutine;    
    private Transform cameraTrasform;

    [SerializeField] private CinemachineFreeLook freeLookCameratoZoom;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float zoomAcceleration = 2.5f;
    [SerializeField] private float zoomInnerRage = 3f;
    [SerializeField] private float zoomOuterRage = 7.5f;

    private float currentMiddleRigRadius = 7.5f;
    private float newMiddleRigRadius = 7.5f;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        cameraTrasform = Camera.main.transform;
    }

    private void OnEnable()
    {
        inputManager.OnPinchTouch += ZoomStart;
    }

    private void OnDisable()
    {
        inputManager.OnPinchTouch -= ZoomStart;
    }

    private void Start()
    {

    }

    private void ZoomStart(Finger finger, Finger finger2)
    {
        zoomCoroutine = StartCoroutine(_ZoomDetection(finger.screenPosition, finger2.screenPosition));
    }

    IEnumerator _ZoomDetection(Vector2 finger, Vector2 finger2)
    {
        float previousDistance = 0f, distance = 0f;

        while (true)
        {
            // 1. 거리계산
            distance = Vector2.Distance(finger, finger2);
            // 2. 거리가 멀어지냐
            // Zoom out
            if (distance > previousDistance)
            {
                currentMiddleRigRadius = Mathf.Lerp(currentMiddleRigRadius, newMiddleRigRadius + zoomSpeed, zoomAcceleration * Time.deltaTime);
                currentMiddleRigRadius = Mathf.Clamp(currentMiddleRigRadius, zoomInnerRage, zoomOuterRage);
                // m_Radius++
                freeLookCameratoZoom.m_Orbits[1].m_Radius = currentMiddleRigRadius;                     // middle
                freeLookCameratoZoom.m_Orbits[0].m_Radius = freeLookCameratoZoom.m_Orbits[1].m_Radius;  // top
                freeLookCameratoZoom.m_Orbits[2].m_Radius = freeLookCameratoZoom.m_Orbits[1].m_Radius; // bottom
            }
            // 3. 거리가 가까워지냐
            // Zoom in
            else if (distance < previousDistance)
            {
                currentMiddleRigRadius = Mathf.Lerp(currentMiddleRigRadius, newMiddleRigRadius - zoomSpeed, zoomAcceleration * Time.deltaTime);
                currentMiddleRigRadius = Mathf.Clamp(currentMiddleRigRadius, zoomInnerRage, zoomOuterRage);
                // m_Radius--
                freeLookCameratoZoom.m_Orbits[1].m_Radius = currentMiddleRigRadius;                     // middle
                freeLookCameratoZoom.m_Orbits[0].m_Radius = freeLookCameratoZoom.m_Orbits[1].m_Radius;  // top
                freeLookCameratoZoom.m_Orbits[2].m_Radius = freeLookCameratoZoom.m_Orbits[1].m_Radius; // bottom
            }

            previousDistance = distance;
            yield return null;
        }
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

        while (true)
        {
            //distance = Vector2.Distance(cameraControls.FreeLookCameraControls.PrimaryFingerPosition.ReadValue<Vector2>(),
            //                            cameraControls.FreeLookCameraControls.SecondaryFingerPosition.ReadValue<Vector2>());
            // Detection
            // Zoom out
            if (distance > previousDistance)
            {
                Vector3 targetPosition = cameraTrasform.position;
                targetPosition.z += 1f;
                cameraTrasform.position = Vector3.Slerp(cameraTrasform.position, targetPosition, Time.deltaTime * cameratSpeed);
            }
            // Zoom in
            else if (distance < previousDistance)
            {
                Vector3 targetPosition = cameraTrasform.position;
                targetPosition.z -= 1f;
                cameraTrasform.position = Vector3.Slerp(cameraTrasform.position, targetPosition, Time.deltaTime * cameratSpeed);
            }

            previousDistance = distance;
            yield return null;
        }
    }
}
