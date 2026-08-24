using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class BallPhysics : MonoBehaviour
{
    private CharacterController _characterController;

    [SerializeField]
    private float _gravity = 20f;

    [SerializeField]
    private float _dampening;

    private float _speed = 10f;

    private Vector3 _moveDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _moveDirection.y -= _gravity * Time.fixedDeltaTime;
        _characterController.Move(_moveDirection * Time.fixedDeltaTime * _speed);

        _speed = _speed - (_dampening * Time.fixedDeltaTime) >= 0f ? _speed - (_dampening * Time.fixedDeltaTime) : 0f;
    }

    public void ShootBall(Vector3 direction)
    {
        _speed = 10f;
        _moveDirection = direction.normalized;
    }
}
