using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    // unique for each snake
    public int snakeID;
    public Color color;

    // objects
    public Transform head;
    public SnakeTail tailPrefab;

    // position in game
    public float headAngle;
    public Vector3 headPosition;

    // properties
    public float speed = 3f;
    public float angularSpeed = 4f;       
    public float width = 0.3f;

    // gaps
    public float breakTime = 0.40f;
    List<SnakeTail> snakeTails;

    // status
    bool tailActive = true;
    bool gameOver = false;
    bool isReady = false;

    void Awake()
    {
        snakeTails = new List<SnakeTail>();
    }

     
    public void StartSnake()
    {
        AddTail();
        StartCoroutine(TailDrawer());
        isReady = true;
    }

    void AddTail()
    {
        SnakeTail tail = Instantiate(tailPrefab, Vector3.zero, Quaternion.identity) as SnakeTail;
        tail.SetColor(color);
        tail.SetWidth(width);

        snakeTails.Add(tail);
    }


    IEnumerator TailDrawer()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 7.0f));
            tailActive = false;
        
            yield return new WaitForSeconds(breakTime);
            AddTail();
            tailActive = true;
        }
    }



    void FixedUpdate()
    {
        if (gameOver || !isReady)
        {
            return;
        }
             
        headAngle += angularSpeed * GetSteerDirection();

        //transform.Translate(Vector2.up * speed * Time.fixedDeltaTime, Space.Self);
        //transform.Rotate(Vector3.forward * headAngle * Time.fixedDeltaTime);

        headPosition.x += speed * Mathf.Cos(headAngle * Mathf.PI / 180f) * Time.fixedDeltaTime;
        headPosition.y += speed * Mathf.Sin(headAngle * Mathf.PI / 180f) * Time.fixedDeltaTime;

        if (tailActive)
        {
            GetCurrentSnakeTail().UpdateTail(headPosition);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (gameOver || !isReady)
        {
            return;
        }

        head.position = headPosition;
        head.localRotation = Quaternion.Euler(0, 0, headAngle); 
    }



    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collision at "+col);
        GameOver();
        SceneManager.LoadScene(0);
    }


    void GameOver()
    {
        gameOver = true;

        for (int i = 0; i < snakeTails.Count; i++)
        {
            GameObject.Destroy(snakeTails[i].gameObject);
        }

        snakeTails.Clear();

        GameObject.Destroy(head.gameObject);
    }


    float GetSteerDirection()
    {
        //if (Input.touchCount > 0)
        //{
        //    var touch = Input.GetTouch(0);
        //    if (touch.position.x < Screen.width / 2)
        //    {
        //        Debug.Log("Left click");
        //        return 1.0f;
        //    }
        //    else if (touch.position.x > Screen.width / 2)
        //    {
        //        Debug.Log("Right click");
        //        return -1.0f;
        //    }
        //}
        //return 0;

        return -Input.GetAxisRaw("Horizontal");

        //if (snakeID == 0)
        //{
        //    if (Input.GetKey(KeyCode.A))
        //    {
        //        return 1f;
        //    }
        //    else if (Input.GetKey(KeyCode.D))
        //    {
        //        return -1f;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        //if (snakeID == 1)
        //{
        //    if (Input.GetKey(KeyCode.LeftArrow))
        //    {
        //        return 1f;
        //    }
        //    else if (Input.GetKey(KeyCode.RightArrow))
        //    {
        //        return -1f;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        //return 0;
    }

    SnakeTail GetCurrentSnakeTail()
    {
        return snakeTails[snakeTails.Count - 1];
    }
}
