using UnityEngine;
using Google.Play.Review;
using Cysharp.Threading.Tasks;
using Google.Play.Common;
using Google.Play.AppUpdate;
using System;
using System.Collections;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Service URL
/// https://developer.android.com/guide/playcore/in-app-updates/unity?hl=ko
/// </summary>

public class GooglePlayManager : MonoBehaviour
{
    public static GooglePlayManager Instance { get; private set; }

    ReviewManager _reviewManager = null;
    AppUpdateManager appUpdateManager = null;

    private const string REVIEW_URL = "https://play.google.com/store/apps/details?id=com.gateways.amazingcore";

    public TMP_Text updateStatusLog;
    public Slider downloadProgressbar;

    public void Awake()
    {
        Instance = this;

        if (Instance != null)
        {
            Debug.Log("GooglePlayManager Instance is not null");
        }
    }
    public void OnDestroy()
    {
        Instance = null;

        Debug.Log($"OnDestroy");
    }

    /// <summary>
    /// 인앱 리뷰 호출 함수.
    /// </summary>
    public void LaunchReview()
    {        
        try
        {       
            _reviewManager = new ReviewManager();
            var playReviewInfoAsyncOperation = _reviewManager.RequestReviewFlow();

            playReviewInfoAsyncOperation.Completed += playReviewInfoAsync =>
            {
                if (playReviewInfoAsync.Error == ReviewErrorCode.NoError)
                {
                    var playReviewInfo = playReviewInfoAsync.GetResult();
                    if (playReviewInfo == null)
                    {
                        Debug.Log("강제이동 1");
                        //null일 경우 브라우저로 리뷰페이지로 강제 이동.
                        OpenUrl();
                    }
                    else
                    {
                        Debug.Log("인앱리뷰");
                        //인앱리뷰 실행.
                        _reviewManager.LaunchReviewFlow(playReviewInfo);
                    }
                }
                else
                {
                    Debug.Log("강제이동 2");
                    OpenUrl();
                }
            };
        }
        catch (Exception e)
        {
            OpenUrl();
            Debug.Log("Exception LaunchReview >>>> " + e.Message);
        }
    }

