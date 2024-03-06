using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();
            return _instance;
        }
    }

    [SerializeField]
    private Sprite[] spritesOX;
    public Sprite spriteO { get { return spritesOX[0]; } }
    public Sprite spriteX { get { return spritesOX[1]; } }
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI endText;
    public TextMeshProUGUI versionText;

    private bool _isEnd;
    public bool isEnd { get { return _isEnd;  } }

    public int turn;
    private int[] table;
    private Vector3[] winPattern =
    {
        new Vector3(0,1,2),
        new Vector3(3,4,5),
        new Vector3(6,7,8),
        new Vector3(0,3,6),
        new Vector3(1,4,7),
        new Vector3(2,5,8),
        new Vector3(0,4,8),
        new Vector3(2,4,6)
    };

    public Vector3 row1;
    public Vector3 row2;
    public Vector3 row3;

    private void Awake()
    {
        _instance = this;

        turn = 0;
        _isEnd = false;

        table = new int[9];
        for (int i = 0; i < table.Length; i++)
        {
            table[i] = -1;
        }

        versionText.text = "version " + Application.version;

        StartCoroutine(OnEscape());
    }

    private void Update()
    {
        UpdateTable();
        UpdateTextTurn();
        UpdateEndGame();
    }

    IEnumerator OnEscape()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                print("OK! Bye!!!");
                Application.Quit();
            }
            yield return null;
        }
    }

    private void UpdateEndGame()
    {
        for (int i = 0; i < winPattern.Length; i++)
        {
            int[] index = new int[3];
            index[0] = (int)winPattern[i].x;
            index[1] = (int)winPattern[i].y;
            index[2] = (int)winPattern[i].z;

            bool isEmpty = false;
            foreach (int n in index)
            {
                if (table[n] == -1)
                {
                    isEmpty = true;
                    break;
                }

            }

            if (!isEmpty)
            {
                if (table[index[0]] == table[index[1]] && table[index[1]] == table[index[2]])
                {
                    _isEnd = true;

                    string winner = ConvertMarkToSign(table[index[0]]);
                    endText.text = winner + " Win!!";
                    endText.transform.parent.gameObject.SetActive(true);
                }
            }
        }

        // Draw
        if(!_isEnd && turn >= 9)
        {
            _isEnd = true;

            endText.text = "Draw!!";
            endText.transform.parent.gameObject.SetActive(true);
        }
    }

    public void ButtonNew()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int MarkOX(int cell)
    {
        int mark = ConvertTurnToSignInt(turn);
        table[cell] = mark;

        turn++;

        return mark;
    }

    private void UpdateTextTurn()
    {
        if (_isEnd)
            turnText.text = "Turn : --";
        else
            turnText.text = "Turn : " + ConvertTurnToSignString(turn);
    }

    private void UpdateTable()
    {
        row1.x = table[0];
        row1.y = table[1];
        row1.z = table[2];

        row2.x = table[3];
        row2.y = table[4];
        row2.z = table[5];

        row3.x = table[6];
        row3.y = table[7];
        row3.z = table[8];
    }

    private int ConvertTurnToSignInt(int turn)
    {
        if (turn % 2 == 0)
            return (int)Sign.O;
        else
            return (int)Sign.X;
    }
    private string ConvertTurnToSignString(int turn)
    {
        if (turn % 2 == 0)
            return Sign.O.ToString();
        else
            return Sign.X.ToString();
    }
    private string ConvertMarkToSign(int mark)
    {
        if (mark == (int)Sign.O)
            return Sign.O.ToString();
        else if (mark == (int)Sign.X)
            return Sign.X.ToString();
        else
            return "Error";
    }
}

public enum Sign { O, X };
