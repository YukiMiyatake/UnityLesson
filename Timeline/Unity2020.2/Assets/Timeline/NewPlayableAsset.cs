using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEditor;


[System.Serializable]
public class NewPlayableAsset : PlayableAsset
{
    public NewPlayableBehaviour template_ = null;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        // Debug.Log("Asset::CreatePlayable");
        if (template_ is null) {
            template_ = new NewPlayableBehaviour();
        }
        var playable = ScriptPlayable<NewPlayableBehaviour>.Create(graph, template_);
       // template_ = playable.GetBehaviour();



        /*
        var director = go.GetComponent<PlayableDirector>();
        
        
       // director.
        var trackBinding = go.GetComponent<PlayableDirector>().GetGenericBinding(director.playableAsset) as GameObject;
        Debug.Log( "Asset::" + trackBinding);
        
        playable.GetBehaviour().target_ = trackBinding;
        */
        //playable.GetBehaviour().hoge = 199;
        //template_.hoge = 0.22f;

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
            base.OnInspectorGUI();
              var owner = target as NewPlayableAsset;
            Debug.Log(owner);
            Debug.Log(owner.template_);

            //owner.template_.move_ = EditorGUILayout.Slider(owner.template_.move_, 0f, 1f);

            // EditorGUI.BeginChangeCheck();
            float v = 0.1f;
//            float v = owner.template_.hoge;
            v = EditorGUILayout.Slider(v, 0f, 1f);
         //   if (EditorGUI.EndChangeCheck())
            {
             //   Undo.RecordObject(target, "NewPlayableAsset");
                //owner.template_.hoge = v;
              //  Debug.Log(owner.template_.hoge);
              //  EditorUtility.SetDirty(owner);
            }



        }

    }
#endif
    
}
