using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform snake;

    void Update()
    {
        if (snake != null)
        {
            transform.position = new Vector3(snake.position.x, snake.position.y, transform.position.z);
        }
    }

    public void setTarget(Transform target)
    {
        snake = target;
    }
}


