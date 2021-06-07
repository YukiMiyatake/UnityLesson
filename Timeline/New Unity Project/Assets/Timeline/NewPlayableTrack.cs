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
        Playable playable = base.CreatePlayable(graph, go, clip);
 

        //var trackBinding = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as GameObject;
        //Debug.Log("Track::" + trackBinding);

        //var p = (ScriptPlayable<NewPlayableBehaviour>)playable;
        //p.GetBehaviour().binding_ = trackBinding;

        return playable;
    }
}