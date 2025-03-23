using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 direction = Vector2.up;
    private Vector2 newDirection = Vector2.up;
    private Vector2 mouseStartPosition;
    private List<Transform> segments = new List<Transform>();
    public Transform segmentPrefab;
    private float baseHue = 0.33f; // Green hue in HSV
    public int initialSize = 4;
    private float tStep = 0.06f;

    private void Start()
    {
        ResetState();
    }
    private void Update()
    {
        // Handle keyboard input
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
        {
            newDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
        {
            newDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
        {
            newDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
        {
            newDirection = Vector2.right;
        }
        // Handle mouse and touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Ended)
            {
                Vector2 swipeDelta = touch.deltaPosition;
                float x = swipeDelta.x;
                float y = swipeDelta.y;

                // Check if the swipe is more horizontal or vertical
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    // Horizontal swipe
                    Vector2 tempDirection = x > 0 ? Vector2.right : Vector2.left;
                    if ((x > 0 && direction != Vector2.left) || (x < 0 && direction != Vector2.right))
                    {
                        newDirection = tempDirection;
                    }
                }
                else
                {
                    // Vertical swipe
                    Vector2 tempDirection = y > 0 ? Vector2.up : Vector2.down;
                    if ((y > 0 && direction != Vector2.down) || (y < 0 && direction != Vector2.up))
                    {
                        newDirection = tempDirection;
                    }
                }
            }
        }

        // Handle mouse swipe input
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 swipeDelta = (Vector2)Input.mousePosition - mouseStartPosition;
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            // Only register as swipe if movement is large enough
            if (swipeDelta.magnitude > 50f)
            {
                // Check if the swipe is more horizontal or vertical
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    // Horizontal swipe
                    Vector2 tempDirection = x > 0 ? Vector2.right : Vector2.left;
                    if ((x > 0 && direction != Vector2.left) || (x < 0 && direction != Vector2.right))
                    {
                        newDirection = tempDirection;
                    }
                }
                else
                {
                    // Vertical swipe
                    Vector2 tempDirection = y > 0 ? Vector2.up : Vector2.down;
                    if ((y > 0 && direction != Vector2.down) || (y < 0 && direction != Vector2.up))
                    {
                        newDirection = tempDirection;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }
        
        this.transform.position = new Vector2(
            Mathf.Round(this.transform.position.x) + newDirection.x,
            Mathf.Round(this.transform.position.y) + newDirection.y
        );
        direction = newDirection;

        UpdateSegmentColors();
    }

    private void UpdateSegmentColors()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            SpriteRenderer renderer = segments[i].GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                // Calculate brightness based on segment position
                float brightness = 0.5f + ((float)(segments.Count - i) / segments.Count) * 0.5f;
                Color color = Color.HSVToRGB(baseHue, 0.7f, brightness);
                renderer.color = color;
            }
        }
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
        UpdateSegmentColors(); // Update colors when growing
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
        } else if (other.tag == "Obstacle")
        {
            // Game over
            ResetState();
        }
    }
    private void ResetState()
    {
        Time.fixedDeltaTime = tStep;
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }
        segments.Clear();
        segments.Add(this.transform);
        UpdateSegmentColors();

        direction = Vector2.up;
        this.transform.position = Vector2.zero;

        // Add initial segments in a vertical line below the head
        for (int i = 1; i < this.initialSize; i++)
        {
            Transform segment = Instantiate(this.segmentPrefab);
            segment.position = new Vector2(
                this.transform.position.x,
                this.transform.position.y - i
            );
            segments.Add(segment);
        }
        UpdateSegmentColors();
    }
}
