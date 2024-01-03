using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Game_DataManager : MonoBehaviour
{
    // public string GameName; //게임 이름
    // public int GameStage; //게임 단계 4단계 

    public string Game_Succsed; // 성공 실패 여부 
    public float level_playTime_step;

    public static Game_DataManager instance { get; private set; }
    public void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;

    }

    //게임 레벨에 따른 진행 시간.
    public void Game1_Time_SetData()
    {
        if (GameManager.instance.CoreGame_Stage_i.Equals(1)) { level_playTime_step = 20; }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(2)) { level_playTime_step = 30; }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(3)) { level_playTime_step = 45; }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(4)) { level_playTime_step = 60; }
    }
    //TowerMaker
    public void Game2_Time_SetData()
    {
        if (GameManager.instance.CoreGame_Stage_i.Equals(1)) { level_playTime_step = 30; }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(2)) { level_playTime_step = 50; }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(3)) { level_playTime_step = 70; }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(4)) { level_playTime_step = 90; }
    }
    //WireWalking 
    public void Game3_Time_SetData()
    {
        if (GameManager.instance.CoreGame_Stage_i.Equals(1)) { level_playTime_step = 20; }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(2)) { level_playTime_step = 30; }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(3)) { level_playTime_step = 45; }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(4)) { level_playTime_step = 60; }
    }
    //게임 데이터 저장
    public void Save_GameClearData()
    {
        //Tower Maker
        if (GameManager.instance.CoreGame_Name_str.Equals("TowerMaker"))
        {
            if (GameManager.instance.CoreGame_Stage_i == 1)
            {
                if (GameManager.instance.Tower_Clear_level < 1)
                    PlayerPrefs.SetInt("Tower_Clear", 1);
            }
            else if (GameManager.instance.CoreGame_Stage_i == 2)
            {
                if (GameManager.instance.Tower_Clear_level < 2)
                    PlayerPrefs.SetInt("Tower_Clear", 2);
            }
            else if (GameManager.instance.CoreGame_Stage_i == 3)
            {
                if (GameManager.instance.Tower_Clear_level < 3)
                    PlayerPrefs.SetInt("Tower_Clear", 3);
            }
            else if (GameManager.instance.CoreGame_Stage_i == 4)
            {
                if (GameManager.instance.Tower_Clear_level < 4)
                    PlayerPrefs.SetInt("Tower_Clear", 4);
            }
        }
        //Wire Walk
        else if (GameManager.instance.CoreGame_Name_str.Equals("WireWalking"))
        {
            if (GameManager.instance.CoreGame_Stage_i == 1)
            {
                if (GameManager.instance.WireWalk_Clear_level < 1)
                    PlayerPrefs.SetInt("WireWalking_Clear", 1);
            }
            else if (GameManager.instance.CoreGame_Stage_i == 2)
            {
                if (GameManager.instance.WireWalk_Clear_level < 2)
                    PlayerPrefs.SetInt("WireWalking_Clear", 2);
            }
            else if (GameManager.instance.CoreGame_Stage_i == 3)
            {
                if (GameManager.instance.WireWalk_Clear_level < 3)
                    PlayerPrefs.SetInt("WireWalking_Clear", 3);
            }
            else if (GameManager.instance.CoreGame_Stage_i == 4)
            {
                if (GameManager.instance.WireWalk_Clear_level < 4)
                    PlayerPrefs.SetInt("WireWalking_Clear", 4);
            }
        }
        //Delivery Man
        else if (GameManager.instance.CoreGame_Name_str.Equals("DeliveryMan"))
        {
            if (GameManager.instance.CoreGame_Stage_i == 1)
            {
                if (GameManager.instance.DeliveryMan_Clear_level < 1)
                    PlayerPrefs.SetInt("DeliveryMan_Clear", 1);
            }
            else if (GameManager.instance.CoreGame_Stage_i == 2)
            {
                if (GameManager.instance.DeliveryMan_Clear_level < 2)
                    PlayerPrefs.SetInt("DeliveryMan_Clear", 2);
            }
            else if (GameManager.instance.CoreGame_Stage_i == 3)
            {

                if (GameManager.instance.DeliveryMan_Clear_level < 3)
                    PlayerPrefs.SetInt("DeliveryMan_Clear", 3);
            }
            else if (GameManager.instance.CoreGame_Stage_i == 4)
            {
                if (GameManager.instance.DeliveryMan_Clear_level < 4)
                    PlayerPrefs.SetInt("DeliveryMan_Clear", 4);
            }
        }
        //2022.11.11
        GameManager.instance.CoreGame_Count += 1;
        PlayerPrefs.SetInt("CoreGameCount", GameManager.instance.CoreGame_Count);
        //
    }
}
