using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Schema;
using Random = System.Random;

namespace Openworld.Editor
{
    public class CreateCube : EditorWindow
    {
        public static GameObject[] ptable = new GameObject[3];

        public static Rect size = new Rect(-1000, 0, 2000, 1000);

        [MenuItem("Split/CreateCube")]
        public static void Init()
        {
            GetWindow<CreateCube>();

            ptable[0] = new GameObject();
            ptable[0].name = "F_";
            ptable[1] = new GameObject();
            ptable[1].name = "M_";
            ptable[2] = new GameObject();
            ptable[2].name = "N_";
        }


        public void OnGUI()
        {
            if (GUILayout.Button("Create!"))
            {
                for (var z = size.y; z < size.y+size.height; z+=50)
                {
                    for (var x = size.x; x < size.x+size.width; x+=50)
                    {
                        var r = UnityEngine.Random.Range(0, 3);
                        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.AddComponent<Rigidbody>();
                        cube.transform.position = new Vector3(x, 0.2f, z);
                        cube.transform.parent = ptable[r].transform;
                    }
                }
            }
        }
    }
}