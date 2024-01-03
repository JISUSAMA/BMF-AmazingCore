using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_AppManager : MonoBehaviour
{
    public UnityEvent OnAwakeEvnet;
    public UnityEvent OnStartEvnet;

    public bool PlayStart = false;
    public bool Tutorial = false;

    public GameObject indicator;
    public GameObject bleDisconnectAlert;
    public static Game_AppManager instance { get; private set; }

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

#if !UNITY_EDITOR && UNITY_ANDROID
        if (!SensorManager.instance._connected)
        {
            SoundCtrl.Instance.ClickButton_Sound();
            Time.timeScale = 0;
            bleDisconnectAlert.SetActive(true);
        }
#endif
        OnAwakeEvnet.Invoke();

    }
    private void OnEnable()
    {
        SensorManager.instance.connectState += ConnectState;
    }
    
    private void OnDisable()
    {
        SensorManager.instance.connectState -= ConnectState;
    }

    private void ConnectState(SensorManager.States reciever)
    {
        Debug.Log($"ConnectState is {reciever} in Game_AppManager");

        // 끊어짐 > 연결끊김 알람 띄움
        if (SensorManager.States.None == reciever && !SensorManager.instance._connected)
        {
            SoundCtrl.Instance.ClickButton_Sound();
            Time.timeScale = 0;
            bleDisconnectAlert.SetActive(true);
        }
    }

    public void Start()
    {
        OnStartEvnet.Invoke();
    }

    public void GameStart()
    {
        StopCoroutine(_GameStart());
        StartCoroutine(_GameStart());
    }
    IEnumerator _GameStart()
    {
        //튜토리얼 들어가야함 
        Game_UIManagaer.instance.Tutorial_start();
        yield return new WaitUntil(() => Tutorial == true);
       
        int countDown = 0;
        Game_UIManagaer.instance.CountDownPopup_ob.SetActive(true);
        SoundManager.instance.PlaySFX_OneShot("GameStart");
        while (countDown < 4)
        {
            if (countDown >= 2)
            {
                if (GameManager.instance.CoreGame_Name_str.Equals("WireWalking"))
                {
                    Game_UIManagaer.instance.WireWalk_StartCamMotion();
                }
            }
            Game_UIManagaer.instance.CountDownPopup_ob.GetComponent<Image>().sprite = Game_UIManagaer.instance.CountNum_sp[countDown];
            countDown += 1;
            yield return new WaitForSeconds(1f);
        }
  
        Game_UIManagaer.instance.CountDownPopup_ob.SetActive(false);
        PlayStart = true;
        yield return new WaitUntil(() => PlayStart == true);
         Debug.Log("게임 시작! ");
        Game_UIManagaer.instance.Timer();

        if (GameManager.instance.CoreGame_Name_str.Equals("DeliveryMan"))
        {
            Debug.Log("인티케이터 활성화");
            indicator.SetActive(true);
            SensorManager.instance.resetRotation();

            Game_UIManagaer.instance.DeliveryManAni.SetTrigger("GamePlay");
            Game_UIManagaer.instance.DeliveryMan_Play();
        }
        else if (GameManager.instance.CoreGame_Name_str.Equals("TowerMaker"))
        {
            Game_UIManagaer.instance.TowerMaker_Play();
            Game_UIManagaer.instance.ClimbCounts_Slider();
        }
        else if (GameManager.instance.CoreGame_Name_str.Equals("WireWalking"))
        {
            Debug.Log("인티케이터 활성화");
            indicator.SetActive(true);
            SensorManager.instance.resetRotation();

            Game_UIManagaer.instance.WireWalking_start();
        }
        yield return null;
    }
}
