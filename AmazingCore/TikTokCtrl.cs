using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TikTokCtrl : MonoBehaviour
{
    public Vector3 ThisOb_vec;
    public bool moveCam = false;
    public float move_f;
    private void Awake()
    {
        move_f = -263;
        ThisOb_vec = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        Move_To_Pos();
    }
    //2022.11.08
    public void Move_To_Pos()
    {
        StartCoroutine(_Move_To_Pos());
    }
    IEnumerator _Move_To_Pos()
    {
      //  Debug.Log("들어오나????");
        move_f = -263 + (Game_UIManagaer.instance.ClimbCounts.Count * 209);
        Game_UIManagaer.instance.PosVector = new Vector3(Game_UIManagaer.instance.cam.gameObject.transform.position.x, move_f, Game_UIManagaer.instance.cam.gameObject.transform.position.z);
      //  Debug.Log("   Game_UIManagaer.instance.PosVector.y" + Game_UIManagaer.instance.PosVector.y);
       // Debug.Log("move_f" + move_f);
        Game_UIManagaer.instance.MoveFollow_CamMotion();
        if (Game_UIManagaer.instance.ClimbCounts.Count != 1)
        {
          //  Debug.Log("여기도??????????");
            while (this.gameObject.transform.position.y >= move_f)
            {
                ThisOb_vec.y -= 30f;
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, ThisOb_vec.y, this.gameObject.transform.position.z);
                if (this.gameObject.transform.position.y <= move_f)
                {
                    this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, move_f, this.gameObject.transform.position.z);
                }
                yield return new WaitForFixedUpdate();
            }
          //  Debug.Log("   Game_UIManagaer.instance.PosVector.y" + Game_UIManagaer.instance.PosVector.y);
        }
        else
        {
          //  Debug.Log("여기도??????????");
            while (this.gameObject.transform.position.y >= move_f)
            {
                ThisOb_vec.y -= 30f;
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, ThisOb_vec.y, this.gameObject.transform.position.z);
                if (this.gameObject.transform.position.y <= move_f)
                {
                    this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, move_f, this.gameObject.transform.position.z);
                }
                yield return new WaitForFixedUpdate();
            }
           // Debug.Log("   Game_UIManagaer.instance.PosVector.y" + Game_UIManagaer.instance.PosVector.y);
        }

    }

}
