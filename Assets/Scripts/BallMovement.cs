using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset _actionAsset;

    private InputActionMap _actionMap;
    private InputAction _leftMouseAction;
    private InputAction _positionAction;

    private Rigidbody _rb;

    private float _leftBtnValue;
    private Vector2 _startPositionValue;
    private Vector2 _endPositionValue;

    private void OnEnable()
    {
        _actionAsset.Enable();
    }

    private void OnDisable()
    {
        _actionAsset.Disable();
    }

    private void Awake()
    {
        _actionMap = _actionAsset.FindActionMap("Player");
        _leftMouseAction = _actionMap.FindAction("LeftMouse");
        _positionAction = _actionMap.FindAction("MousePosition");

        _leftMouseAction.started += StartVector;
        _leftMouseAction.canceled += EndVector;
    }

    private void StartVector(InputAction.CallbackContext context)
    {
        _startPositionValue = _positionAction.ReadValue<Vector2>();

        Debug.Log("Start: " + _startPositionValue);
    }

    private void EndVector(InputAction.CallbackContext context)
    {
        _endPositionValue = _positionAction.ReadValue<Vector2>();

        Debug.Log("End: " + _endPositionValue);

        Vector2 difference = _endPositionValue - _startPositionValue;

        Vector3 test = new Vector3(difference.x, 0, difference.y) * -1;

        _rb.AddForce(test);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
