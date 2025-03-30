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
            screenPos.x = Mathf.Clamp(screenPos.x, padding, Screen.width - padding);
            screenPos.y = (snakeScreenPos.y-foodScreenPos.y)/(snakeScreenPos.x-foodScreenPos.x)*(screenPos.x-snakeScreenPos.x)+snakeScreenPos.y;
            if (screenPos.y < padding || screenPos.y > Screen.height - padding) {
                screenPos.y = Mathf.Clamp(screenPos.y, padding, Screen.height - padding);
                screenPos.x = (screenPos.y-snakeScreenPos.y)/(foodScreenPos.y-snakeScreenPos.y)*(foodScreenPos.x-snakeScreenPos.x)+snakeScreenPos.x;
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
