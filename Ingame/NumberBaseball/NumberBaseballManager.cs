using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NumberBaseballManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private TextMeshProUGUI strikeText;
    [SerializeField] private TextMeshProUGUI ballText;
    [SerializeField] private TextMeshProUGUI outText;
    [SerializeField] private GameObject solvePanel;

    private int[] answer;
    private int[] input;

    private int curIndex = 0;

    void Start()
    {
        answer = new int[4];
        input = new int[4];
        SetAnswer();
        string _ans = "";
        for (int i = 0; i < 4; i++)
            _ans += answer[i];
        Debug.Log(_ans);
    }
    void Update()
    {
        ShowInput();
    }
    private void ShowInput()
    {
        string _input = "";
        for (int i = 0; i < 4; i++)
            _input += input[i];
        inputText.text = _input;
    }
    private void SetAnswer()
    {
        for(int i = 0; i < 4; i++)
        {
            answer[i] = Random.Range(1, 10);
            if(i != 0)
                for(int j = 0; j < i; j++)
                    if(answer[j] == answer[i])
                    {
                        i--;
                        break;
                    }
        }
    }
    public void Submit()
    {
        int _strikeNum = StrikeCheck();
        int _ballNum = BallCheck();
        int _outNum = 4 - _strikeNum - _ballNum;

        strikeText.text = "<color=#B00000>S:</color> " + _strikeNum.ToString();
        ballText.text = "<color=#B00000>B:</color> " + _ballNum.ToString();
        outText.text = "<color=#B00000>O:</color> " + _outNum.ToString();

        if (_strikeNum == 4)
        {
            solvePanel.SetActive(true);
            Invoke("Clear", 2f);
        }
    }
    private void Clear()
    {
        Managers.Game.PuzzleSolve();
        GameObject go = GameObject.FindGameObjectWithTag("Puzzle");
        Destroy(go);
    }
    private int StrikeCheck()
    {
        int _ret = 0;

        for(int i = 0; i < 4; i++)
            if (input[i] == answer[i])
                _ret++;

        return _ret;
    }
    private int BallCheck()
    {
        int _ret = 0;

        for (int i = 0; i < 4; i++)
            for(int j = 0; j < 4; j++)
                if (i != j && input[i] == answer[j])
                    _ret++;

        return _ret;
    }
    public void SetInput(int _num)
    {
        if (curIndex >= 4)
            return;
        input[curIndex] = _num;
        curIndex++;
    }
    public void ResetInput()
    {
        curIndex = 0;
        input = new int[4];
    }
}
