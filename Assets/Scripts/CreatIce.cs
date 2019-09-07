using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreatIce : MonoBehaviour
{
    static CreatIce creater;
    public static CreatIce Creater
    {
        get { return creater; }
    }

    public Sprite[] Images;
    public GameObject foodPerfabs;
    public GameObject ice;

    //食物产生的范围 X: -11*offset -> 22*offset  Y: -11 *offset -> 11*offset
    int xLimit = 22;
    int yLimit = 11;
    int xoffset = 11;
     void Awake()
    {
        creater = this;      
    }
    //void Start()
    //{  
    //    Createfood();
    //}
    public void Createfood()
    {
        ice = GameObject.Instantiate(foodPerfabs);
        ice.transform.SetParent(GameObject.Find("foodCreater").transform, false);
        int index = Random.Range(0, Images.Length);
        ice.GetComponent<Image>().sprite = Images[index];
        int x = Random.Range(-xLimit+xoffset ,xLimit);
        int y = Random.Range(-yLimit,yLimit);
        //判断生成的冰激凌不可以与蛇头或者蛇身有相同的位置 
        if (Snack._Instantiate.bodys.Count > 0)
        {
            while (true)
            {
                if (Snack._Instantiate.transform.localPosition.x == x && Snack._Instantiate.transform.localPosition.y == y)
                {
                    x = Random.Range(-xLimit + xoffset, xLimit);
                    y = Random.Range(-yLimit, yLimit);
                }
                else break;
            }
            for (int  i = 0; i <Snack._Instantiate.bodys.Count;i++)
            {
                if (Snack._Instantiate.bodys[i].transform.localPosition.x == x && Snack._Instantiate.bodys[i].transform.localPosition.y == y)
                {
                    x = Random.Range(-xLimit + xoffset, xLimit);
                    y = Random.Range(-yLimit, yLimit);
                    i = 0;
                    if (i == Snack._Instantiate.bodys.Count - 1) i = 0;
                }        
            }
        }
        else
        {
            while (true)
            {
                if (Snack._Instantiate.transform.localPosition.x == x && Snack._Instantiate.transform.localPosition.y == y)
                {
                    x = Random.Range(-xLimit + xoffset, xLimit);
                    y = Random.Range(-yLimit, yLimit);
                }
                else break;
            }

        }
        ice.transform.localPosition = new Vector3(x*17, y*17, 0);
    }

}
