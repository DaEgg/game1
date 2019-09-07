using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChange : MonoBehaviour
{
    public void Home()
    {
        UIManager._Instance.changUI = GameStatus.Menu;
        Snack._Instantiate.SnackReset();
       
    }
    public void StarGame()
    {
        UIManager._Instance.changUI = GameStatus.play;
        Snack._Instantiate.SnackContiue();       
    }
    public void Pasue()
    {
        if (UIManager._Instance.gamestatus == GameStatus.pause) { UIManager._Instance.changUI = GameStatus.Continue; }
        else UIManager._Instance.changUI = GameStatus.pause;

    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Again()
    {
        UIManager._Instance.changUI = GameStatus.play;
        Snack._Instantiate.gameObject.SetActive(true);
        Snack._Instantiate.SnackContiue();
        CreatIce.Creater.Createfood();
        UIManager._Instance.ResetScore();
    }
    public void OnColorChange()
    {
        UIManager._Instance.isBlue = GameObject.FindGameObjectWithTag("blue").GetComponent<Toggle>().isOn;
    }
    public void OnModelChange(bool isFreedom)
    {
        UIManager._Instance.isFreedom =  GameObject.FindGameObjectWithTag("freedom").GetComponent<Toggle>().isOn;
    }

}
