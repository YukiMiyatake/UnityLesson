using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEditor;

[System.Serializable]
public class NewPlayableAsset : PlayableAsset
{
    public NewPlayableBehaviour behaviour_;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        Playable playable = ScriptPlayable<NewPlayableBehaviour>.Create(graph, behaviour_);
        return playable;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(NewPlayableAsset))]
    public class NewPlayableAssetEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            float move = 0f;
            EditorGUILayout.Slider(move, 0f, 1f);
        }

    }
#endif

}
