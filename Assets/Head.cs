using System.Threading;
using UnityEngine;

public class Head : MonoBehaviour
{

    public float speed = 4f;
    public float rotationSpeed = 250f;

    float direction = 0f;

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime, Space.Self);
        transform.Rotate(Vector3.forward * -direction * rotationSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
    }
}
