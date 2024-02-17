using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float acceleration = 10.0f;
    public float deceleration = 5.0f;

    private GameActionMap _gameActionMap;
    private Vector3 currentVelocity;
    private Rigidbody _rb;

    private Animator _animator;
    private float currentSpeed;

    private void Awake()
    {
        _gameActionMap = new GameActionMap();

        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _gameActionMap.Enable();
    }

    private void Update()
    {
        Vector2 inputDirection = _gameActionMap.Player.Move.ReadValue<Vector2>();

        Vector3 targetVelocity = new Vector3(inputDirection.x, 0.0f, inputDirection.y) * moveSpeed;

        currentSpeed = Mathf.Lerp(currentSpeed, targetVelocity.magnitude, acceleration * Time.deltaTime);
        _animator.SetFloat("move", currentSpeed);

        if (targetVelocity != Vector3.zero)
        {
            Vector3 normalizedVelocity = targetVelocity.normalized;

            float targetAngle = Mathf.Atan2(normalizedVelocity.x, normalizedVelocity.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.deltaTime);

        if (inputDirection == Vector2.zero)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        _rb.velocity = currentVelocity;
    }
}