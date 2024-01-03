using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public string UserName;
    public string UserAge;
    public string UserHeight;
    public string UserWeight;

    public string StartExerciseDay;
    public string LastExerciseDay;

    [Header("운동 리포트")]
    public int CoreTraing_Count; 
    public int CoreGame_Count;
    public float TotalPlay_Time;
    //코어 트레이닝 모드
    //초급자, 중급자, 상급자
    // 
    [Header("Core Training")]
    public int ProfileLevel; //트레이닝 레벨
    public string CoreLevel_str;// 초급, 중급, 상급
    public string CoreTrain_Name_str; //코어 트레이닝 명
   // public int TrainingLevel;

    //코어 게임 모드
    //Tower Maker, Delivery Man, Wire Walking
    public string CoreGame_Name_str; //코어 게임 명
    public int CoreGame_Stage_i; //코어 게임 레벨

    public int Tower_Clear_level;
    public int DeliveryMan_Clear_level;
    public int WireWalk_Clear_level;

    public static GameManager instance { get; private set; }

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        //PlayerPrefs.DeleteAll();
        Data_Initialization();
    }
    public void Data_Initialization()
    {
        if (PlayerPrefs.HasKey("Tower_Clear"))
        {
            Tower_Clear_level = PlayerPrefs.GetInt("Tower_Clear");
            DeliveryMan_Clear_level = PlayerPrefs.GetInt("DeliveryMan_Clear");
            WireWalk_Clear_level = PlayerPrefs.GetInt("WireWalking_Clear");
        }
        else
        {
            PlayerPrefs.SetInt("Tower_Clear", 0);
            PlayerPrefs.SetInt("DeliveryMan_Clear", 0);
            PlayerPrefs.SetInt("WireWalking_Clear", 0);
            Tower_Clear_level = 0;
            DeliveryMan_Clear_level = 0;
            WireWalk_Clear_level = 0;

        }
        //사용자 정보 저장
        UserName = PlayerPrefs.GetString("UserName");
        UserAge = PlayerPrefs.GetString("UserAge");
        UserHeight = PlayerPrefs.GetString("UserHeight");
        UserWeight = PlayerPrefs.GetString("UserWeight");
        StartExerciseDay= PlayerPrefs.GetString("StartExerciseDay");
        LastExerciseDay = PlayerPrefs.GetString("LastExerciseDay");
        //2022.11.11
        CoreTraing_Count = PlayerPrefs.GetInt("CoreTraing_Count");
        CoreGame_Count = PlayerPrefs.GetInt("CoreGameCount");
        TotalPlay_Time = PlayerPrefs.GetFloat("TotalPlayTime");
        //
        //Training Level 
        if (!PlayerPrefs.HasKey("BeginnerClear")) { PlayerPrefs.SetInt("BeginnerClear", 0); }
        if (!PlayerPrefs.HasKey("intermediateClear")) { PlayerPrefs.SetInt("intermediateClear", 0); }
        if (!PlayerPrefs.HasKey("AdvancedClear")) { PlayerPrefs.SetInt("AdvancedClear", 0); }

        ProfileLevel = PlayerPrefs.GetInt("BeginnerClear") + PlayerPrefs.GetInt("intermediateClear") + PlayerPrefs.GetInt("AdvancedClear");

    }
    public void LoadSceneName(string name)
    {
        SceneManager.LoadScene(name);
    }

}
