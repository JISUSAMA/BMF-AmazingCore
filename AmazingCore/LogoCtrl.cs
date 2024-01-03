using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LogoCtrl : MonoBehaviour
{
    public void Load_Main()
    {
        StartCoroutine(_Load_Main());
    }
    IEnumerator _Load_Main()
    {
      //  AppSoundManager.Instance.PlaySFX("LogoSound1");
        //yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("01.Login");
        yield return null;
    }
}
