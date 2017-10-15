using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestMenu
{
    [MenuItem("Test/Menu %a")]
    public static void ShowMenu()
    {
        Debug.Log("ShowMenu");
    }


    [MenuItem("Test/AddObject %b", true)]
    static bool AddObjectValidate()
    {
        // GameObjectか？
        return Selection.activeObject.GetType() == typeof(GameObject);
    }

    [MenuItem("Test/AddObject %b", false)]
    static void AddObject(MenuCommand command)
    {
        GameObject objMenuCommand = new GameObject("MenuCommand");
        //　Undo登録
        Undo.RegisterCreatedObjectUndo(objMenuCommand, "Test_AddObject " + objMenuCommand.name);
        GameObjectUtility.SetParentAndAlign(objMenuCommand, command.context as GameObject);


        GameObject obj = new GameObject("NewGame");
        GameObjectUtility.SetParentAndAlign(obj, Selection.activeGameObject );

        //　Undo登録
        Undo.RegisterCreatedObjectUndo(obj, "Test_AddObject " + obj.name);
        Undo.AddComponent<Rigidbody>(obj);

    }

}
