using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


[TrackBindingType(typeof(GameObject))]
[TrackClipType(typeof(NewPlayableAsset))]
public class NewPlayableTrack : TrackAsset
{
    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        //      Playable playable = base.CreatePlayable(graph, go, clip);
        /*
        AnimationCurve curve = new AnimationCurve();
        
        Keyframe kfA = new Keyframe((float)(clip.duration*0.5), 1);
        curve.AddKey(kfA);
        clip.CreateCurves("test");
        */
        var playable = ScriptPlayable<NewPlayableBehaviour>.Create(graph);
        var trackBinding = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as GameObject;
 
        playable.GetBehaviour().target_ = trackBinding;
        
        return playable;
    }
}