using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Snake snake;

    void FixedUpdate()
    {
        if (snake != null)
        {
            transform.position = new Vector3(snake.head.position.x, snake.head.position.y, transform.position.z);
            transform.localRotation = Quaternion.Euler(0, 0, snake.headAngle - 90f);
        }
    }

    public void setTarget(Snake target)
    {
        snake = target;
    }
}


