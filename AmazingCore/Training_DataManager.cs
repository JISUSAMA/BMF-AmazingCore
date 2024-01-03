using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Training_DataManager : MonoBehaviour
{
    public bool DescriptionOver; //운동설명 시간
    public bool TutorialPlayOver;//튜토리얼 시간
    public bool TrainingPlayOver;   //트레이닝 시간
    public bool BreakTimeOver; //쉬는 시간
    public bool VideoPlayOver = false;

   // public int Training_Number;
    public int Total_Training_count; //모든 종목을 레벨에 따라서 다했는가 확인
    public int TrainingRound_count; //한 종목을 몇번 했는지 확인, 1 : 초급, 2: 중급 3:상급
    public string Training_step_str;

    public string[] training_Name = {};  //운동 이름
    public float trainingTime;
    // public string[] training_descript; //운동설명
   

    //처음 플레이 시작했을 때 값 초기화
    public void Initialization()
    {
     //   Training_Number = 0;
        Total_Training_count = 0;
        Training_step_str = "";
        DescriptionOver = false;
        TutorialPlayOver = false;
        TrainingPlayOver = false;
        VideoPlayOver = false;
        BreakTimeOver = false;
    }
    //영상 플레이 관련 데이터 초기화
    public void Check_PlayVideo_init()
    {
        DescriptionOver = false;
        TutorialPlayOver = false;
        TrainingPlayOver = false;
        VideoPlayOver = false;
        BreakTimeOver = false;
        TrainingRound_count = 0;
    }
  

}
