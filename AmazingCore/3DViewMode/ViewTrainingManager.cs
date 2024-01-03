using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EnumTypes;
using UnityEngine.UI;

using UnityEngine.Localization;
using UnityEngine.Localization.Components;


public class ViewTrainingManager : MonoBehaviour
{
    public LocalizeSpriteEvent LTrainingTitle;
    public Sprite[] sprites;
    public Image trainigName; 
    // Start is called before the first frame update
    void Start()
    {        

        Initialize();
    }
    ///
    private void Initialize()
    {
        Debug.Log("Current Training : " + GameManager.instance.viewModeList);
        LTrainingTitle.AssetReference.TableReference = "BasicAssetTable";
        if ((int)GameManager.instance.viewModeList == 0)
        {
            LTrainingTitle.AssetReference.TableEntryReference = "Plank_Title_Label";
        }
        else if ((int)GameManager.instance.viewModeList == 1)
        {

            LTrainingTitle.AssetReference.TableEntryReference = "LegPull_Title_Label";
        }
        else if ((int)GameManager.instance.viewModeList == 2)
        {
            LTrainingTitle.AssetReference.TableEntryReference = "MountainClimer_Title_Label";
        }
        else if ((int)GameManager.instance.viewModeList == 3)
        {
            LTrainingTitle.AssetReference.TableEntryReference = "BirdDog_Title_Label";
        }
        else if ((int)GameManager.instance.viewModeList == 4)
        {
            LTrainingTitle.AssetReference.TableEntryReference = "Bridge_Title_Label";
        }
        else if ((int)GameManager.instance.viewModeList == 5)
        {
            LTrainingTitle.AssetReference.TableEntryReference = "SidePlank_Title_Label";
        }
        else if ((int)GameManager.instance.viewModeList == 6)
        {
            LTrainingTitle.AssetReference.TableEntryReference = "z-up_Title_Label";
        }
     //   trainigName.sprite = sprites[(int)GameManager.instance.viewModeList];
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("02.ChooseMode");
    }
}
