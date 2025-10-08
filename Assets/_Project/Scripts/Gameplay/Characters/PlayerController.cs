using UnityEngine;
using UnityEngine.InputSystem;

namespace TheCommunityFestival.Gameplay.Characters
{
    /// <summary>
    /// Player character controller with movement, interaction, and camera
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _walkSpeed = 5f;
        [SerializeField] private float _runSpeed = 8f;
        [SerializeField] private float _jumpHeight = 1.5f;
        [SerializeField] private float _gravity = -15f;

        [Header("Camera")]
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _mouseSensitivity = 2f;
        [SerializeField] private float _cameraMinY = -60f;
        [SerializeField] private float _cameraMaxY = 60f;

        [Header("Interaction")]
        [SerializeField] private float _interactionDistance = 3f;
        [SerializeField] private LayerMask _interactableLayers;

        private CharacterController _controller;
        private Vector3 _velocity;
        private bool _isGrounded;
        private float _cameraRotationX = 0f;

        // Input
        private Vector2 _moveInput;
        private bool _isRunning;
        private bool _jumpPressed;
        private Vector2 _lookInput;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            
            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Start()
        {
            // Setup camera if not assigned
            if (_cameraTransform == null)
            {
                Camera cam = GetComponentInChildren<Camera>();
                if (cam != null)
                {
                    _cameraTransform = cam.transform;
                }
            }
        }

        private void Update()
        {
            HandleMovement();
            HandleCamera();
            HandleInteraction();
            
            // Toggle cursor lock with Keyboard class (Input System)
            if (UnityEngine.InputSystem.Keyboard.current != null && 
                UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                ToggleCursorLock();
            }
        }

        #region Movement
        
        private void HandleMovement()
        {
            // Check if grounded
            _isGrounded = _controller.isGrounded;
            
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f; // Keep grounded
            }

            // Get movement direction
            Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
            
            // Apply speed
            float speed = _isRunning ? _runSpeed : _walkSpeed;
            _controller.Move(move * speed * Time.deltaTime);

            // Jump
            if (_jumpPressed && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
                _jumpPressed = false;
            }

            // Apply gravity
            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }

        #endregion

        #region Camera

        private void HandleCamera()
        {
            if (_cameraTransform == null) return;

            // Rotate player body (Y axis)
            transform.Rotate(Vector3.up * _lookInput.x * _mouseSensitivity);

            // Rotate camera (X axis)
            _cameraRotationX -= _lookInput.y * _mouseSensitivity;
            _cameraRotationX = Mathf.Clamp(_cameraRotationX, _cameraMinY, _cameraMaxY);
            _cameraTransform.localRotation = Quaternion.Euler(_cameraRotationX, 0f, 0f);
        }

        #endregion

        #region Interaction

        private void HandleInteraction()
        {
            // Raycast for interactables
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, 
                out RaycastHit hit, _interactionDistance, _interactableLayers))
            {
                // TODO: Show interaction prompt
                Debug.DrawRay(_cameraTransform.position, _cameraTransform.forward * _interactionDistance, Color.green);
            }
            else
            {
                Debug.DrawRay(_cameraTransform.position, _cameraTransform.forward * _interactionDistance, Color.red);
            }
        }

        private void ToggleCursorLock()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        #endregion

        #region Input System Callbacks

        public void OnMove(InputValue value)
        {
            _moveInput = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            _lookInput = value.Get<Vector2>();
        }

        public void OnJump(InputValue value)
        {
            _jumpPressed = value.isPressed;
        }

        public void OnSprint(InputValue value)
        {
            _isRunning = value.isPressed;
        }

        public void OnInteract(InputValue value)
        {
            if (value.isPressed)
            {
                TryInteract();
            }
        }

        #endregion

        #region Public Methods

        public void TryInteract()
        {
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, 
                out RaycastHit hit, _interactionDistance, _interactableLayers))
            {
                // Try to interact with object
                var interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(gameObject);
                    Debug.Log($"[Player] Interacted with {hit.collider.name}");
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Interface for interactable objects
    /// </summary>
    public interface IInteractable
    {
        void Interact(GameObject player);
    }
}

