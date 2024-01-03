
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    public delegate void TouchEvent(Finger finger, float time); // handle
    public delegate void TwoTouchEvent(Finger primary_finger, Finger second_finger); // handle
    public event TouchEvent OnStartTouch;  // Event Point ( Call )
    public event TouchEvent OnMoveTouch;  // Event Point ( Call )    
    public event TwoTouchEvent OnPinchTouch;  // Event Point ( Call )    
    public event TouchEvent OnEndTouch;  // Event Point ( Call )
    //public delegate void EndTouchEvent(Vector2 position, float time); // handle
    //public event EndTouchEvent OnEndTouch;  // Event Point ( Call )
    private TouchControls touchControls;
    private InputAction mouseClick;

    public void Awake()
    {
        //base.Awake();   // base Awake (SingletonPersistent 안에 public virtual void Awake 있음) 부터 부르고 Awake 진행
        touchControls = new TouchControls();        
    }

    private void OnEnable()
    {        
        touchControls.Enable();
        //TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += FingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += FingerUp;
    }

    private void OnDisable()
    {
        touchControls.Disable();
        //TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= FingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= FingerUp;        
    }

    private void Start()
    {        
        //touchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);        
        //touchControls.Touch.TouchPress.performed += ctx => PerformedTouch(ctx);
        //touchControls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    private void Update()
    {
        //Debug.Log(UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches);
        //foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        //{
        //    Debug.Log(touch.phase == UnityEngine.InputSystem.TouchPhase.Began);
        //}
    }

    private void FingerDown(Finger finger)
    {        
        OnStartTouch?.Invoke(Touch.activeFingers[0], Time.time);
    }

    private void FingerMove(Finger finger)
    {
        if (Touch.activeTouches.Count == 1)
        {            
            OnMoveTouch?.Invoke(Touch.activeFingers[0], Time.time);
        }
        else if (Touch.activeTouches.Count == 2)
        {         
            OnPinchTouch?.Invoke(Touch.activeFingers[0], Touch.activeFingers[1]);
        }
    }

    private void FingerUp(Finger finger)
    {
        OnEndTouch?.Invoke(Touch.activeFingers[0], Time.time);
    }
    private void StartTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch started " + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        //OnStartTouch?.Invoke(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
    }
    private void PerformedTouch(InputAction.CallbackContext context)
    {
        Debug.Log("context.control.path : " + context.control.path);
        Debug.Log("context.performed : " + context.performed);
        Debug.Log("Touch performed " + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        //OnStartTouch?.Invoke(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
    }
    private void EndTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch ended " + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        //OnEndTouch?.Invoke(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
    }
}
