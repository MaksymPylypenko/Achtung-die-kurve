using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    public SnakeBrain controller;

    // unique for each snake
    public int snakeID;
    public Color color;

    // objects
    public Transform head;
    public SnakeTail tailPrefab;

    // position in game
    public float headAngle;
    public Vector3 headPosition;

    // frequency of points
    public Vector3 prevPosition;
    public float pointSpacing = 0.5f;

    // properties
    public float speed = 0.1f;
    public float angularSpeed = 4f;       
    public float width = 0.3f;

    // gaps
    public float breakTime = 0.30f;
    List<SnakeTail> snakeTails;

    // status
    bool tailActive = true;
    bool gameOver = false;
    bool isReady = false;
    bool isCollision = false;

    // 
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    CircleCollider2D col;
    Mesh mesh;

    void setCircle()
    {
        float w = width / 2.0f + 0.01f;
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-w, -w, 0),
            new Vector3(w, -w, 0),
            new Vector3(-w, w, 0),
            new Vector3(w, w, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;
        meshFilter.mesh = mesh;
    }

    void Awake()
    {
        snakeTails = new List<SnakeTail>();      

        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshFilter = gameObject.GetComponent<MeshFilter>();
        col = gameObject.GetComponent<CircleCollider2D>();
        col.radius = width / 2.0f;
        //pointSpacing = width / 3.2f; // ??

        breakTime = width*1.5f;
        setCircle();
    }


    public void StartSnake()
    {
        meshRenderer.material.color = new Vector4(color[0], color[1], color[2], 1.0f);
        AddTail();    
        StartCoroutine(TailDrawer());
        isReady = true;
        prevPosition = new Vector3(-99, -99, 0);
    }

    void AddTail()
    {
        SnakeTail tail = Instantiate(tailPrefab, Vector3.zero, Quaternion.identity) as SnakeTail;
        tail.SetColor(color);
        tail.SetWidth(width);
        tail.SetOffset((int)(width * 15));
        snakeTails.Add(tail);
    }


    IEnumerator TailDrawer()
    {
        tailActive = false;
        yield return new WaitForSeconds(2.0f);
        tailActive = true;

        while (!gameOver)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 7.0f));
            tailActive = false;
                      
            yield return new WaitForSeconds(breakTime);

            if (isCollision)
            {
                StartCoroutine(GameOver());
                Debug.Log("Edge case");
            }

            AddTail();
            snakeTails[snakeTails.Count - 2].AdjustCollider(); // remove offset in the previous tail

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
            if (Vector3.Distance(prevPosition, headPosition) >= pointSpacing)
            {
                GetCurrentSnakeTail().UpdateTail(headPosition);
                prevPosition = headPosition;
            }

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


    void OnTriggerExit2D(Collider2D other)
    {
        isCollision = false;
        Debug.Log("CollisionExit");        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isCollision = true;
        if (tailActive)
        {
            Debug.Log("Collision at " + other.transform.position);
            StartCoroutine(GameOver());
        }
        
    }


    IEnumerator GameOver()
    {
        gameOver = true;
        yield return new WaitForSeconds(1);
        gameOver = false;

        //for (int i = 0; i < snakeTails.Count; i++)
        //{
        //    GameObject.Destroy(snakeTails[i].gameObject);
        //}

        //snakeTails.Clear();

        //GameObject.Destroy(head.gameObject);
        //SceneManager.LoadScene(0);
    }


    float GetSteerDirection() // This should depend on the current player controlling the snake..
    {
        return controller.Move();
    }

    SnakeTail GetCurrentSnakeTail()
    {
        return snakeTails[snakeTails.Count - 1];
    }
}
