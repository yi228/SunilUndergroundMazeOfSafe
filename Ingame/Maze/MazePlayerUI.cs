using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePlayerUI : MonoBehaviour
{
    private MazePlayerController player;
    public GameObject solvePanel;

    public void MoveUp()
    {
        FindPlayer();
        player.InputVec = Vector2.up;
    }
    public void MoveDown()
    {
        FindPlayer();
        player.InputVec = Vector2.down;
    }
    public void MoveLeft()
    {
        FindPlayer();
        player.InputVec = Vector2.left;
    }
    public void MoveRight()
    {
        FindPlayer();
        player.InputVec = Vector2.right;
    }
    public void ReleaseMove()
    {
        FindPlayer();
        player.InputVec = Vector2.zero;
    }
    private void FindPlayer()
    {
        if (player == null)
        {
            player = FindObjectOfType<MazePlayerController>();
            player.GetComponent<MazePlayerController>().mazeUI = this;
        }
    }
}
