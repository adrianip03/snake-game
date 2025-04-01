using UnityEngine;
using UnityEngine.UI;

public class ArrowMovement : MonoBehaviour
{
    public Food foodObject;
    private Transform food;
    public Camera mainCamera;
    public Transform snake;
    private float padding = 200f; // Distance from screen edge
    private SpriteRenderer spriteRenderer;
    
    private bool isOffScreen(Vector3 position){
        return position.x <= 0 || position.x >= Screen.width || position.y <= 0 || position.y >= Screen.height;
    }
    void Start()
    {
        // Set initial transparency to 100%
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
        food = foodObject.transform;
    }

    void FixedUpdate()
    {
        if (!foodObject.gameObject.activeSelf) {
            Color color = spriteRenderer.color;
            color.a = 0f;
            spriteRenderer.color = color;
            return;
        }

        // Get food position in screen space
        Vector3 foodScreenPos = mainCamera.WorldToScreenPoint(food.position);
        bool foodIsOffScreen = isOffScreen(foodScreenPos);

        if (foodIsOffScreen)
        {
            Color color = spriteRenderer.color;
            color.a = 0.75f;
            spriteRenderer.color = color;

            // Calculate direction to food
            Vector3 direction = food.position - snake.position;
            // Rotate arrow towards food
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            this.transform.eulerAngles = new Vector3(0, 0, angle-90);


            // Set arrow position at screen edge
            Vector3 snakeScreenPos = mainCamera.WorldToScreenPoint(snake.position);
            Vector3 screenPos = foodScreenPos;
            
            // Calculate direction vector
            Vector2 directionVec = foodScreenPos - snakeScreenPos;
            float dx = Mathf.Abs(directionVec.x);
            float dy = Mathf.Abs(directionVec.y);
            
            // Handle different cases based on direction to prevent division by zero
            if (dx < 0.01f) {
                // Vertical case
                screenPos.x = snakeScreenPos.x;
                screenPos.y = directionVec.y > 0 ? Screen.height - padding : padding;
            }
            else if (dy < 0.01f) {
                // Horizontal case
                screenPos.y = snakeScreenPos.y;
                screenPos.x = directionVec.x > 0 ? Screen.width - padding : padding;
            }
            else {
                // Diagonal case
                float slope = directionVec.y / directionVec.x;
                if (directionVec.x > 0) {
                    screenPos.x = Screen.width - padding;
                    screenPos.y = snakeScreenPos.y + slope * (screenPos.x - snakeScreenPos.x);
                } else {
                    screenPos.x = padding;
                    screenPos.y = snakeScreenPos.y + slope * (screenPos.x - snakeScreenPos.x);
                }
                
                // Clamp y position if it goes off screen
                if (screenPos.y < padding || screenPos.y > Screen.height - padding) {
                    screenPos.y = screenPos.y < padding ? padding : Screen.height - padding;
                    screenPos.x = snakeScreenPos.x + (screenPos.y - snakeScreenPos.y) / slope;
                }
            }
            
            transform.position = mainCamera.ScreenToWorldPoint(screenPos);
        }
        else
        {
            Color color = spriteRenderer.color;
            color.a = 0f;
            spriteRenderer.color = color;
        }
    }
}
