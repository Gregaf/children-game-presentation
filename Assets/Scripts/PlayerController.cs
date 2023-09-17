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
    private float currentSpeed; // Current speed value for animator

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

        // Calculate target velocity based on input direction and move speed
        Vector3 targetVelocity = new Vector3(inputDirection.x, 0.0f, inputDirection.y) * moveSpeed;

        // Update the current speed for animator
        currentSpeed = Mathf.Lerp(currentSpeed, targetVelocity.magnitude, acceleration * Time.deltaTime);
        _animator.SetFloat("move", currentSpeed);

        // Check if there's any movement input
        if (targetVelocity != Vector3.zero)
        {
            Vector3 normalizedVelocity = targetVelocity.normalized;

            // Calculate the rotation angle based on the movement direction
            float targetAngle = Mathf.Atan2(normalizedVelocity.x, normalizedVelocity.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

            // Rotate the character smoothly
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Smoothly interpolate to the target velocity
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.deltaTime);

        // Apply deceleration when there is no input
        if (inputDirection == Vector2.zero)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        // Move the player
        _rb.velocity = currentVelocity;
    }
}