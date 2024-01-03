using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnimationCtrl : MonoBehaviour
{
    /////////////////// 딜리버리맨 사운드 //////////////////////
    public void DeliveryMan_Bicycle_sound()
    {
        SoundManager.instance.PlaySFX_OneShot("bicycle-bell-ring");
    }
    /////////////////// 와이어워크 사운드 //////////////////////
    public void WireWalking_shaking()
    {
        SoundManager.instance.PlaySFX_OneShot("moving");
    }

}