    ///// <summary>
    ///// 인앱 업데이트 호출 함수.
    ///// </summary>
    ///// <returns></returns>
    public async UniTask<int> UpdateApp()
    {
        Debug.Log(">>>> UpdateApp");

        try
        {
            appUpdateManager = new AppUpdateManager();  // Create AppUpdateManager 

            // get appupdate info
            PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation = appUpdateManager.GetAppUpdateInfo();
            await appUpdateInfoOperation;   // getupdateinfo start
            
            if (appUpdateInfoOperation.IsSuccessful)
            {
                var appUpdateInfoResult = appUpdateInfoOperation.GetResult();

                if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
                {
                    updateStatusLog.text = "Update Available.";

                    var appUpdateOptions = AppUpdateOptions.FlexibleAppUpdateOptions();

                    var startUpdateRequest = appUpdateManager.StartUpdate(appUpdateInfoResult, appUpdateOptions);

                    while (!startUpdateRequest.IsDone)
                    {
                        await UniTask.Yield();
                        Debug.Log("startUpdateRequest.Status : " + startUpdateRequest.Status);
                        Debug.Log("DownloadProgress : " + startUpdateRequest.DownloadProgress);
                        Debug.Log(appUpdateInfoResult.ToString());                            
                        
                        if (startUpdateRequest.Status == AppUpdateStatus.Pending)
                        {
                            downloadProgressbar.gameObject.SetActive(true);
                        }
                        else if (startUpdateRequest.Status == AppUpdateStatus.Downloading)
                        {
                            updateStatusLog.text = $"Downloading ... {Mathf.Floor(startUpdateRequest.DownloadProgress * 100) }%";
                            downloadProgressbar.value = startUpdateRequest.DownloadProgress;
                        }
                        else if (startUpdateRequest.Status == AppUpdateStatus.Downloaded)
                        {
                            downloadProgressbar.gameObject.SetActive(false);
                        }
                    }

                    var result = appUpdateManager.CompleteUpdate();

                    while (!result.IsDone)
                    {
                        await UniTask.Yield();                        
                        Debug.Log("CompleteUpdate.Status 2: " + startUpdateRequest.Status);                        
                        updateStatusLog.text = $"{startUpdateRequest.Status}.";
                    }

                    updateStatusLog.text = $"{startUpdateRequest.Status}";

                    return (int)startUpdateRequest.Status;  // 0 ~ 6
                }
                else if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.DeveloperTriggeredUpdateInProgress)
                {
                    updateStatusLog.text = "Update In Progress..";

                    downloadProgressbar.gameObject.SetActive(true);

                    var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();

                    var startUpdateRequest = appUpdateManager.StartUpdate(appUpdateInfoResult, appUpdateOptions);

                    while (!startUpdateRequest.IsDone)
                    {
                        await UniTask.Yield();
                        Debug.Log(appUpdateInfoResult.ToString());
                        
                        if (startUpdateRequest.Status == AppUpdateStatus.Downloading)
                        {
                            updateStatusLog.text = $"Downloading ... {Mathf.Floor(startUpdateRequest.DownloadProgress * 100) }%";
                            downloadProgressbar.value = startUpdateRequest.DownloadProgress;
                        }
                        else if (startUpdateRequest.Status == AppUpdateStatus.Downloaded)
                        {
                            downloadProgressbar.gameObject.SetActive(false);
                        }
                    }

                    var result = appUpdateManager.CompleteUpdate();

                    while (!result.IsDone)
                    {
                        await UniTask.Yield();
                        Debug.Log("CompleteUpdate.Status 2: " + startUpdateRequest.Status);
                        updateStatusLog.text = $"{startUpdateRequest.Status}.";
                    }

                    updateStatusLog.text = $"{startUpdateRequest.Status}";

                    return (int)startUpdateRequest.Status;  // 0 ~ 6

                    //return (int)UpdateAvailability.DeveloperTriggeredUpdateInProgress;  // 3
                }
                else if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateNotAvailable)
                {
                    Debug.Log("Update Not Available.");
                    updateStatusLog.text = "Update Not Available.";
                    return (int)UpdateAvailability.UpdateNotAvailable;  // 1
                }
                else
                {
                    Debug.Log("UpdateAvailability Unknown.");
                    updateStatusLog.text = "UpdateAvailability Unknown.";
                    return (int)UpdateAvailability.Unknown; // 0
                }
            }
            else
            {
                updateStatusLog.text = $"{appUpdateInfoOperation.Error}";
                return -1;
            }
        }
        catch (Exception e) 
        { 
            Debug.Log(e.Message);
            updateStatusLog.text = $"{e.Message}";
            return -2; 
        }
    }

    public string AppUpdateStatusString(AppUpdateStatus appUpdateStatus)
    {
        switch (appUpdateStatus)
        {
            case Google.Play.AppUpdate.AppUpdateStatus.Unknown:
                break;
            case Google.Play.AppUpdate.AppUpdateStatus.Pending:
                break;
            case Google.Play.AppUpdate.AppUpdateStatus.Downloading:
                break;
            case Google.Play.AppUpdate.AppUpdateStatus.Downloaded:
                break;
            case Google.Play.AppUpdate.AppUpdateStatus.Installing:
                break;
            case Google.Play.AppUpdate.AppUpdateStatus.Installed:
                break;
            case Google.Play.AppUpdate.AppUpdateStatus.Failed:
                break;
            case Google.Play.AppUpdate.AppUpdateStatus.Canceled:
                break;
            default:
                break;
        }

        return "";
    }

    public string AppUpdateErrorCodeString(AppUpdateErrorCode appUpdateErrorCode)
    {
        string result = "";

        switch (appUpdateErrorCode)
        {
            case Google.Play.AppUpdate.AppUpdateErrorCode.NoError:
                result = "AppUpdateErrorCode.NoError";
                break;
            case Google.Play.AppUpdate.AppUpdateErrorCode.NoErrorPartiallyAllowed:
                result = "AppUpdateErrorCode.NoErrorPartiallyAllowed";
                break;
            case Google.Play.AppUpdate.AppUpdateErrorCode.ErrorUnknown:
                result = "AppUpdateErrorCode.ErrorUnknown";
                break;
            case Google.Play.AppUpdate.AppUpdateErrorCode.ErrorApiNotAvailable:
                result = "AppUpdateErrorCode.ErrorApiNotAvailable";
                break;
            case Google.Play.AppUpdate.AppUpdateErrorCode.ErrorInvalidRequest:
                result = "AppUpdateErrorCode.ErrorInvalidRequest";
                break;
            case Google.Play.AppUpdate.AppUpdateErrorCode.ErrorUpdateUnavailable:
                result = "AppUpdateErrorCode.ErrorUpdateUnavailable";
                break;
            case Google.Play.AppUpdate.AppUpdateErrorCode.ErrorUpdateNotAllowed:
                result = "AppUpdateErrorCode.ErrorUpdateNotAllowed";
                break;
            case Google.Play.AppUpdate.AppUpdateErrorCode.ErrorDownloadNotPresent:
                result = "AppUpdateErrorCode.ErrorDownloadNotPresent";
                break;
            case Google.Play.AppUpdate.AppUpdateErrorCode.ErrorUpdateInProgress:
                result = "AppUpdateErrorCode.ErrorUpdateInProgress";
                break;
            case Google.Play.AppUpdate.AppUpdateErrorCode.ErrorInternalError:
                result = "AppUpdateErrorCode.ErrorInternalError";
                break;
            case Google.Play.AppUpdate.AppUpdateErrorCode.ErrorUserCanceled:
                result = "AppUpdateErrorCode.ErrorUserCanceled";
                break;
            case Google.Play.AppUpdate.AppUpdateErrorCode.ErrorUpdateFailed:
                result = "AppUpdateErrorCode.ErrorUpdateFailed";
                break;
            case Google.Play.AppUpdate.AppUpdateErrorCode.ErrorPlayStoreNotFound:
                result = "AppUpdateErrorCode.ErrorPlayStoreNotFound";
                break;
            case Google.Play.AppUpdate.AppUpdateErrorCode.ErrorAppNotOwned:
                result = "AppUpdateErrorCode.ErrorAppNotOwned";
                break;
            default:
                break;
        }

        return result;
    }

    public void OpenUrl()
    {
        Debug.Log(">>>> OpenUrl");
        Application.OpenURL(REVIEW_URL);
    }

}