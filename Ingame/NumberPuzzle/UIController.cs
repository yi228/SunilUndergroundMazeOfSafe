using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
	[SerializeField] private GameObject resultPanel;

	public void OnResultPanel()
	{
		resultPanel.SetActive(true);
        Invoke("Clear", 2f);
	}
    private void Clear()
    {
        Managers.Game.PuzzleSolve();
        GameObject go = GameObject.FindGameObjectWithTag("Puzzle");
        Destroy(go);
    }
    public void OnClickRestart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}

