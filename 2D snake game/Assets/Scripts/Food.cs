using UnityEngine;
using System.Collections.Generic;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public Snake snake; // Reference to the snake
    public ParticleSystem foodEffect;
    private bool isActive = false;

    private void Start()
    {
        // Don't spawn food immediately
        gameObject.SetActive(false);
    }

    public void OnGameStart()
    {
        isActive = true;
        gameObject.SetActive(true);
        RandomizePosition();
    }

    public void OnGameOver()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    private void RandomizePosition()
    {
        if (!isActive) return;

        Bounds bounds = this.gridArea.bounds;
        bool validPosition = false;
        Vector3 newPosition = Vector3.zero;

        while (!validPosition)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            newPosition = new Vector3(Mathf.Round(x), Mathf.Round(y), -1.0f);

            // Check if position overlaps with any snake segment
            validPosition = true;
            foreach (Transform segment in snake.segments)
            {
                Vector3 segmentPosAtFoodZ = segment.position;
                segmentPosAtFoodZ.z = -1.0f; // Match food's z coordinate
                if (Vector3.Distance(newPosition, segmentPosAtFoodZ) < 1f)
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
        if (other.tag == "Player" && isActive)
        {
            ParticleSystem effect = Instantiate(foodEffect, transform.position, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration);
            RandomizePosition();
        }
    }
}
