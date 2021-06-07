using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class NewPlayableBehaviour : PlayableBehaviour
{
    public GameObject target_;

    [SerializeField] public float move_;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        Debug.Log("OnGraphStart");
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        Debug.Log("OnGraphStop");
    }


    public override void OnPlayableCreate(Playable playable)
    {
        Debug.Log("OnPlayableCreate");
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        Debug.Log("OnPlayableDestroy");
    }


    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        target_.SetActive(false);
        Debug.Log("OnBehaviourPlay");
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        target_.SetActive(true);
        Debug.Log("OnBehaviourPause");
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        // Debug.Log("PrepareFrame");
    }
}
