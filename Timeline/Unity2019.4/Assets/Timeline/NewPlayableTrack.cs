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
    const float BLINK_MIN = 2f;
    const float BLINK_MAX = 5f;
    const float BLINK_TIME = 0.1f;
    const float BLINK_INTER = 0.05f;


    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        Playable playable = base.CreatePlayable(graph, go, clip);

//#if UNITY_EDITOR
#if FALSE
        clip.CreateCurves(null);
        var curve = new AnimationCurve();


        Debug.LogFormat( "clip start={0}  end={1} dulation={2}", clip.start, clip.end, clip.duration);
        float clip_duration = (float)clip.duration;
        curve.AddKey(new Keyframe(0f, 1f));
        for (float t = 0; t < clip_duration - BLINK_MIN; )
        {
            float r = Random.Range(BLINK_MIN, BLINK_MAX);
           if ( t + r + BLINK_TIME + BLINK_MIN/2 > clip_duration) break;            
            
            curve.AddKey(new Keyframe(t + r, 1f));
            curve.AddKey(new Keyframe(t + r + BLINK_INTER, 0f));
            curve.AddKey(new Keyframe(t + r + BLINK_TIME - BLINK_INTER, 0f));
            curve.AddKey(new Keyframe(t + r + BLINK_TIME, 1f));
            t += r + BLINK_TIME;
        }
        curve.AddKey(new Keyframe(clip_duration, 1f));

        /*
//GroupTrack作成
var grp = timelineAsset.CreateTrack<GroupTrack>(null, "Group");

// Groupトラックの子トラックとしてAnimationという名前で生成する
var _track = timelineAsset.CreateTrack<NewPlayableTrack>(grp, "NewPlayable");
var c = _track.CreateClip<NewPlayableAsset>();
c.curves.SetCurve("", typeof(NewPlayableAsset), "hoge", curve);
        */

        clip.curves.SetCurve("", typeof(NewPlayableAsset), "hoge", curve);

        EditorUtility.SetDirty(clip.parentTrack);
        TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
#endif


        var trackBinding = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as GameObject;
        var b = ((ScriptPlayable<NewPlayableBehaviour>)playable).GetBehaviour();
        b.binding_ = trackBinding;
        b.clip_ = clip;

        return playable;
    }
}