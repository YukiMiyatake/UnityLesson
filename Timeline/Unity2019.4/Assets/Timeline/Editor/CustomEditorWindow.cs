using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEditor;
using UnityEditor.Timeline;

public class CustomEditorWindow : EditorWindow
{

    [MenuItem("GameObject/CustomEditor", false, 1)]
    private static void Create()
    {
        // 生成
        GetWindow<CustomEditorWindow>("CustomEditor");

    }
    

    private void OnGUI()
    {
        using (new GUILayout.HorizontalScope())
        {
            var targetGo = Selection.activeGameObject;

            if (GUILayout.Button("CreateTimeline"))
            {
                if(targetGo is null)
                {
                    return;
                }
                // 選択中のGameObjectにPlayableDirectorコンポーネントをつける
                var director = targetGo.GetComponent<PlayableDirector>();
                if(director == null)
                {
                    director = targetGo.AddComponent<PlayableDirector>();
                }

                // TimelineAssetの作成
                var timelineAsset = director.playableAsset as TimelineAsset;
                if (timelineAsset == null)
                {
                    timelineAsset = ScriptableObject.CreateInstance<TimelineAsset>();
                    director.playableAsset = timelineAsset;
                }


                var path = "Assets/Resources/sample.playable";
                AssetDatabase.DeleteAsset(path); 
                AssetDatabase.CreateAsset(timelineAsset, path);
                AssetDatabase.SaveAssets();



                var groupTrack = timelineAsset.CreateTrack<GroupTrack>(null, "Group");
                var track = timelineAsset.CreateTrack<NewPlayableTrack>(groupTrack, "NewPlayable");

                var b = GameObject.Find("Cube");
                director.SetGenericBinding(track, b);

                var clip = track.CreateClip<NewPlayableAsset>();

                clip.CreateCurves(null);
                var curve = new AnimationCurve();

                curve.AddKey(new Keyframe(0f, 1f));
                curve.AddKey(new Keyframe((float)clip.duration, 0f));
                clip.curves.SetCurve("", typeof(NewPlayableAsset), "hoge", curve);


                EditorUtility.SetDirty(timelineAsset);
                TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
            }
        }
    }

}
