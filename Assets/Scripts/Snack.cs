using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snack : MonoBehaviour
{
    float offset = 17.0f; //每一次移动的距离
    public float speed = 0.21f;
    public GameObject body;
    public Sprite[] snackImages; // 0 1是白色蓝色，2 3是灰色黄色，45是蓝头，黄头
    Image IsnackHead;
    AudioClip dieAc;
    public List<GameObject> bodys;
    bool enableChangeDir = true;
    Vector3 lastBodyPosHistory;
    Vector3 headHistoryPos;
    static Snack _instantiate;

    public static Snack _Instantiate
    {
        get { return _instantiate; }
    }

    enum MoveStatus
    {
        up,
        down,
        left,
        right
    }
    MoveStatus moveStatus = new MoveStatus();

    private void Awake()
    {
        _instantiate = this;
    }
    void Start()
    {
        dieAc = Resources.Load("Audio/Die") as AudioClip;
        bodys = new List<GameObject>();
        moveStatus = MoveStatus.up;

    }
    void Update()
    {
        if (UIManager._Instance.gamestatus != GameStatus.pause)
        {
            ChangeDirct();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CancelInvoke();
                InvokeRepeating("SnackMove", 0, speed - 0.2f);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                CancelInvoke();
                InvokeRepeating("SnackMove", 0, speed);
            }
        }
    }
    public void SnackMove()
    {
        //在移动前保存蛇头和最后一节的位置，如果只有头，则保存头部坐标
        headHistoryPos = transform.localPosition;
        if (bodys.Count == 0) lastBodyPosHistory = transform.localPosition;
        else lastBodyPosHistory = bodys[bodys.Count - 1].transform.localPosition;

        //判断移动方向，向该方向移动
        if (moveStatus == MoveStatus.up)
        {
            transform.localPosition += Vector3.up * offset;
        }
        else if (moveStatus == MoveStatus.down)
        {
            transform.localPosition += Vector3.down * offset;
        }
        else if (moveStatus == MoveStatus.left)
        {
            transform.localPosition += Vector3.left * offset;
        }
        else if (moveStatus == MoveStatus.right)
        {
            transform.localPosition += Vector3.right * offset;
        }

        if (bodys.Count != 0) UpdateBody();
        enableChangeDir = true;
    }
    public void UpdateBody()
    {
        Vector3 temp_fist = transform.position;
        Vector3 temp_second;
        for (int i = 0; i < bodys.Count; i++)
        {
            if (i == 0)
            {
                temp_fist = bodys[i].transform.localPosition;
                bodys[i].transform.localPosition = headHistoryPos;
            }
            else
            {
                temp_second = bodys[i].transform.localPosition;
                bodys[i].transform.localPosition = temp_fist;
                temp_fist = temp_second;

            }

        }

    }
    public void ChangeDirct()
    {   //按键改变移动方向     
            if (Input.GetKey(KeyCode.W) && moveStatus != MoveStatus.down && enableChangeDir)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                moveStatus = MoveStatus.up;
                enableChangeDir = false;
            }
            else if (Input.GetKey(KeyCode.S) && moveStatus != MoveStatus.up && enableChangeDir)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
                moveStatus = MoveStatus.down;
                enableChangeDir = false;
            }
            else if (Input.GetKey(KeyCode.A) && moveStatus != MoveStatus.right && enableChangeDir)
            {
                transform.rotation = Quaternion.Euler(0, 0, -90);
                moveStatus = MoveStatus.left;
                enableChangeDir = false;
            }
            else if (Input.GetKey(KeyCode.D) && moveStatus != MoveStatus.left && enableChangeDir)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
                moveStatus = MoveStatus.right;
                enableChangeDir = false;
            }              
    }
    public void CreatBody()
    {

        GameObject body_Temp = GameObject.Instantiate(body);
        if (UIManager._Instance.isBlue)
        {
            if ((bodys.Count + 1) % 2 == 0) body_Temp.GetComponent<Image>().sprite = snackImages[1];
        }
        else
        {
            if ((bodys.Count + 1) % 2 == 0) body_Temp.GetComponent<Image>().sprite = snackImages[3];
            else body_Temp.GetComponent<Image>().sprite = snackImages[2];
        }
        bodys.Add(body_Temp);
        body_Temp.transform.SetParent(GameObject.Find("UI_Play").transform, false);
        body_Temp.transform.localPosition = lastBodyPosHistory;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!UIManager._Instance.isFreedom)
        {
            if (collision.gameObject.tag == "body" || collision.gameObject.tag == "wall")
            {
                Die();
            }
        }
        else
        {
            if (collision.gameObject.tag == "body")
            {
                Die();
            }
            else
            {
              //  inTransport = true;
                if (collision.gameObject.name == "top") transform.localPosition = new Vector3(transform.localPosition.x, -204, 0);
                else if (collision.gameObject.name == "down") transform.localPosition = new Vector3(transform.localPosition.x, 204, 0);
                else if (collision.gameObject.name == "right") transform.localPosition = new Vector3(-204, transform.localPosition.y, 0);
                else if (collision.gameObject.name == "left") transform.localPosition = new Vector3(391, transform.localPosition.y, 0);
            }
        }
    }
    public void Die()
    {
        gameObject.SetActive(false);
        SnackReset();


        AudioSource.PlayClipAtPoint(dieAc, Camera.main.transform.position);
        GameObject.Find("bg_music").GetComponent<AudioSource>().Stop();
        UIManager._Instance.changUI = GameStatus.gameOver;
    }
    public void SnackStop()
    {
        CancelInvoke();
    }
    public void SnackContiue()
    {
        InvokeRepeating("SnackMove", 0.5f, speed);
    }
    /// <summary>
    /// RestSnack
    /// </summary>
    public void SnackReset()
    {
        if (!UIManager._Instance.isBlue) gameObject.GetComponent<Image>().sprite = snackImages[5];
        else gameObject.GetComponent<Image>().sprite = snackImages[4];
        //删除蛇身，清空list的引用       
        foreach (var item in bodys)
        {
            GameObject.Destroy(item);
        }
        bodys.Clear();
        transform.localPosition = new Vector3(0, 0, 0);
        moveStatus = MoveStatus.up;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        CancelInvoke();
        GameObject.Destroy(CreatIce.Creater.ice);

    }

}
