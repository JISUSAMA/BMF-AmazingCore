// GENERATED AUTOMATICALLY FROM 'Assets/01.Scripts/3DViewMode/CameraControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CameraControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @CameraControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraControls"",
    ""maps"": [
        {
            ""name"": ""Free Look Camera Controls"",
            ""id"": ""d8202c4b-c12f-4308-b06e-34af2e185573"",
            ""actions"": [
                {
                    ""name"": ""PrimaryFingerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""7b9eae64-e889-42d1-af22-843e7e3b5294"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryFingerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""7ee31f23-81ba-4289-8566-783208232605"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryTouchContact"",
                    ""type"": ""Button"",
                    ""id"": ""c9d6b174-e164-4d39-a7f2-a706cababc38"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""MouseZoom"",
                    ""type"": ""PassThrough"",
                    ""id"": ""95de537b-d76f-4e0e-a06c-c7aa32d3bdab"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2bf7cc4b-1c94-4cae-a108-ca852b3fab98"",
                    ""path"": ""<Touchscreen>/touch0/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryFingerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7418972a-f692-4691-b03f-180f740983f8"",
                    ""path"": ""<Touchscreen>/touch1/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryFingerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""106f05f3-bbd2-40a9-b0af-b3ed502a4119"",
                    ""path"": ""<Touchscreen>/touch1/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryTouchContact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9afef9d8-25b8-4613-86f2-d43365bc3cf5"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseZoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Free Look Camera Controls
        m_FreeLookCameraControls = asset.FindActionMap("Free Look Camera Controls", throwIfNotFound: true);
        m_FreeLookCameraControls_PrimaryFingerPosition = m_FreeLookCameraControls.FindAction("PrimaryFingerPosition", throwIfNotFound: true);
        m_FreeLookCameraControls_SecondaryFingerPosition = m_FreeLookCameraControls.FindAction("SecondaryFingerPosition", throwIfNotFound: true);
        m_FreeLookCameraControls_SecondaryTouchContact = m_FreeLookCameraControls.FindAction("SecondaryTouchContact", throwIfNotFound: true);
        m_FreeLookCameraControls_MouseZoom = m_FreeLookCameraControls.FindAction("MouseZoom", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Free Look Camera Controls
    private readonly InputActionMap m_FreeLookCameraControls;
    private IFreeLookCameraControlsActions m_FreeLookCameraControlsActionsCallbackInterface;
    private readonly InputAction m_FreeLookCameraControls_PrimaryFingerPosition;
    private readonly InputAction m_FreeLookCameraControls_SecondaryFingerPosition;
    private readonly InputAction m_FreeLookCameraControls_SecondaryTouchContact;
    private readonly InputAction m_FreeLookCameraControls_MouseZoom;
    public struct FreeLookCameraControlsActions
    {
        private @CameraControls m_Wrapper;
        public FreeLookCameraControlsActions(@CameraControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @PrimaryFingerPosition => m_Wrapper.m_FreeLookCameraControls_PrimaryFingerPosition;
        public InputAction @SecondaryFingerPosition => m_Wrapper.m_FreeLookCameraControls_SecondaryFingerPosition;
        public InputAction @SecondaryTouchContact => m_Wrapper.m_FreeLookCameraControls_SecondaryTouchContact;
        public InputAction @MouseZoom => m_Wrapper.m_FreeLookCameraControls_MouseZoom;
        public InputActionMap Get() { return m_Wrapper.m_FreeLookCameraControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FreeLookCameraControlsActions set) { return set.Get(); }
        public void SetCallbacks(IFreeLookCameraControlsActions instance)
        {
            if (m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface != null)
            {
                @PrimaryFingerPosition.started -= m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface.OnPrimaryFingerPosition;
                @PrimaryFingerPosition.performed -= m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface.OnPrimaryFingerPosition;
                @PrimaryFingerPosition.canceled -= m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface.OnPrimaryFingerPosition;
                @SecondaryFingerPosition.started -= m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface.OnSecondaryFingerPosition;
                @SecondaryFingerPosition.performed -= m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface.OnSecondaryFingerPosition;
                @SecondaryFingerPosition.canceled -= m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface.OnSecondaryFingerPosition;
                @SecondaryTouchContact.started -= m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface.OnSecondaryTouchContact;
                @SecondaryTouchContact.performed -= m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface.OnSecondaryTouchContact;
                @SecondaryTouchContact.canceled -= m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface.OnSecondaryTouchContact;
                @MouseZoom.started -= m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface.OnMouseZoom;
                @MouseZoom.performed -= m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface.OnMouseZoom;
                @MouseZoom.canceled -= m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface.OnMouseZoom;
            }
            m_Wrapper.m_FreeLookCameraControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PrimaryFingerPosition.started += instance.OnPrimaryFingerPosition;
                @PrimaryFingerPosition.performed += instance.OnPrimaryFingerPosition;
                @PrimaryFingerPosition.canceled += instance.OnPrimaryFingerPosition;
                @SecondaryFingerPosition.started += instance.OnSecondaryFingerPosition;
                @SecondaryFingerPosition.performed += instance.OnSecondaryFingerPosition;
                @SecondaryFingerPosition.canceled += instance.OnSecondaryFingerPosition;
                @SecondaryTouchContact.started += instance.OnSecondaryTouchContact;
                @SecondaryTouchContact.performed += instance.OnSecondaryTouchContact;
                @SecondaryTouchContact.canceled += instance.OnSecondaryTouchContact;
                @MouseZoom.started += instance.OnMouseZoom;
                @MouseZoom.performed += instance.OnMouseZoom;
                @MouseZoom.canceled += instance.OnMouseZoom;
            }
        }
    }
    public FreeLookCameraControlsActions @FreeLookCameraControls => new FreeLookCameraControlsActions(this);
    public interface IFreeLookCameraControlsActions
    {
        void OnPrimaryFingerPosition(InputAction.CallbackContext context);
        void OnSecondaryFingerPosition(InputAction.CallbackContext context);
        void OnSecondaryTouchContact(InputAction.CallbackContext context);
        void OnMouseZoom(InputAction.CallbackContext context);
    }
}
