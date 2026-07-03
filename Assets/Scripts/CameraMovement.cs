using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset _actionAsset;

    private InputActionMap _actionMap;
    private InputAction _rightMouseAction;
    private InputAction _deltaAction;

    private Rigidbody _rb;

    private float _rightBtnValue;
    private Vector2 _deltaValue;

    private void OnEnable()
    {
        _actionAsset.Enable();
    }

    private void OnDisable()
    {
        _actionAsset.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _actionMap = _actionAsset.FindActionMap("Player");
        _rightMouseAction = _actionMap.FindAction("ShouldLook");
        _deltaAction = _actionMap.FindAction("Look");

        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _rightBtnValue = _rightMouseAction.ReadValue<float>();
        _deltaValue = _deltaAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (_rightBtnValue != 0)
        {
            Vector2 rotAmt = _deltaValue * 10 * Time.deltaTime;
            Quaternion deltaRotation = Quaternion.Euler(rotAmt.y, rotAmt.x, 0);
            _rb.MoveRotation(_rb.rotation * deltaRotation);
        }
    }
}
