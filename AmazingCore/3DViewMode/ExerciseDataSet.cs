using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Exercise Data" , menuName = "ExerciseScriptable/ExcerciseData")]
public class ExerciseDataSet : ScriptableObject
{
    public enum OnState
    {
        PLAYING,
        PAUSE,
        STOP
    }

    public string name;
    public OnState onState;
    public List<GameObject> objPerFrame;
    //public TouchPhase touchPhase;
}
