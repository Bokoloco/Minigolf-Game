using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallMovement : MonoBehaviour
{
    [SerializeField]
    private float _powerPerUnit = 2f;

    [SerializeField]
    private float _maxPower = 10f;

    [SerializeField]
    private InputActionAsset _actionAsset;

    private InputActionMap _actionMap;
    private InputAction _leftMouseAction;
    private InputAction _rightMouseAction;
    private InputAction _positionAction;

    private Rigidbody _rb;

    private LineRenderer _lineRenderer;

    private Vector2 _endPositionValue;
    private bool _shouldDrawLine;
    private Plane _groundPlane;

    private int _strokeCount = 0;

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
        _rightMouseAction = _actionMap.FindAction("RightMouse");
        _positionAction = _actionMap.FindAction("MousePosition");

        // Connect events to functions
        _leftMouseAction.started += StartVector;
        _leftMouseAction.canceled += AddForce;

        _rightMouseAction.started += StopVector;

        // Make groundplane for raycast
        _groundPlane = new Plane(Vector3.up, transform.position);
    }

    private void StartVector(InputAction.CallbackContext context)
    {
        // Check if ball is still moving
        if (_rb.linearVelocity.magnitude > 0.1)
            return;

        // Make line visible
        _shouldDrawLine = true;
        _lineRenderer.enabled = true;
    }

    private void StopVector(InputAction.CallbackContext context)
    {
        _shouldDrawLine = false;
        _lineRenderer.enabled = false;
    }

    private void AddForce(InputAction.CallbackContext context)
    {
        // No force needs to be added if right click was pressed
        if (!_shouldDrawLine)
            return;

        _endPositionValue = _positionAction.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(_endPositionValue);

        if (_groundPlane.Raycast(ray, out float distance))
        {
            // Get world position and calculate the direction
            Vector3 worldPositionEnd = ray.GetPoint(distance);
            Vector3 direction = transform.position - worldPositionEnd;
            direction.y = 0;

            // Clamp magnitude of the line so it can not go past the Max
            direction = Vector3.ClampMagnitude(direction, _maxPower);

            // Calculate the force and apply it to the rigid body
            float forceMultiplier = direction.magnitude * _powerPerUnit; 
            _rb.AddForce(direction.normalized * forceMultiplier, ForceMode.Impulse);
        }

        _shouldDrawLine = false;
        _lineRenderer.enabled = false;
        ++_strokeCount;
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
        _lineRenderer.endWidth = 0.02f;

        // Set the number of vertices
        _lineRenderer.positionCount = 2;

        // Make line invisible
        _lineRenderer.enabled = false;
    }

    void Update()
    {
        if (_shouldDrawLine)
        {
            // Get screen position
            var screenPosition = _positionAction.ReadValue<Vector2>();

            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            if (_groundPlane.Raycast(ray, out float distance))
            {
                // Get the correct world and start position
                Vector3 worldPosition = ray.GetPoint(distance);
                Vector3 start = transform.position;

                // Clamp magnitude of the line so it can not go past the Max
                Vector3 direction = worldPosition - start;
                direction = Vector3.ClampMagnitude(direction, _maxPower);
                worldPosition = start + direction;

                // Set the positions of the vertices
                _lineRenderer.SetPosition(0, start);
                _lineRenderer.SetPosition(1, worldPosition);
            }
        }
    }

    public int GetStrokeCount()
    {
        return _strokeCount;
    }
}
