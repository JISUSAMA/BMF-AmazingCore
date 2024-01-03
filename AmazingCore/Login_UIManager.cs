
using UnityEngine;
using UnityEngine.UI;
using System;

public class Login_UIManager : MonoBehaviour
{
 
    public GameObject Defalt_ob;

    public InputField Name_inf, Age_Inf, Height_Inf, Weight_Inf;
    public Text Name_text, Age_text, Height_text, Weight_text;

    public GameObject Login_ob;

    public GameObject System_Exit_popup;
    private void Awake()
    {
        //로그인했음
        if (PlayerPrefs.HasKey("Login"))
        {
            Login_ob.SetActive(true);
        }
        else
        {
            Defalt_ob.SetActive(true);
        }
    }
    public void OnClick_Login_Btn()
    {
        SoundCtrl.Instance.ClickButton_Sound();
        if (Name_inf.text.Length > 0 && Age_Inf.text.Length > 0
            && Height_Inf.text.Length > 0 && Weight_Inf.text.Length > 0)
        {
            PlayerPrefs.SetString("UserName", Name_inf.text);
            PlayerPrefs.SetString("UserAge", Age_Inf.text);
            PlayerPrefs.SetString("UserHeight", Height_Inf.text);
            PlayerPrefs.SetString("UserWeight", Weight_Inf.text);
            PlayerPrefs.SetString("Login", "true");

            //운동 시작일 저장
            string startDay = DateTime.Now.ToString("yy.MM.dd");
            PlayerPrefs.SetString("StartExerciseDay", startDay);
            PlayerPrefs.SetString("LastExerciseDay", startDay);
   
            GameManager.instance.UserName = PlayerPrefs.GetString("UserName");
            GameManager.instance.UserAge = PlayerPrefs.GetString("UserAge");
            GameManager.instance.UserHeight = PlayerPrefs.GetString("UserHeight");
            GameManager.instance.UserWeight = PlayerPrefs.GetString("UserWeight");

            GameManager.instance.LoadSceneName("02.ChooseMode");
        }
        else
        {
            Debug.Log("로그인 실패 !");
        }
    }
    public void OnClick_Exit_Btn()
    {
        System_Exit_popup.SetActive(true);
        SoundCtrl.Instance.ClickButton_Sound();
    }
    public void OnClick_StartBtn()
    {
        SoundCtrl.Instance.ClickButton_Sound();
        GameManager.instance.LoadSceneName("02.ChooseMode");
    }
}
