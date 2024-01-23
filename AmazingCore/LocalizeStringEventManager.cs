using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
public class LocalizeStringEventManager : MonoBehaviour
{
    bool isChanging;
    [Serializable]
    public class LocalString
    {
        public LocalizeStringEvent Clear_LStringEvent;
        public LocalizedString Clear_LString;
    }
    public LocalString Lstring = new LocalString();
    private void OnEnable()
    {
      
    }
    private void OnDisable()
    {

    }
    
    public void ChangeLocal(int index)
    {
        if (isChanging)
            return;
        StartCoroutine(ChangeRoutine(index));
    }
    IEnumerator ChangeRoutine(int index)
    {
        isChanging = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        isChanging = false;
        SceneManager.LoadScene("00.Title");
        PlayerPrefs.SetInt("LocalNum", LocalizationSettings.SelectedLocale.SortOrder);
    }
    //clear 이벤트
    public void SetClearLevelStringEvnet()
    {
        //게임 레벨
        if (GameManager.instance.CoreGame_Stage_i == 1)
        {
            Lstring.Clear_LString.TableReference = "Game_Global_StringTable";
            Lstring.Clear_LString.TableEntryReference = "Game_Clear1_str";
        }
        else if (GameManager.instance.CoreGame_Stage_i == 2)
        {
            Lstring.Clear_LString.TableReference = "Game_Global_StringTable";
            Lstring.Clear_LString.TableEntryReference = "Game_Clear2_str";

        }
        else if (GameManager.instance.CoreGame_Stage_i == 3)
        {
            Lstring.Clear_LString.TableReference = "Game_Global_StringTable";
            Lstring.Clear_LString.TableEntryReference = "Game_Clear3_str";
        }
    }
}
