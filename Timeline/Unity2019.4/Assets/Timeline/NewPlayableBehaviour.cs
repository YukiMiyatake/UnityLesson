using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// A behaviour that is attached to a playable
[System.Serializable]
public class NewPlayableBehaviour : PlayableBehaviour
{
    [SerializeField]public float hoge;

    public GameObject binding_;
    [HideInInspector] public TimelineClip clip_;

    [SerializeField] public float move_ { set; get; }

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        Debug.Log("OnGraphStart" + hoge);
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        //Debug.Log("OnGraphStop");
    }


    public override void OnPlayableCreate(Playable playable)
    {
        Debug.Log("OnPlayableCreate" + hoge);
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        //Debug.Log("OnPlayableDestroy");
    }


    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        binding_.SetActive(false);
        //Debug.Log("OnBehaviourPlay");
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        binding_.SetActive(true);
        //Debug.Log("OnBehaviourPause");
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        // Debug.Log("PrepareFrame");
    }
}
