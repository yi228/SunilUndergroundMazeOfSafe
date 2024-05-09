using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazePlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 inputVec;
    public Vector2 InputVec { get { return inputVec; } set { inputVec = value; } }
    public bool mazeClear = false;

    private Rigidbody2D rigid;
    private BoxCollider2D boxColl;
    public MazePlayerUI mazeUI;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxColl = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        Move();
    }
    private void Move()
    {
        rigid.MovePosition(rigid.position + inputVec * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            mazeClear = true;
            mazeUI.solvePanel.SetActive(true);
            Invoke("Clear", 2f);
        }
    }
    private void Clear()
    {
        Managers.Game.PlayerUIOn();
        Managers.Game.PuzzleSolve();
        GameObject go = GameObject.FindGameObjectWithTag("Puzzle");
        Destroy(go);
    }
}
