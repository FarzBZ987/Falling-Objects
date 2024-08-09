using System;
using System.Collections;
using UnityEngine;

using UnityEngine.InputSystem.EnhancedTouch;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float m_Position;

    public delegate void onTouchChange(bool touchAvailable, float position);

    public static event onTouchChange onTouchChangeEvents;

    private InputControls m_InputControls;

    [SerializeField] private bool _isTouchAvailable;

    private bool touchAvailability
    {
        set
        {
            if (_isTouchAvailable != value)
            {
                _isTouchAvailable = value;
            }
            onTouchChangeEvents?.Invoke(value, m_Position);
        }
    }

    private void Awake()
    {
        m_InputControls = new InputControls();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        m_InputControls.Enable();
        TouchSimulation.Enable();
    }

    private void Start()
    {
        m_InputControls.Touchsreen.Touch.performed += Touch_performed;
        m_InputControls.Touchsreen.Touch.canceled += Touch_canceled;
    }

    private void Touch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_isTouchAvailable) return;
        if (!GameManager.gameStarted) return;
        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return null;
            Debug.Log("Touch Position " + m_InputControls.Touchsreen.TouchPosition.ReadValue<Vector2>());
            SetDirection(m_InputControls.Touchsreen.TouchPosition.ReadValue<Vector2>());
            touchAvailability = true;
        }
    }

    private void Touch_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        touchAvailability = false;
    }

    private void SetDirection(Vector2 finger)
    {
        var screen = Camera.main.pixelWidth / 2;

        m_Position = finger.x < screen ? -1 : 1;
    }

    private void OnDisable()
    {
        m_InputControls.Disable();

        TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();
    }

    private void OnDestroy()
    {
        m_InputControls.Touchsreen.Touch.performed -= Touch_performed;
        m_InputControls.Touchsreen.Touch.canceled -= Touch_canceled;
    }
}