using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Image GamePostImge;
    [SerializeField] Sprite[] GamePostSprite;
    [SerializeField] Image progressBar;
    [SerializeField] Sprite[] loadingBarSprite;
    [SerializeField] GameObject bleDisconnectAlert;

    //0: DeliveryMan 1: TowerMaker 2: WireWalk
    private void Awake()
    {
        if (GameManager.instance.CoreGame_Name_str.Equals("DeliveryMan")) { GamePostImge.sprite = GamePostSprite[0]; progressBar.sprite = loadingBarSprite[0]; }
        else if (GameManager.instance.CoreGame_Name_str.Equals("TowerMaker")) { GamePostImge.sprite = GamePostSprite[1]; progressBar.sprite = loadingBarSprite[1]; }
        else if (GameManager.instance.CoreGame_Name_str.Equals("WireWalking")) { GamePostImge.sprite = GamePostSprite[2]; progressBar.sprite = loadingBarSprite[2]; }
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
        Debug.Log($"ConnectState is {reciever} in Training_AppManager");

        // ²÷¾îÁü > ¿¬°á²÷±è ¾Ë¶÷ ¶ç¿ò
        if (SensorManager.States.None == reciever && !SensorManager.instance._connected)
        {
            SoundCtrl.Instance.ClickButton_Sound();
            Time.timeScale = 0;
            bleDisconnectAlert.SetActive(true);
        }
    }
    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("06.Loading");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime * 0.001f;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount >= 0.99f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    public void GotoLobby()
    {
        SceneManager.LoadScene("02.ChooseMode");
    }
}
