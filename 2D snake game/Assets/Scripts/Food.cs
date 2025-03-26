using UnityEngine;
using System.Collections.Generic;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public Snake snake; // Reference to the snake
    public ParticleSystem foodEffect;

    private void Start()
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        Bounds bounds = this.gridArea.bounds;
        bool validPosition = false;
        Vector3 newPosition = Vector3.zero;

        while (!validPosition)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            newPosition = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);

            // Check if position overlaps with any snake segment
            validPosition = true;
            foreach (Transform segment in snake.segments)
            {
                if (Vector3.Distance(newPosition, segment.position) < 1f)
                {
                    validPosition = false;
                    break;
                }
            }
        }

        this.transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ParticleSystem effect = Instantiate(foodEffect, transform.position, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration);
            RandomizePosition();
        }
    }
}
