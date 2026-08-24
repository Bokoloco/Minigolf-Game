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

    private LineRenderer _lineRenderer;

    private float _leftBtnValue;
    private Vector2 _startPositionValue;
    private Vector2 _endPositionValue;

    private bool _ShouldDrawLine;

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
        // Find the correct action maps
        _actionMap = _actionAsset.FindActionMap("Player");
        _leftMouseAction = _actionMap.FindAction("LeftMouse");
        _positionAction = _actionMap.FindAction("MousePosition");

        // Connect events to functions
        _leftMouseAction.started += StartVector;
        _leftMouseAction.canceled += EndVector;
    }

    private void StartVector(InputAction.CallbackContext context)
    {
        _startPositionValue = _positionAction.ReadValue<Vector2>();
        _ShouldDrawLine = true;

        // Make line visible
        _lineRenderer.enabled = true;

        //Debug.Log("Start: " + _startPositionValue);
    }

    private void EndVector(InputAction.CallbackContext context)
    {
        _endPositionValue = _positionAction.ReadValue<Vector2>();

        Vector2 difference = _endPositionValue - _startPositionValue;

        Vector3 test = new Vector3(difference.x, 0, difference.y) * -1;

        _rb.AddForce(test);

        _ShouldDrawLine = false;

        // Make line invisible
        _lineRenderer.enabled = false;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // Add a LineRenderer component
        _lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set the material
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;

        // Set the width
        _lineRenderer.startWidth = 0.02f;
        _lineRenderer.endWidth = 0.002f;

        // Set the number of vertices
        _lineRenderer.positionCount = 2;

        // Make line invisible
        _lineRenderer.enabled = false;
    }

    void Update()
    {
        if (_ShouldDrawLine)
        {
            // Get screen position
            var screenPosition = _positionAction.ReadValue<Vector2>();

            // Transform screen pos to world position
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
            
            Vector3 start = transform.position;

            // Set the positions of the vertices
            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, worldPosition);
        }
    }
}
