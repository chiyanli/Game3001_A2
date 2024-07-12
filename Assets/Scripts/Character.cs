using UnityEngine;

public class Character : MonoBehaviour
{
    public float MovementSpeed = 0.2f;

    public void Move(Vector2 direction)
    {
        Vector2 newPosition = (Vector2)transform.position + direction;
        transform.position = newPosition;
    }
}
