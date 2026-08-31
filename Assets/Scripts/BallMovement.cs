using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallMovement : MonoBehaviour
{
    
    [SerializeField] private float _powerPerUnit = 2f;
    
    [SerializeField]
    private InputActionAsset _actionAsset;

    private InputActionMap _actionMap;
    private InputAction _leftMouseAction;
    private InputAction _positionAction;

    //private BallPhysics _rb;
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
    }

    private void EndVector(InputAction.CallbackContext context)
    {
        _endPositionValue = _positionAction.ReadValue<Vector2>();

        var groundPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(_endPositionValue);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPositionEnd = ray.GetPoint(distance);
            Vector3 direction = transform.position - worldPositionEnd;
            direction.y = 0;
            
            float forceMultiplier = direction.magnitude * _powerPerUnit; 

            _rb.AddForce(direction.normalized * forceMultiplier, ForceMode.Impulse);
        }

        _ShouldDrawLine = false;
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
        _lineRenderer.endWidth = 0.02f;

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

            var groundPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 worldPosition = ray.GetPoint(distance);
                Vector3 start = transform.position;

                // Set the positions of the vertices
                _lineRenderer.SetPosition(0, start);
                _lineRenderer.SetPosition(1, worldPosition);
            }

            // Transform screen pos to world position
            //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
            
            

            
        }
    }
}
