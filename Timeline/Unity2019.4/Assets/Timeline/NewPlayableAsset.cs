using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class NewPlayableAsset : PlayableAsset
{
    public NewPlayableBehaviour template_;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<NewPlayableBehaviour>.Create(graph, template_);

       


        return playable;
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(NewPlayableAsset))]
    public class NewPlayableAssetEditor: Editor
    {
        private NewPlayableAsset owner_ = null;

        private void OnEnable()
        {
            owner_ = target as NewPlayableAsset;
        }
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            float v = owner_.template_.hoge;
            v = EditorGUILayout.Slider(v, 0f, 1f);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "NewPlayableAsset");
                owner_.template_.hoge = v;
                EditorUtility.SetDirty(owner_);
            }



        }

    }
#endif
    
}
