using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Timeline;
#endif

[TrackBindingType(typeof(GameObject))]
[TrackClipType(typeof(NewPlayableAsset))]
public class NewPlayableTrack : TrackAsset
{
    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        Playable playable = base.CreatePlayable(graph, go, clip);

#if UNITY_EDITOR
        clip.CreateCurves(null);
        var curve = new AnimationCurve();
        curve.AddKey(new Keyframe(0f, 0.3f));
        clip.curves.SetCurve("", typeof(NewPlayableAsset), "hoge", curve);

        EditorUtility.SetDirty(clip.parentTrack);
        TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
#endif


        var trackBinding = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as GameObject;
        var p = (ScriptPlayable<NewPlayableBehaviour>)playable;
        p.GetBehaviour().binding_ = trackBinding;

        return playable;
    }
}