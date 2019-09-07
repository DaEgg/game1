using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStatus
{
    Menu,
    play,
    pause,
    Continue,
    gameOver
}
public enum SelectOption
{
    blue,
    yellow,
}

public class UIManager : MonoBehaviour
{
    static UIManager UIMgr;
    public static UIManager _Instance
    {
        get { return UIMgr; }
    }

    public SelectOption selectOption;
    public GameStatus gamestatus;
    public GameStatus changUI;
    public GameObject[] UI_GameObjects;
    Text score;
    Text lenght;
    Text lastT;
    Text bestT;
    int historyMaxScore;
    int historyMaxlenght;
    int scoreNum;
    int lenghtNum;
    public bool isBlue;
    public bool isFreedom;

    private void Awake()
    {
        UIMgr = this;
    }
    void Start()
    {
        gamestatus = GameStatus.play;
        changUI = GameStatus.Menu;
        UI_GameObjects = GameObject.FindGameObjectsWithTag("UI");
        score = GameObject.FindGameObjectWithTag("score").GetComponent<Text>();
        lenght = GameObject.FindGameObjectWithTag("lenght").GetComponent<Text>();
        lastT = GameObject.FindGameObjectWithTag("last").GetComponent<Text>();
        bestT = GameObject.FindGameObjectWithTag("best").GetComponent<Text>();    
        scoreNum = 0;
        lenghtNum = 0;
        isBlue = true;
        isFreedom = false;
    }
    void Update()
    {
        if (gamestatus != changUI) ChangeUI(changUI);
    }
    public void ChangeUI(GameStatus changeTo)
    {
        if (changeTo == GameStatus.Menu)
        {
            foreach (var item in UI_GameObjects)
            {
                if (item.gameObject.name == "UI_Manual") item.gameObject.SetActive(true);
                else item.gameObject.SetActive(false);
            }
            Time.timeScale = 0;
            lastT.text = "上次：  长度" + PlayerPrefs.GetInt("lastL", 0) + ",   分数"+ PlayerPrefs.GetInt("lastS", 0);
            bestT.text = "最好：  长度" + PlayerPrefs.GetInt("bestL", 0) + ",   分数" + PlayerPrefs.GetInt("bestS", 0);
          
        }
        if (changeTo == GameStatus.play)
        {
            Time.timeScale = 1;
            
            foreach (var item in UI_GameObjects)
            {
                if (item.gameObject.name == "UI_Play") item.gameObject.SetActive(true);
                else item.gameObject.SetActive(false);
            }
            if(isFreedom) GameObject.Find("model").GetComponent<Text>().text = "自由模式";
            else GameObject.Find("model").GetComponent<Text>().text = "边界模式";

            if (CreatIce.Creater.ice == null) { CreatIce.Creater.Createfood(); }
            ResetScore();
            Snack._Instantiate.gameObject.SetActive(true);
            Snack._Instantiate.SnackReset();
            CreatIce.Creater.Createfood();
            Snack._Instantiate.SnackContiue();

        }
        if (changeTo == GameStatus.gameOver)
        {
            PlayerPrefs.SetInt("lastL", lenghtNum);
            PlayerPrefs.SetInt("lastS", scoreNum);
            if (PlayerPrefs.GetInt("bestS", 0) < scoreNum)
            {
                PlayerPrefs.SetInt("bestL", lenghtNum);
                PlayerPrefs.SetInt("bestS", scoreNum);
            }

            foreach (var item in UI_GameObjects)
            {
                if (item.gameObject.name == "UI_End") item.gameObject.SetActive(true);
            }

        }
        if (changeTo == GameStatus.pause)
        {
            Snack._Instantiate.SnackStop();
            gamestatus = changeTo;
        }
        if (changeTo == GameStatus.Continue)
        {
            Snack._Instantiate.SnackContiue();
            gamestatus = changeTo;
        }
        gamestatus = changUI;
    }
    public void ResetScore()
    {
        score.text = "得分:\n0";
        lenght.text = "长度:\n0";
    }
    public void AddScore()
    {
        //增加得分，长度  
        string[] s = score.text.Split(':'); //获取的UI文本 以：分割
        string[] l = lenght.text.Split(':');

        scoreNum = int.Parse(s[1]);
        lenghtNum = int.Parse(l[1]);
        scoreNum += 10;
        lenghtNum += 1;
        s[1] = scoreNum.ToString();
        l[1] = lenghtNum.ToString();

        score.text = s[0] + ":\n" + s[1];
        lenght.text = l[0] + ":\n" + l[1];
    }
}
