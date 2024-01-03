using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Training_UIManager : MonoBehaviour
{
    [Header("Training Video Clip")]
    public Image BackGroundIMG;
    public Sprite[] BackSprite;

    public GameObject TutorialRaw_ob; 
    public GameObject TrainingRaw_ob;
    public GameObject IndicatorBox;
    public GameObject IndicatorAngle;
    public VideoClip[] mVideoClip_training;  //비디오 칩
    public VideoClip[] mVideoClip_tutorial;  //비디오 칩

    public GameObject Finish_VideoPopup;
    public VideoClip mVideoClip_finish;

    [Header("Training UI")]
    [Header("Training Description Text")]
    public GameObject pDescription_ob; //운동 시작전 위치 

    public Image PostureDescription_img;
    public Sprite[] PostureDescription_sp;

    [SerializeField] List<Dictionary<string, string>> pDescription_strList;
    

    public Image TrainingName_img;
    public Sprite[] TrainingName_sp;
    public Text pDescriotion_stringTxt; //운동 설명

    public Text TrainingTimer; 
    [Header("Break Time Grup")]
    public GameObject BreakTimeGrup_ob;
    public float breakTimer;
    public Text BreakTimer_text;

    [Header("Exit Popup")]
    [SerializeField] GameObject ExitPopupGrup_ob;
    Button YesBtn, NoBtn;

    [Header("Count Down")]
    public Image CountDown_img;
    public Sprite[] countDown_sp;

    //영상 수정할 때, Video Handle 안에있는 mVideoPlayer, mVideoRawImage 변경하기 
  // [Header("Training Toutorial Video")]

    public void Tutorial_VideoPlay(int video_num)
    {
        VideoHandle.instance.Tutorial_VideoPlayer.clip = mVideoClip_tutorial[video_num];
        TutorialRaw_ob.gameObject.SetActive(true);
        VideoFinishTime_tutorial();
    }
    //트레이닝 영상 플레이
    public void Training_VideoPlay(int video_num)
    {
        VideoHandle.instance.Training_VideoPlayer.clip = mVideoClip_training[video_num];
        TrainingName_img.sprite = TrainingName_sp[Training_AppManager.instance.DataManager.Total_Training_count];
        TrainingRaw_ob.gameObject.SetActive(true);

        if (video_num == 6 || video_num == 7 || video_num == 8)
        {
            IndicatorBox.SetActive(false);
            IndicatorAngle.SetActive(true);
            // 초기화
            SensorManager.instance.resetRotation();
        }
        else
        {
            IndicatorBox.SetActive(true);
            IndicatorAngle.SetActive(false);
            // 초기화
            IndicatorCursor.distance = 0;
            SensorManager.instance.resetRotation();
        }

        VideoFinishTime_Training();       
    }


    //튜토리얼 비디오가 끝나는 타이밍 확인
    public void VideoFinishTime_tutorial()
    {
        StopCoroutine(_VideoFinishTime_tutorial());
        StartCoroutine(_VideoFinishTime_tutorial());
    }
    IEnumerator _VideoFinishTime_tutorial()
    {
        VideoHandle.instance.Tutorial_VideoPlayer.Play();
        while (true)
        {
            long playerCurrentFrame = VideoHandle.instance.Tutorial_VideoPlayer.frame;
            long playerFrameCount = Convert.ToInt64(VideoHandle.instance.Tutorial_VideoPlayer.frameCount);
            if (playerCurrentFrame < playerFrameCount - 10) { }
            else
            {
                print("VIDEO IS OVER");
                yield return new WaitForSeconds(1f);
                TutorialRaw_ob.SetActive(false);
                Training_AppManager.instance.DataManager.TutorialPlayOver = true;
                break;
            }
            yield return null;
        }
    }
    //트레이닝 비디오가 끝나는 타이밍 확인
    public void VideoFinishTime_Training()
    {
        StopCoroutine(_VideoFinishTime_Training());
        StartCoroutine(_VideoFinishTime_Training());
    }
    IEnumerator _VideoFinishTime_Training()
    {
        VideoHandle.instance.Training_PlayVideo();
        VideoHandle.instance.Training_VideoPlayer.Pause();

        TrainingTimer.text = getParseTime((float)VideoHandle.instance.Training_VideoPlayer.clip.length);
     pDescriotion_stringTxt.text
       = (int)VideoHandle.instance.Training_VideoPlayer.clip.length + "초 동안 정확한 "
       + Training_AppManager.instance.DataManager.training_Name[Training_AppManager.instance.DataManager.Total_Training_count] + " 자세를 유지해주세요.";
        
        //카운트 다운
        CountDown_img.gameObject.SetActive(true);
        for (int i =0; i<4; i++)
        {
            CountDown_img.sprite = countDown_sp[i];
            yield return new WaitForSeconds(1f);
        }
        CountDown_img.gameObject.SetActive(false);
        print("Restart Video");
        VideoHandle.instance.Training_VideoPlayer.Play();
     
        while (true)
        {
            long playerCurrentFrame = VideoHandle.instance.Training_VideoPlayer.frame;
            long playerFrameCount = Convert.ToInt64(VideoHandle.instance.Training_VideoPlayer.frameCount);

            TrainingTimer.text = getParseTime((float)VideoHandle.instance.Training_VideoPlayer.clip.length 
                - (float)VideoHandle.instance.Training_VideoPlayer.time).ToString();
            
            if (playerCurrentFrame < playerFrameCount-10)
            {
                // print("VIDEO IS PLAYING");
               // print("Timer :" + VideoHandle.instance.Training_VideoPlayer.time);
               // print("cilp.length :" + VideoHandle.instance.Training_VideoPlayer.clip.length);
                //Debug.Log("playerCurrentFrame : " + playerCurrentFrame + " " + "playerFrameCount : " + playerFrameCount);
            }
            else
            {
                print("VIDEO IS OVER");
               // yield return new WaitForSeconds(1f);
               // VideoHandle.instance.ClearOutRenderTexture(VideoHandle.instance.Training_VideoRawImage);
                StopCoroutine(_Play_Training_VdieoCtrl());
                StartCoroutine(_Play_Training_VdieoCtrl());
                break;
            }
            yield return null;
        }
     
    }
   
    //트레이닝 영상 
    IEnumerator _Play_Training_VdieoCtrl()
    {
        //제트업의 경우는 영상의 길이가 풀영상으로 반복하지 않고 끝나는 즉시 트레이닝 종료
        //난이도 초급, 영상을 한번만 플레이하고 다음으로 넘어감

        if (GameManager.instance.CoreLevel_str.Equals("Beginner"))
        {
            print("VIDEO IS OVER-0-------------------------4");
            //트레이닝 횟수 확인 난이도 별로 횟수가 다름 초급 1회, 중급 2회, 상급 3회
            Training_AppManager.instance.DataManager.TrainingRound_count += 1;
            if (Training_AppManager.instance.DataManager.TrainingRound_count == 1)
            {
             //  print("TrainingRound_count :" + Training_AppManager.instance.DataManager.TrainingRound_count);
                Training_AppManager.instance.DataManager.Total_Training_count += 1;
                Training_AppManager.instance.DataManager.TrainingPlayOver = true;
                TrainingRaw_ob.SetActive(false);
            }
        }
        //중급 2Round
        else if (GameManager.instance.CoreLevel_str.Equals("intermediate"))
        {
            Training_AppManager.instance.DataManager.TrainingRound_count += 1;
            //print("TrainingRound_count 1:" + Training_AppManager.instance.DataManager.TrainingRound_count);
            //제트업이 아닌경우 영상 여러번 반복
            if (!Training_AppManager.instance.DataManager.Total_Training_count.Equals(6))
            {
                if (Training_AppManager.instance.DataManager.TrainingRound_count == 2)
                {
                    Training_AppManager.instance.DataManager.Total_Training_count += 1;
                    Training_AppManager.instance.DataManager.TrainingPlayOver = true;
                    Training_AppManager.instance.DataManager.VideoPlayOver = true;
                    TrainingRaw_ob.SetActive(false);
                }
                else
                {
                    print("VIDEO IS OVER-0-------------------------5");
                    //휴식시간
                    BreakTime_Timer();//쉬는 시간
                    VideoHandle.instance.Training_StopVideo();
                    TrainingRaw_ob.gameObject.SetActive(false);
                    yield return new WaitUntil(() => Training_AppManager.instance.DataManager.BreakTimeOver == true);
                    Training_AppManager.instance.DataManager.BreakTimeOver = false;
                    //휴식시간 끝나고 다음 다시 동작 시작
                    Training_VideoPlay(Training_AppManager.instance.DataManager.Total_Training_count);

                }
            }
            else
            {
                //제트업 구간
                 print("VIDEO IS OVER-0-------------------------4");
                 print("TrainingRound_count :" + Training_AppManager.instance.DataManager.TrainingRound_count);
                 Training_AppManager.instance.DataManager.Total_Training_count += 1;
                 Training_AppManager.instance.DataManager.TrainingPlayOver = true;
            }
       
        
        }
        //상급 3Round
        else if (GameManager.instance.CoreLevel_str.Equals("Advanced"))
        {
            Training_AppManager.instance.DataManager.TrainingRound_count += 1;
            //print("TrainingRound_count 3:" + Training_AppManager.instance.DataManager.TrainingRound_count);
            if (!Training_AppManager.instance.DataManager.Total_Training_count.Equals(6))
            {

                if (Training_AppManager.instance.DataManager.TrainingRound_count == 3)
                {
                    Training_AppManager.instance.DataManager.Total_Training_count += 1;
                    Training_AppManager.instance.DataManager.TrainingPlayOver = true;
                    Training_AppManager.instance.DataManager.VideoPlayOver = true;
                    TrainingRaw_ob.SetActive(false);
                }
                else
                {
                    //휴식시간
                    BreakTime_Timer();//쉬는 시간
                    VideoHandle.instance.Training_StopVideo();
                    yield return new WaitUntil(() => Training_AppManager.instance.DataManager.BreakTimeOver == true);
                    Training_AppManager.instance.DataManager.BreakTimeOver = false;
                    //휴식시간 끝나고 다음 다시 동작 시작
                    //VideoHandle.instance.Training_PlayVideo();
                    Training_AppManager.instance.UIManager.Training_VideoPlay(Training_AppManager.instance.DataManager.Total_Training_count);
                }
            }
            else
            {
                //제트업 구간
                print("VIDEO IS OVER-0-------------------------4");
                print("TrainingRound_count :" + Training_AppManager.instance.DataManager.TrainingRound_count);
                Training_AppManager.instance.DataManager.Total_Training_count += 1;
                Training_AppManager.instance.DataManager.TrainingPlayOver = true;
            }
            yield return null;
        }
   
        yield return null;
    }

    //트레이닝 훈련 모두 끝나고 났을때 영상
    public void TrainingEnd_VideoPlay()
    {
        StopCoroutine(_TrainingEnd_VideoPlay());
        StartCoroutine(_TrainingEnd_VideoPlay());
    }
    IEnumerator _TrainingEnd_VideoPlay()
    {
        //튜토리얼 RenderRaw 안에 Finish영상 넣어줌
        //VideoHandle.instance.Training_VideoPlayer.clip = mVideoClip_finish;
        //VideoHandle.instance.Training_PlayVideo(); //비디오 시작하기
        Finish_VideoPopup.SetActive(true); //튜토리얼 팝업 활성화 

        VideoHandle.instance.Finish_VideoPlayer.Play();
        while (true)
        {
            long playerCurrentFrame = VideoHandle.instance.Finish_VideoPlayer.frame;
            long playerFrameCount = Convert.ToInt64(VideoHandle.instance.Finish_VideoPlayer.frameCount);
            if (playerCurrentFrame < playerFrameCount - 10)
            {

            }
            else
            {
                print("VIDEO IS OVER");
                GameManager.instance.LoadSceneName("02.ChooseMode");
                break;
            }
            yield return null;
        }
    }
    public void TrainingProgress_Save()
    {
        if (GameManager.instance.CoreLevel_str.Equals("Beginner"))
        {
            PlayerPrefs.SetInt("BeginnerClear", 1);
        }
        //중급 2Round
        else if (GameManager.instance.CoreLevel_str.Equals("intermediate"))
        {
            PlayerPrefs.SetInt("intermediateClear", 1);
        }
        //상급 3Round
        else if (GameManager.instance.CoreLevel_str.Equals("Advanced"))
        {
            PlayerPrefs.SetInt("AdvancedClear", 1);
        }
    }
    //////////////////////////////////////////////////////////////////////////////////
    //운동설명 팝업 스킵 했을 때
    public void DescriptionOver_Btn()
    {
        SoundCtrl.Instance.ClickButton_Sound();
        pDescription_ob.SetActive(false);
        Training_AppManager.instance.DataManager.DescriptionOver = true;
    }
    public void BreakTime_Timer()
    {
        StopCoroutine(_BreakTime_Timer());
        StartCoroutine(_BreakTime_Timer());
    }
    IEnumerator _BreakTime_Timer()
    {
        //VideoHandle.instance.Training_VideoRawImage.Release();
        BreakTimeGrup_ob.SetActive(true);
        breakTimer = 3f;
        while (breakTimer > 0)
        {
            breakTimer -= Time.deltaTime;
            BreakTimer_text.text = getParseTime(breakTimer);
            yield return null;
        }
        BreakTimer_text.text = "0";
        BreakTimeGrup_ob.SetActive(false);
        Training_AppManager.instance.DataManager.BreakTimeOver = true;
        yield return null;
    }
    //나가기 버튼 클릭
    public void OnClick_ExitBtn()
    {
        SoundCtrl.Instance.ClickButton_Sound();
        //종료 팝업 띄우기 
        Time.timeScale = 0;
        ExitPopupGrup_ob.SetActive(true);
    }
    //종료하기
    public void OnClick_ExitBtn_Yes()
    {
        SoundCtrl.Instance.ClickButton_Sound();
        //종료 팝업 띄우기 
        Time.timeScale = 1;
        GameManager.instance.LoadSceneName("02.ChooseMode");

    }
    //이어하기
    public void OnClick_ExitBtn_No()
    {
        SoundCtrl.Instance.ClickButton_Sound();
        //종료 팝업 띄우기 
        Time.timeScale = 1;
        ExitPopupGrup_ob.SetActive(false);
    }
    public void OnClick_Tutorial_SkipBtn()
    {
        SoundCtrl.Instance.ClickButton_Sound();
        VideoHandle.instance.Tutorial_VideoPlayer.Stop();
        TutorialRaw_ob.SetActive(false);
        Training_AppManager.instance.DataManager.TutorialPlayOver = true;
    }
    //시간 초 형태 변환
    public string getParseTime(float time)
    {
        string t = TimeSpan.FromSeconds(time).ToString("mm\\:ss");
        string[] tokens = t.Split(':');
        return tokens[0] + ':' + tokens[1];
    }
}
