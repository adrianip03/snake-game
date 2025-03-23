using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 offset = new Vector2(0f, 0f);
    
    [Header("Smooth Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float minY = -10f;
    [SerializeField] private float maxY = 10f;
    
    private Camera mainCamera;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        
        if (target == null)
        {
            Debug.LogWarning("Camera target not assigned! Please assign the snake object in the inspector.");
            return;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate target position with offset
        Vector3 targetPosition = new Vector3(
            target.position.x + offset.x,
            Mathf.Clamp(target.position.y + offset.y, minY, maxY),
            transform.position.z
        );

        // Smoothly move the camera
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            1f / smoothSpeed
        );
    }
} 