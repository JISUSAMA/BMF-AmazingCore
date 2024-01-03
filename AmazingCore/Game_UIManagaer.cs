using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_UIManagaer : MonoBehaviour
{
    [Header("In Game Timer")]
    public float playTime;
    public TextMeshProUGUI PlayTime_tmp;
    public GameObject CountDownPopup_ob;
    public Sprite[] CountNum_sp;

    [Header("RoundLevel UI")]

    public int Stage_i;
    public Image StageImg;
    public Sprite[] StageImg_sp;
    public GameObject[] StageGrup;
    
    public GameObject ClearPopup;
    public Text ClearText;
    public GameObject[] clear_nextStageBtn;
    
    public GameObject FailPopup;
    public GameObject ExitPopup;

    public Image Clear_Fevertime_img;
    public Sprite[] Clear_FeverTime_sp;
    public GameObject ClearParticle;
    public Button GameExitBtn;

    public GameObject TutorialPopup; //튜로리얼
    public GameObject[] Tutorial_obs; //7개

    [Range(0, 1f)]
    public float AnimationMoveValue; //blend 에서 사용되는 값

    [Header("Cinemachine")]
    public CinemachineDollyCart[] cDollyCart;
    public CinemachineSmoothPath[] cSmoothPath;

    [Header("3. TowerMaker")]
    public GameObject[] Tik_tok_prefebs;
    public Vector3 PosVector;
    public Camera cam;
    public Transform createTrs;
    public List<GameObject> ClimbCounts; //틱톡 프리팹의 갯수 확인
    public int ClimbCounts_i; // 총 몇번했는지 확인
    public Text ClimbCounts_text;

    public GameObject[] TikTok_FeverObGrup;
    public Slider TowerCountSlider; //타워를 쌓은 갯수
    bool FeverTime; //피버타임
    bool isResetPosition = false;
    public GameObject[] Stage_Clear_Fairy;

    //public CinemachineVirtualCamera TowerClear_Cam;

    [System.Serializable]

    public struct WireWalking
    {
        //스테이지 별, 카메라 전환에 필요한 카메라 
        public CinemachineVirtualCamera[] Stage1_Cam;
        public CinemachineVirtualCamera[] Stage2_Cam;
        public CinemachineVirtualCamera[] Stage3_Cam;
        public CinemachineVirtualCamera[] Stage4_Cam;
        public CinemachineVirtualCamera Fail_Cam;

        public Animator[] TikTok_stage1_ani;
        public Animator[] TikTok_stage2_ani;
        public Animator[] TikTok_stage3_ani;
        public Animator[] TikTok_stage4_ani;

        public GameObject[] ClearParticleGrup;
        public bool CmMotionFinish;
        public GameObject FailCameraMotion_ob;
        public Vector3 FailCameraMotion_vec;

    }
    [Header("3. Wire Walking")]
    public WireWalking WireWalk;


    [Header("3. Delivery Man")]
    public Animator DeliveryManAni;
    public GameObject[] Mandu_left_ob;
    public GameObject[] Mandu_right_ob;
    public float pos;
    public static Game_UIManagaer instance { get; private set; }
    public void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;

        //UI_Initialization();
        
    }


    public void UI_Initialization()
    {
        GameExitBtn.onClick.AddListener(() => OnClicK_PopupOpen(ExitPopup)); //나가기 버튼 

        if (GameManager.instance.CoreGame_Stage_i.Equals(1))
        {
            StageImg.sprite = StageImg_sp[0]; //스테이지 넘버
            clear_nextStageBtn[0].SetActive(true);
            clear_nextStageBtn[1].SetActive(false);
            ClearText.text = "축하합니다! 레벨 1을 클리어하셨어요. 다음 단계로 바로 넘어갈까요?";
        }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(2))
        {
            StageImg.sprite = StageImg_sp[1];//스테이지 넘버
            clear_nextStageBtn[0].SetActive(true);
            clear_nextStageBtn[1].SetActive(false);
            ClearText.text = "축하합니다! 레벨 2을 클리어하셨어요. 다음 단계로 바로 넘어갈까요?";
        }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(3))
        {
            StageImg.sprite = StageImg_sp[2];//스테이지 넘버
            clear_nextStageBtn[0].SetActive(true);
            clear_nextStageBtn[1].SetActive(false);
            ClearText.text = "축하합니다! 레벨 3을 클리어하셨어요. 다음 단계로 바로 넘어갈까요?";
        }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(4))
        {
            StageImg.sprite = StageImg_sp[3];//스테이지 넘버
            clear_nextStageBtn[0].SetActive(false);
            clear_nextStageBtn[1].SetActive(true);
            ClearText.text = "축하합니다!\n모든 스테이지를 클리어하셨어요.";
        }

    }
    //게임 플레이 시간 관련
    public void Timer()
    {
        StopCoroutine(_Timer());
        StartCoroutine(_Timer());
    }
    IEnumerator _Timer()
    {
      
        while (Game_AppManager.instance.PlayStart)
        {
            playTime += Time.deltaTime;
            PlayTime_tmp.text = getParseTime(Game_DataManager.instance.level_playTime_step - playTime);
            if (playTime >= Game_DataManager.instance.level_playTime_step)
            {
                //Debug.Log("cDollyCart : " + cDollyCart[0].m_Position);
                Game_AppManager.instance.PlayStart = false;
            }
            yield return null;
        }

        yield return null;
    }
    public string getParseTime(float time)
    {
        string t = TimeSpan.FromSeconds(time).ToString("mm\\:ss");
        string[] tokens = t.Split(':');
        return tokens[0] + ':' + tokens[1];
    }
    //튜토리얼 관련
    public void Tutorial_start()
    {
        StopCoroutine(_Tutorial_start());
        StartCoroutine(_Tutorial_start());
    }
    IEnumerator _Tutorial_start()
    {
        if (GameManager.instance.CoreGame_Name_str.Equals("DeliveryMan"))
        {
            if (PlayerPrefs.HasKey("DeliveryMan_tutorial")) { Game_AppManager.instance.Tutorial = true; }
            else
            {
                PlayerPrefs.SetString("DeliveryMan_tutorial", "true");
                TutorialPopup.SetActive(true);
            }
        }
        else if (GameManager.instance.CoreGame_Name_str.Equals("TowerMaker"))
        {
            if (PlayerPrefs.HasKey("TowerMaker_tutorial")){Game_AppManager.instance.Tutorial = true; }
            else 
            {
                PlayerPrefs.SetString("TowerMaker_tutorial", "true");
                TutorialPopup.SetActive(true);
            }
        }
        else if (GameManager.instance.CoreGame_Name_str.Equals("WireWalking"))
        {
            if (PlayerPrefs.HasKey("WireWalking_tutorial")) { Game_AppManager.instance.Tutorial = true; }
            else
            {
                PlayerPrefs.SetString("WireWalking_tutorial", "true");
                TutorialPopup.SetActive(true);
            }
        }
        yield return null;
    }
    public void FinishTutorial(GameObject lastPopup)
    {
        Game_AppManager.instance.Tutorial = true;
        lastPopup.SetActive(false);
    }
    public void OnClicK_PopupOpen(GameObject ob)
    {
        SoundCtrl.Instance.ClickButton_Sound();
        Time.timeScale = 0;
        ob.SetActive(true);
    }
    public void OnClick_PopupClose(GameObject ob)
    {
        SoundCtrl.Instance.ClickButton_Sound();
        Time.timeScale = 1;
        ob.SetActive(false);
    }
    //재도전
    public void OnClick_Retry()
    {
        SoundCtrl.Instance.ClickButton_Sound();
        Time.timeScale = 1;
        string name = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(name);
    }
    //나가기
    public void OnClick_GameExit()
    {
        SoundCtrl.Instance.ClickButton_Sound();
        Time.timeScale = 1;
        SceneManager.LoadScene("02.ChooseMode");
    }
    //다음 단계로
    public void OnClick_NextLevel()
    {
        SoundCtrl.Instance.ClickButton_Sound();
        if (GameManager.instance.CoreGame_Name_str.Equals("DeliveryMan")) 
        {
            SceneManager.LoadScene("DeliveryMan");
            GameManager.instance.CoreGame_Stage_i += 1; 
        }
        else if (GameManager.instance.CoreGame_Name_str.Equals("TowerMaker")) 
        { 
            SceneManager.LoadScene("TowerMaker");
            GameManager.instance.CoreGame_Stage_i += 1;
        }
        else if (GameManager.instance.CoreGame_Name_str.Equals("WireWalking")) 
        { 
            SceneManager.LoadScene("WireWalking");
            GameManager.instance.CoreGame_Stage_i += 1;
        }
    }
    ///////////////////////////// Delivery Man ////////////////////////////////////
    public void DeliveryMan_Play()
    {
        StopCoroutine(_DeliveryMan_Play());
        StartCoroutine(_DeliveryMan_Play());
    }
    IEnumerator _DeliveryMan_Play()
    {
        DeliveryMan_BasicSetting(); //만두 활성화
        yield return new WaitUntil(() => Game_AppManager.instance.PlayStart == true);
        Game_DataManager.instance.Game_Succsed = "Clear";

        float distRatio;
        float AnimationMoveValue;

        cDollyCart[0].m_Speed = 5;
        while (Game_AppManager.instance.PlayStart == true)
        {
            float value = DeliveryManAni.GetFloat("Blend");
            //흔들림 최대치 일 경우, 게임이 끝남
            //코어를 유지하고 시간동안 도착 했을 경우, clear
            //중간에 무너지면, Fail
            //if (value == 1)
            //{
            //    Game_DataManager.instance.Game_Succsed = "Fail";
            //    int random = UnityEngine.Random.RandomRange(0, 1); //오른쪽,왼쪽 랜덤 애니메이션 값
            //    if (random.Equals(0)) { DeliveryManAni.SetTrigger("Fail_left"); }
            //    else { DeliveryManAni.SetTrigger("Fail_right"); }
            //    Game_AppManager.instance.PlayStart = false;
            //    break;
            //}

            distRatio = IndicatorCursor.distance;
            AnimationMoveValue = Mathf.InverseLerp(0f, 180f, distRatio);    // 0 ~ 1

            //float value = DeliveryManAni.GetFloat("Blend");
            //흔들림 최대치 일 경우, 게임이 끝남
            //코어를 유지하고 시간동안 도착 했을 경우, clear
            //중간에 무너지면, Fail
            if (AnimationMoveValue >= 1)
            {
                Game_DataManager.instance.Game_Succsed = "Fail";
                int random = UnityEngine.Random.Range(0, 1); //오른쪽,왼쪽 랜덤 애니메이션 값
                if (random.Equals(0)) { DeliveryManAni.SetTrigger("Fail_left"); }
                else { DeliveryManAni.SetTrigger("Fail_right"); }
                Game_AppManager.instance.PlayStart = false;
                break;
            }

            yield return null;
        }
        //타이머가 0이 되면 playerStart가 false가 되면서 넘어감
        yield return new WaitUntil(() => Game_AppManager.instance.PlayStart == false);
        cDollyCart[0].m_Speed = 0;

        Debug.Log("인티케이터 비활성화");
        IndicatorCursor.distance = 0f;
        Game_AppManager.instance.indicator.SetActive(false);

        //Game_Succsed가 "Clear"일 경우, 카트 멈추고 성공 모션
        if (Game_DataManager.instance.Game_Succsed.Equals("Clear"))
        {
            Debug.Log("성공");
            DeliveryManAni.SetTrigger("Clear");
            Game_DataManager.instance.Save_GameClearData(); //성공 데이터 저장
            yield return new WaitForSeconds(2f);
            Clear_Fevertime_img.gameObject.SetActive(true);
            Clear_Fevertime_img.sprite = Clear_FeverTime_sp[0]; //clear img
            //파티클
            yield return new WaitForSeconds(2f);
            Clear_Fevertime_img.gameObject.SetActive(false);
            //게임 성공
            ClearPopup.SetActive(true);
        }
        //Game_Succsed가 "Fail"일 경우,
        else
        {
            int randSoundPlay = UnityEngine.Random.Range(0, 2);
            if (randSoundPlay.Equals(0))
            {
                SoundManager.instance.PlaySFX_OneShot("fall3");
            }
            else { SoundManager.instance.PlaySFX_OneShot("fall2"); }
            Debug.Log("실패!");
            Debug.Log("실패!");
            FailPopup.SetActive(true);
            //게임 오버
        }

    }
    public void DeliveryMan_BasicSetting()
    {
        if (GameManager.instance.CoreGame_Stage_i.Equals(1))
        {
            Mandu_left_ob[0].SetActive(true); Mandu_right_ob[0].SetActive(true);
        }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(2))
        {
            for (int i = 0; i < 2; i++) { Mandu_left_ob[i].SetActive(true); Mandu_right_ob[i].SetActive(true); }
        }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(3))
        {
            for (int i = 0; i < 3; i++) { Mandu_left_ob[i].SetActive(true); Mandu_right_ob[i].SetActive(true); }
        }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(4))
        {
            for (int i = 0; i < 4; i++) { Mandu_left_ob[i].SetActive(true); Mandu_right_ob[i].SetActive(true); }
        }
    }
    ///////////////////////////// TowerMaker ////////////////////////////////////
    /*1단계 :  30초동안 10개
      2단계 :  50초동안 15개
      3단계 :  70초동안  30개
      6단계  :  90초동안 45개*/

    public void TowerMaker_Initialization()
    {
        //슬라이더 값 초기화
        TowerCountSlider.value = 0;
        Stage_i = GameManager.instance.CoreGame_Stage_i - 1;
        if (GameManager.instance.CoreGame_Stage_i.Equals(1)) { TowerCountSlider.maxValue = 10; }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(2)) { TowerCountSlider.maxValue = 15; }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(3)) { TowerCountSlider.maxValue = 30; }
        else if (GameManager.instance.CoreGame_Stage_i.Equals(4)) { TowerCountSlider.maxValue = 45; }

        if (Stage_i.Equals(0)) { Set_StageCanvase(true, false, false, false); }
        else if (Stage_i.Equals(1)) { Set_StageCanvase(false, true, false, false); }
        else if (Stage_i.Equals(2)) { Set_StageCanvase(false, false, true, false); }
        else if (Stage_i.Equals(3)) { Set_StageCanvase(false, false, false, true); }
    }
    public void TowerMaker_Play()
    {
       StopCoroutine(_TowerMaker_Play());
       StartCoroutine(_TowerMaker_Play());
    }
    IEnumerator _TowerMaker_Play()
    {
        if (Stage_i.Equals(0))
        {
            yield return new WaitUntil(() => ClimbCounts.Count >= 10);
        }
        else if (Stage_i.Equals(1))
        {
            yield return new WaitUntil(() => ClimbCounts.Count >= 15);
        }
        else if (Stage_i.Equals(2))
        {
            yield return new WaitUntil(() => ClimbCounts.Count >=30);
        }
        else if (Stage_i.Equals(3))
        {
            yield return new WaitUntil(() => ClimbCounts.Count >= 45);
        }
        TowerMakerClear();        
        yield return new WaitUntil(() => Game_AppManager.instance.PlayStart == false); //게임이 끝나면 false가 됨
        Clear_Fevertime_img.gameObject.SetActive(false);
        //Game_Succsed가 "Clear"일 경우, 카트 멈추고 성공 모션
        if (Game_DataManager.instance.Game_Succsed.Equals("Clear"))
        {
         //   Debug.Log("성공");
            //게임 성공
            ClearPopup.SetActive(true);
        }
        //Game_Succsed가 "Fail"일 경우,
        else
        {
           // Debug.Log("실패!");
            FailPopup.SetActive(true);
            //게임 오버
        }
        yield return null;
    }
    public void MoveFollow_CamMotion()
    {
        StartCoroutine(_MoveFollow_CamMotion());
    }
    IEnumerator _MoveFollow_CamMotion()
    {
        float posY = cam.gameObject.transform.position.y;
    
        while (cam.gameObject.transform.position.y < PosVector.y)
        {
            Debug.Log(" posY" + posY);
            Debug.Log(" PosVector.y" + PosVector.y);
            posY += 15f;
            cam.gameObject.transform.position = new Vector3(cam.gameObject.transform.position.x, posY, cam.gameObject.transform.position.z);
            if (cam.gameObject.transform.position.y < posY)
            {
                cam.gameObject.transform.position = new Vector3(cam.gameObject.transform.position.x, PosVector.y, cam.gameObject.transform.position.z);
            }
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    void TowerMakerClear()
    {
        StopCoroutine(_TowerMakerClear());
        StartCoroutine(_TowerMakerClear());
    }
    IEnumerator _TowerMakerClear()
    {
        Game_DataManager.instance.Save_GameClearData(); //성공 데이터 저장
        ClearParticle.SetActive(true);
        //yield return new WaitUntil(() => cDollyCart[0].m_Speed == 0);
        FeverTime = true;
        Stage_Clear_Fairy[Stage_i].SetActive(true);
        Debug.Log("FeverTime !!!!!!!!!!!!!!!!");
        Clear_Fevertime_img.gameObject.SetActive(true);
        Clear_Fevertime_img.sprite = Clear_FeverTime_sp[0]; //피버타임
        yield return new WaitForSeconds(2f);
        Clear_Fevertime_img.sprite = Clear_FeverTime_sp[1]; //피버타임
        Game_DataManager.instance.Game_Succsed = "Clear";
    }
    void Set_StageCanvase(bool s1, bool s2, bool s3, bool s4)
    {
        StageGrup[0].SetActive(s1);
        StageGrup[1].SetActive(s2);
        StageGrup[2].SetActive(s3);
        StageGrup[3].SetActive(s4);

    }
    public void ClimbCounts_Slider()
    {
        StopCoroutine(_ClimbCounts_Slider());
        StartCoroutine(_ClimbCounts_Slider());
    }
    IEnumerator _ClimbCounts_Slider()
    {
        while (true)
        {
            // 카운팅
            if (SensorManager.instance._connected && Game_AppManager.instance.PlayStart)
            {
                Debug.Log($"Cube : {SensorManager.instance.cube.rotation.eulerAngles.y} / reset : {isResetPosition}");
                if (SensorManager.instance.cube.rotation.eulerAngles.y <= 10 && SensorManager.instance.cube.rotation.eulerAngles.y >= 0
                    || SensorManager.instance.cube.rotation.eulerAngles.y <= 360 && SensorManager.instance.cube.rotation.eulerAngles.y >= 350)
                {
                    Debug.Log("Reset : " + isResetPosition);
                    // Reset
                    isResetPosition = true;
                }

                if (SensorManager.instance.cube.rotation.eulerAngles.y >= 45 && SensorManager.instance.cube.rotation.eulerAngles.y <= 90)
                {
                    // 45 ~ 90
                    if (isResetPosition)
                    {
                        isResetPosition = false;
                        //ClimbCounts_i += 1;
                        Create_TikTok_Prefebs();
                    }
                }
                else if (SensorManager.instance.cube.rotation.eulerAngles.y <= 315 && SensorManager.instance.cube.rotation.eulerAngles.y >= 270)
                {
                    // -45 ~ -90
                    if (isResetPosition)
                    {
                        isResetPosition = false;
                        //ClimbCounts_i += 1;
                        Create_TikTok_Prefebs();
                    }
                }
            }

            ClimbCounts_text.text = ClimbCounts_i.ToString(); //틱톡 몇개?
            TowerCountSlider.value = ClimbCounts.Count; //슬라이더 값을 올려줌
            if (Game_AppManager.instance.PlayStart == false) { break; }
            yield return null;
        }
    }
    public void Create_TikTok_Prefebs()
    {
        if (Game_AppManager.instance.PlayStart)
        {
   
            int rand_TikTok = UnityEngine.Random.Range(0, Tik_tok_prefebs.Length);
            ClimbCounts_i += 1; //한번 할때마다 하나씩 올려줌
                                //틱톡을 최대 생성 갯수
            if (Stage_i.Equals(0) && ClimbCounts.Count < 10)
            {
                GameObject spawn = Instantiate(Tik_tok_prefebs[rand_TikTok], createTrs.position, Quaternion.identity);
                ClimbCounts.Add(spawn); //마운틴 클라이밍 횟수
            }
            else if (Stage_i.Equals(1) && ClimbCounts.Count < 15)
            {
                GameObject spawn = Instantiate(Tik_tok_prefebs[rand_TikTok], createTrs.position, Quaternion.identity);
                ClimbCounts.Add(spawn); //마운틴 클라이밍 횟수
            }
            else if (Stage_i.Equals(2) && ClimbCounts.Count < 30)
            {
                GameObject spawn = Instantiate(Tik_tok_prefebs[rand_TikTok], createTrs.position, Quaternion.identity);
                ClimbCounts.Add(spawn); //마운틴 클라이밍 횟수
            }
            else if (Stage_i.Equals(3) && ClimbCounts.Count < 45)
            {
                GameObject spawn = Instantiate(Tik_tok_prefebs[rand_TikTok], createTrs.position, Quaternion.identity);
                ClimbCounts.Add(spawn); //마운틴 클라이밍 횟수
            }
        }
    }
    ///////////////////////////// WireWalking Game ////////////////////////////////////
    public void WireWalking_Initialization()
    {
        //스테이지에 따른 캔버스 활성화 시킴
        Stage_i = GameManager.instance.CoreGame_Stage_i - 1;
        if (Stage_i.Equals(0))
        {
            Set_StageCanvase(true, false, false, false);
            WireWalk.Stage1_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width = 0;

        }
        else if (Stage_i.Equals(1))
        {
            Set_StageCanvase(false, true, false, false);
            WireWalk.Stage2_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width = 0;
        }
        else if (Stage_i.Equals(2))
        {
            Set_StageCanvase(false, false, true, false);
            WireWalk.Stage3_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width = 0;
        }
        else if (Stage_i.Equals(3))
        {
            Set_StageCanvase(false, false, false, true);
            WireWalk.Stage4_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width = 0;
        }
       // WireWalk.Fail_Cam.gameObject.GetComponent<CinemachineVirtualCamera>().m_Priority = 0; //게임 끝 카메라 

    }
    public void WireWalking_start()
    {
        StopCoroutine(_WireWalking_start());
        StartCoroutine(_WireWalking_start());
    }
    IEnumerator _WireWalking_start()
    {
        //카운트 다운 끝나고 나서 게임 시작 
        yield return new WaitUntil(() => Game_AppManager.instance.PlayStart == true);
        TikTok_animation_Play(); //틱톡 애니메이션 시작

        // IndicatorCursor.distance : 0 ~ maxDistance
        float distRatio;

        Game_DataManager.instance.Game_Succsed = "Clear"; //실패하기 전 까지는 Clear 상태 , 넘어질 경우, Fail
        while (Game_AppManager.instance.PlayStart == true)
        {
            //if (!AnimationMoveValue.Equals(1))
            //{
            //    //1 stage
            //    if (Stage_i.Equals(0))
            //    {
            //        WireWalk.TikTok_stage1_ani[0].SetFloat("Blend", AnimationMoveValue);
            //        WireWalk.TikTok_stage1_ani[1].SetFloat("Blend", AnimationMoveValue);
            //        WireWalk.TikTok_stage1_ani[2].SetFloat("Blend", AnimationMoveValue);
            //    }
            //    //2 Stage
            //    else if (Stage_i.Equals(1))
            //    {
            //        WireWalk.TikTok_stage2_ani[0].SetFloat("Blend", AnimationMoveValue);
            //        WireWalk.TikTok_stage2_ani[1].SetFloat("Blend", AnimationMoveValue);
            //        WireWalk.TikTok_stage2_ani[2].SetFloat("Blend", AnimationMoveValue);
            //    }
            //    //3 Stage
            //    else if (Stage_i.Equals(2))
            //    {
            //        WireWalk.TikTok_stage3_ani[0].SetFloat("Blend", AnimationMoveValue);
            //        WireWalk.TikTok_stage3_ani[1].SetFloat("Blend", AnimationMoveValue);
            //        WireWalk.TikTok_stage3_ani[2].SetFloat("Blend", AnimationMoveValue);
            //    }
            //    //4 Stage
            //    else if (Stage_i.Equals(3))
            //    {
            //        WireWalk.TikTok_stage4_ani[0].SetFloat("Blend", AnimationMoveValue);
            //        WireWalk.TikTok_stage4_ani[1].SetFloat("Blend", AnimationMoveValue);
            //        WireWalk.TikTok_stage4_ani[2].SetFloat("Blend", AnimationMoveValue);
            //    }

            //IndicatorCursor.distance            
            // Calc > 0 ~ 180 => 0 ~ 1            
            distRatio = IndicatorCursor.distance;
            AnimationMoveValue = Mathf.InverseLerp(0f, 180f, distRatio);

            if (AnimationMoveValue < 1)
            {
                //1 stage
                if (Stage_i.Equals(0))
                {
                    WireWalk.TikTok_stage1_ani[0].SetFloat("Blend", AnimationMoveValue);
                    WireWalk.TikTok_stage1_ani[1].SetFloat("Blend", AnimationMoveValue);
                    WireWalk.TikTok_stage1_ani[2].SetFloat("Blend", AnimationMoveValue);
                }
                //2 Stage
                else if (Stage_i.Equals(1))
                {
                    WireWalk.TikTok_stage2_ani[0].SetFloat("Blend", AnimationMoveValue);
                    WireWalk.TikTok_stage2_ani[1].SetFloat("Blend", AnimationMoveValue);
                    WireWalk.TikTok_stage2_ani[2].SetFloat("Blend", AnimationMoveValue);
                }
                //3 Stage
                else if (Stage_i.Equals(2))
                {
                    WireWalk.TikTok_stage3_ani[0].SetFloat("Blend", AnimationMoveValue);
                    WireWalk.TikTok_stage3_ani[1].SetFloat("Blend", AnimationMoveValue);
                    WireWalk.TikTok_stage3_ani[2].SetFloat("Blend", AnimationMoveValue);
                }
                //4 Stage
                else if (Stage_i.Equals(3))
                {
                    WireWalk.TikTok_stage4_ani[0].SetFloat("Blend", AnimationMoveValue);
                    WireWalk.TikTok_stage4_ani[1].SetFloat("Blend", AnimationMoveValue);
                    WireWalk.TikTok_stage4_ani[2].SetFloat("Blend", AnimationMoveValue);
                }
            }
            else
            {
                //게임 끝! 틱톡 떨어짐
                Game_DataManager.instance.Game_Succsed = "Fail";
                Game_AppManager.instance.PlayStart = false;
                break;
            }
            yield return null;
        }
        //타이머가 0이 되면 playerStart가 false가 되면서 넘어감
        yield return new WaitUntil(() => Game_AppManager.instance.PlayStart == false);
        TikTok_animation_Finish(); //애들 멈춰!
        Debug.Log("인티케이터 비활성화");
        IndicatorCursor.distance = 0;
        Game_AppManager.instance.indicator.SetActive(false);

        //Game_Succsed가 "Clear"일 경우, 카트 멈추고 성공 모션
        if (Game_DataManager.instance.Game_Succsed.Equals("Clear"))
        {
            Debug.Log("성공");
            TikTok_animation_Clear_Finish(); //카메라 모션 시작
            yield return new WaitUntil(() => WireWalk.CmMotionFinish == true); // 캠모션 끝!
            Game_DataManager.instance.Save_GameClearData(); //성공 데이터 저장
            Clear_Fevertime_img.gameObject.SetActive(true);
            Clear_Fevertime_img.sprite = Clear_FeverTime_sp[0]; //clear img
            yield return new WaitForSeconds(2f);
            Clear_Fevertime_img.gameObject.SetActive(false);
            //게임 성공
            ClearPopup.SetActive(true);
        }
        //Game_Succsed가 "Fail"일 경우,
        else
        {
            SoundManager.instance.PlaySFX_OneShot("drop2");
            TikTok_animation_Fail_Finish();
            yield return new WaitUntil(() => WireWalk.CmMotionFinish == true); // 캠모션 끝!

            yield return new WaitForSeconds(2f);
            Debug.Log("실패!");
            FailPopup.SetActive(true);
            //게임 오버
        }
        yield return null;
    }
    public void WireWalk_StartCamMotion()
    {
        Debug.Log("들어옹ㅁ????????????????");
        //1 stage
        if (Stage_i.Equals(0))
        {
            WireWalk.Stage1_Cam[2].m_Priority = 10;
        }
        //2 Stage
        else if (Stage_i.Equals(1))
        {
            WireWalk.Stage2_Cam[2].m_Priority = 10;
        }
        //3 Stage
        else if (Stage_i.Equals(2))
        {
            WireWalk.Stage3_Cam[2].m_Priority = 10;
        }
        //4 Stage
        else if (Stage_i.Equals(3))
        {
           WireWalk.Stage4_Cam[2].m_Priority = 10;
        }
    }

    //틱톡 애니메이션 시작 , 카트 m_speed= 5 설정
    public void TikTok_animation_Play()
    {
        //1 stage 
        if (Stage_i.Equals(0))
        {
            for (int i = 0; i < WireWalk.TikTok_stage1_ani.Length; i++)
            {
                WireWalk.TikTok_stage1_ani[i].SetTrigger("GamePlay");
            }
            cDollyCart[0].m_Speed = 5;
        }
        //2 Stage
        else if (Stage_i.Equals(1))
        {
            for (int i = 0; i < WireWalk.TikTok_stage2_ani.Length; i++)
            {
                WireWalk.TikTok_stage2_ani[i].SetTrigger("GamePlay");
            }
            cDollyCart[1].m_Speed = 5;
        }
        //3 Stage
        else if (Stage_i.Equals(2))
        {
            for (int i = 0; i < WireWalk.TikTok_stage3_ani.Length; i++)
            {
                WireWalk.TikTok_stage3_ani[i].SetTrigger("GamePlay");
            }
            cDollyCart[2].m_Speed = 5;
        }
        //4 Stage
        else if (Stage_i.Equals(3))
        {
            for (int i = 0; i < WireWalk.TikTok_stage4_ani.Length; i++)
            {
                WireWalk.TikTok_stage4_ani[i].SetTrigger("GamePlay");
            }
            cDollyCart[3].m_Speed = 5;
        }
    }
    public void TikTok_animation_Finish()
    {
        //1 stage 
        if (Stage_i.Equals(0))
        {
            for (int i = 0; i < WireWalk.TikTok_stage1_ani.Length; i++)
            {
                WireWalk.TikTok_stage1_ani[i].SetTrigger("Clear");
            }
            cDollyCart[0].m_Speed = 0;
        }
        //2 Stage
        else if (Stage_i.Equals(1))
        {
            for (int i = 0; i < WireWalk.TikTok_stage2_ani.Length; i++)
            {
                WireWalk.TikTok_stage2_ani[i].SetTrigger("Clear");
            }
            cDollyCart[1].m_Speed = 0;
        }
        //3 Stage
        else if (Stage_i.Equals(2))
        {
            for (int i = 0; i < WireWalk.TikTok_stage3_ani.Length; i++)
            {
                WireWalk.TikTok_stage3_ani[i].SetTrigger("Clear");
            }
            cDollyCart[2].m_Speed = 0;
        }
        //4 Stage
        else if (Stage_i.Equals(3))
        {
            for (int i = 0; i < WireWalk.TikTok_stage4_ani.Length; i++)
            {
                WireWalk.TikTok_stage4_ani[i].SetTrigger("Clear");
            }
            cDollyCart[3].m_Speed = 0;
        }
    }

    //WireWalking 성공 했을 때 ! VideoMotion
    public void TikTok_animation_Clear_Finish()
    {
        WireWalk.ClearParticleGrup[Stage_i].SetActive(true);
        //1 stage 
        if (Stage_i.Equals(0))
        {
            WireWalk.Stage1_Cam[1].m_Priority = 12; // 카메라 우선순위
            float motionT = WireWalk.Stage1_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width;
            while (motionT < 14)
            {
                motionT += Time.deltaTime * 0.5f;
                WireWalk.Stage1_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width = motionT;
                if (motionT >= 14)
                {
                    WireWalk.Stage1_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width = 14;
                    WireWalk.CmMotionFinish = true;// 캠모션 끝!
                    break;
                }
            }
        }
        //2 Stage
        else if (Stage_i.Equals(1))
        {
            WireWalk.Stage2_Cam[1].m_Priority = 12; // 카메라 우선순위 주기
            float motionT = WireWalk.Stage2_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width;
            while (motionT < 14)
            {
                motionT += Time.deltaTime * 0.5f;
                WireWalk.Stage2_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width = motionT;
                if (motionT >= 14)
                {
                    WireWalk.Stage2_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width = 14;
                    WireWalk.CmMotionFinish = true;// 캠모션 끝!
                    break;
                }
            }
        }
        //3 Stage
        else if (Stage_i.Equals(2))
        {
            WireWalk.Stage3_Cam[1].m_Priority = 12; // 카메라 우선순위 주기
            float motionT = WireWalk.Stage3_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width;
            while (motionT < 14)
            {
                motionT += Time.deltaTime * 0.5f;
                WireWalk.Stage3_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width = motionT;
                if (motionT >= 14)
                {
                    WireWalk.Stage3_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width = 14;
                    WireWalk.CmMotionFinish = true;// 캠모션 끝!
                    break;
                }
            }
        }
        //4 Stage
        else if (Stage_i.Equals(3))
        {
            WireWalk.Stage4_Cam[1].m_Priority = 12; // 카메라 우선순위 주기
            float motionT = WireWalk.Stage4_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width;
            while (motionT < 14)
            {
                motionT += Time.deltaTime * 0.5f;
                WireWalk.Stage4_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width = motionT;
                if (motionT >= 14)
                {
                    WireWalk.Stage4_Cam[1].gameObject.GetComponent<CinemachineFollowZoom>().m_Width = 14;
                    WireWalk.CmMotionFinish = true;// 캠모션 끝!
                    break;
                }
            }
        }


    }
    //줄타기 게임 실패 
    public void TikTok_animation_Fail_Finish()
    {
        StopCoroutine(_TikTok_animation_Fail_Finish());
        StartCoroutine(_TikTok_animation_Fail_Finish());
    }
    IEnumerator _TikTok_animation_Fail_Finish()
    {
        WireWalk.Fail_Cam.m_Priority = 12;
        while (pos > -900)
        {
            pos -= 5;
            WireWalk.FailCameraMotion_vec = new Vector3(WireWalk.FailCameraMotion_vec.x, pos, WireWalk.FailCameraMotion_vec.z);
            WireWalk.FailCameraMotion_ob.transform.position = WireWalk.FailCameraMotion_vec;
            yield return new WaitForFixedUpdate();
        }
        WireWalk.CmMotionFinish = true;
    }
}
