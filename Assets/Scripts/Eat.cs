using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eat : MonoBehaviour
{

    public AudioClip ac;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "head")
        {

            //播放吃食物音效
            AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);


            //销毁食物，再次生成一个食物
            GameObject.Destroy(gameObject);
            CreatIce.Creater.Createfood();
            UIManager._Instance.AddScore();

            //通知Snack类调用生成身体函数
            Snack._Instantiate.CreatBody();
        }
    }
}
