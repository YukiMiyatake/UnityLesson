using System;
using System.Runtime.CompilerServices;
//using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Openworld.Editor
{

    public class SplitWorld : EditorWindow
    {
        enum LayerType
        {
            Terrain,
            Far,
            Middle,
            Near,
        }

        private static readonly int LAYER_LEN = Enum.GetNames(typeof(LayerType)).Length;

        struct LayerParam
        {
            public string name;
            public string prefix_in;
            public string prefix_out;
            public int xSize;
            public int zSize;

        }

        //        private static List<GameObject> terrainList = new List<GameObject>();
        //      private readonly string  terrainPrefix = "WSTerrain_";
        //        private static List<LayerParam> layerParam = new List<LayerParam>();
        private static LayerParam[] layerParam = new LayerParam[LAYER_LEN];

        // Must be power of two plus 1
        static int terrainHeightRes = 65;       // default: 513
        static int terrainDetailRes = 256;      // default: 1024
        static int terrainSplatRes = 128;       // default: 512


        [MenuItem("Split/Split Terrain")]
        public static void Init()
        {
            for (int i = 0; i < LAYER_LEN; i++)
            {
                layerParam[i].xSize = 100;
                layerParam[i].zSize = 100;
            }

            layerParam[(int) LayerType.Terrain].name = "Terrain";
            layerParam[(int)LayerType.Terrain].prefix_in = "T_";
            layerParam[(int)LayerType.Terrain].prefix_out = "OT_";

            layerParam[(int)LayerType.Far].name = "FarObject";
            layerParam[(int)LayerType.Far].prefix_in = "F_";
            layerParam[(int)LayerType.Far].prefix_out = "OF_";

            layerParam[(int)LayerType.Middle].name = "MiddleObject";
            layerParam[(int)LayerType.Middle].prefix_in = "M_";
            layerParam[(int)LayerType.Middle].prefix_out = "OM_";

            layerParam[(int)LayerType.Near].name = "NearObject";
            layerParam[(int)LayerType.Near].prefix_in = "N_";
            layerParam[(int)LayerType.Near].prefix_out = "ON_";

            GetWindow<SplitWorld>();

        }


        public void OnGUI()
        {

            for (int i = 0; i < LAYER_LEN; i++)
            {
                var param = layerParam[i];

                // Validate
                if (param.xSize < 10) param.xSize = 10;
                if (param.zSize < 10) param.zSize = 10;

                // Terrainは特殊
                if (i == (int) LayerType.Terrain)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Label(param.name);

                    GUILayout.Label("Input Prefix");
                    param.prefix_in = EditorGUILayout.TextField(param.prefix_in);
                    GUILayout.Label("Output Prefix");
                    param.prefix_in = EditorGUILayout.TextField(param.prefix_out);

                    param.xSize = EditorGUILayout.IntField("Split x Size", param.xSize);
                    param.zSize = EditorGUILayout.IntField("Split z Size", param.zSize);

                    terrainHeightRes = EditorGUILayout.IntField("New heightmap res", terrainHeightRes);
                    terrainDetailRes = EditorGUILayout.IntField("New detail res", terrainDetailRes);
                    terrainSplatRes = EditorGUILayout.IntField("New splat res", terrainSplatRes);
                    if (GUILayout.Button("Split  " + param.name))
                    {
                        SplitTerrain(i);
                        EditorUtility.ClearProgressBar();
                    }
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Label(param.name);

                    GUILayout.Label("Input Prefix");
                    param.prefix_in = EditorGUILayout.TextField(param.prefix_in);
                    GUILayout.Label("Output Prefix");
                    param.prefix_in = EditorGUILayout.TextField(param.prefix_out);

                    param.xSize = EditorGUILayout.IntField("Split x Size", param.xSize);
                    param.zSize = EditorGUILayout.IntField("Split z Size", param.zSize);

                    if (GUILayout.Button("Split  " + param.name))
                    {
                        SplitGameObject(i);
                        EditorUtility.ClearProgressBar();
                    }

                }

            }
            
        }



        private void SplitTerrain(int layer)
        {
            var param = layerParam[layer];
            foreach (GameObject go in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (go.name.StartsWith(param.prefix_in, StringComparison.CurrentCultureIgnoreCase))
                {
                    var tera = go.GetComponent<Terrain>();
                    if (tera)
                    {
                        for (var x = 0; x < tera.terrainData.size.x; x += param.xSize)
                        {
                            for (var z = 0; z < tera.terrainData.size.z; z += param.zSize)
                            {
                                copyTerrain(tera,
                                    string.Format("{0}{1}_{2}_0_{3}", param.prefix_out, go.name, x, z),
                                    x, x + param.xSize,
                                    z, z + param.zSize,
                                    terrainHeightRes, terrainDetailRes, terrainSplatRes);
                            }
                        }
                    }
                }
            }
        }

        private void SplitGameObject(int layer)
        {
            var param = layerParam[layer];
            foreach (GameObject go in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (go.name.StartsWith(param.prefix_in, StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (Transform c in go.transform)
                    {
                        int x = (int)c.transform.position.x / param.xSize;
                        int z = (int)c.transform.position.z / param.zSize;

                        var goName = string.Format("{0}{1}_{2}_0_{3}", param.prefix_out, go.name, x, z);

                        var goParent = GameObject.Find(goName);

                        if (!goParent)
                        {
                            goParent = new GameObject(goName);
                        }

                        var tmp = Instantiate(c);
                        tmp.parent = goParent.transform;
                    }
                }
            }
        }
        void copyTerrain(Terrain origTerrain, string newName, float xMin, float xMax, float zMin, float zMax,
            int heightmapResolution, int detailResolution, int alphamapResolution)
        {
            if (heightmapResolution < 33 || heightmapResolution > 4097)
            {
                Debug.Log("Invalid heightmapResolution " + heightmapResolution);
                return;
            }

            if (detailResolution < 0 || detailResolution > 4048)
            {
                Debug.Log("Invalid detailResolution " + detailResolution);
                return;
            }

            if (alphamapResolution < 16 || alphamapResolution > 2048)
            {
                Debug.Log("Invalid alphamapResolution " + alphamapResolution);
                return;
            }

            if (xMin < 0 || xMin > xMax || xMax > origTerrain.terrainData.size.x)
            {
                Debug.Log("Invalid xMin or xMax");
                return;
            }

            if (zMin < 0 || zMin > zMax || zMax > origTerrain.terrainData.size.z)
            {
                Debug.Log("Invalid zMin or zMax");
                return;
            }

            if (AssetDatabase.FindAssets(newName).Length != 0)
            {
                Debug.Log("Asset with name " + newName + " already exists");
                return;
            }

            TerrainData td = new TerrainData();
            GameObject gameObject = Terrain.CreateTerrainGameObject(td);
            Terrain newTerrain = gameObject.GetComponent<Terrain>();
            /*
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");
            // Must do this before Splat
            AssetDatabase.CreateAsset(td, "Assets/Resources/" + newName + ".asset");
            */
            // Copy over all vars
            newTerrain.bakeLightProbesForTrees = origTerrain.bakeLightProbesForTrees;
            newTerrain.basemapDistance = origTerrain.basemapDistance;
            newTerrain.castShadows = origTerrain.castShadows;
            newTerrain.collectDetailPatches = origTerrain.collectDetailPatches;
            newTerrain.detailObjectDensity = origTerrain.detailObjectDensity;
            newTerrain.detailObjectDistance = origTerrain.detailObjectDistance;
            newTerrain.drawHeightmap = origTerrain.drawHeightmap;
            newTerrain.drawTreesAndFoliage = origTerrain.drawTreesAndFoliage;
            newTerrain.editorRenderFlags = origTerrain.editorRenderFlags;
            newTerrain.heightmapMaximumLOD = origTerrain.heightmapMaximumLOD;
            newTerrain.heightmapPixelError = origTerrain.heightmapPixelError;
            newTerrain.legacyShininess = origTerrain.legacyShininess;
            newTerrain.legacySpecular = origTerrain.legacySpecular;
            newTerrain.lightmapIndex = origTerrain.lightmapIndex;
            newTerrain.lightmapScaleOffset = origTerrain.lightmapScaleOffset;
            newTerrain.materialTemplate = origTerrain.materialTemplate;
            newTerrain.materialType = origTerrain.materialType;
            newTerrain.realtimeLightmapIndex = origTerrain.realtimeLightmapIndex;
            newTerrain.realtimeLightmapScaleOffset = origTerrain.realtimeLightmapScaleOffset;
            newTerrain.reflectionProbeUsage = origTerrain.reflectionProbeUsage;
            newTerrain.treeBillboardDistance = origTerrain.treeBillboardDistance;
            newTerrain.treeCrossFadeLength = origTerrain.treeCrossFadeLength;
            newTerrain.treeDistance = origTerrain.treeDistance;
            newTerrain.treeMaximumFullLODCount = origTerrain.treeMaximumFullLODCount;

//            td.splatPrototypes = origTerrain.terrainData.splatPrototypes;
            td.treePrototypes = origTerrain.terrainData.treePrototypes;
            td.detailPrototypes = origTerrain.terrainData.detailPrototypes;

  //          td.terrainLayers = origTerrain.terrainData.terrainLayers;
            // Get percent of original
            float xMinNorm = xMin / origTerrain.terrainData.size.x;
            float xMaxNorm = xMax / origTerrain.terrainData.size.x;
            float zMinNorm = zMin / origTerrain.terrainData.size.z;
            float zMaxNorm = zMax / origTerrain.terrainData.size.z;
            float dimRatio1, dimRatio2;

            // Height
            td.heightmapResolution = heightmapResolution;
            float[,] newHeights = new float[heightmapResolution, heightmapResolution];
            dimRatio1 = (xMax - xMin) / (heightmapResolution - 1);
            dimRatio2 = (zMax - zMin) / (heightmapResolution - 1);
            for (int i = 0; i < heightmapResolution; i++)
            {
                for (int j = 0; j < heightmapResolution; j++)
                {
                    // Divide by size.y because height is stored as percentage
                    // Note this is [j, i] and not [i, j] (Why?!)
                    newHeights[j, i] =
                        origTerrain.SampleHeight(new Vector3(xMin + (i * dimRatio1), 0, zMin + (j * dimRatio2))) /
                        origTerrain.terrainData.size.y;
                }
            }

            td.SetHeightsDelayLOD(0, 0, newHeights);

            // Detail
            td.SetDetailResolution(detailResolution, 8); // Default? Haven't messed with resolutionPerPatch
            for (int layer = 0; layer < origTerrain.terrainData.detailPrototypes.Length; layer++)
            {
                int[,] detailLayer = origTerrain.terrainData.GetDetailLayer(
                    Mathf.FloorToInt(xMinNorm * origTerrain.terrainData.detailWidth),
                    Mathf.FloorToInt(zMinNorm * origTerrain.terrainData.detailHeight),
                    Mathf.FloorToInt((xMaxNorm - xMinNorm) * origTerrain.terrainData.detailWidth),
                    Mathf.FloorToInt((zMaxNorm - zMinNorm) * origTerrain.terrainData.detailHeight),
                    layer);
                int[,] newDetailLayer = new int[detailResolution, detailResolution];
                dimRatio1 = (float)detailLayer.GetLength(0) / detailResolution;
                dimRatio2 = (float)detailLayer.GetLength(1) / detailResolution;
                for (int i = 0; i < newDetailLayer.GetLength(0); i++)
                {
                    for (int j = 0; j < newDetailLayer.GetLength(1); j++)
                    {
                        newDetailLayer[i, j] =
                            detailLayer[Mathf.FloorToInt(i * dimRatio1), Mathf.FloorToInt(j * dimRatio2)];
                    }
                }

                td.SetDetailLayer(0, 0, layer, newDetailLayer);
            }



            // Splat
            td.alphamapResolution = alphamapResolution;
            float[,,] alphamaps = origTerrain.terrainData.GetAlphamaps(
                Mathf.FloorToInt(xMinNorm * origTerrain.terrainData.alphamapWidth),
                Mathf.FloorToInt(zMinNorm * origTerrain.terrainData.alphamapHeight),
                Mathf.FloorToInt((xMaxNorm - xMinNorm) * origTerrain.terrainData.alphamapWidth),
                Mathf.FloorToInt((zMaxNorm - zMinNorm) * origTerrain.terrainData.alphamapHeight));
            // Last dim is always origTerrain.terrainData.splatPrototypes.Length so don't ratio
            float[,,] newAlphamaps = new float[alphamapResolution, alphamapResolution, alphamaps.GetLength(2)];
            dimRatio1 = (float)alphamaps.GetLength(0) / alphamapResolution;
            dimRatio2 = (float)alphamaps.GetLength(1) / alphamapResolution;
            for (int i = 0; i < newAlphamaps.GetLength(0); i++)
            {
                for (int j = 0; j < newAlphamaps.GetLength(1); j++)
                {
                    for (int k = 0; k < newAlphamaps.GetLength(2); k++)
                    {
                        newAlphamaps[i, j, k] = alphamaps[Mathf.FloorToInt(i * dimRatio1),
                            Mathf.FloorToInt(j * dimRatio2), k];
                    }
                }
            }

            td.SetAlphamaps(0, 0, newAlphamaps);

            // Tree
            for (int i = 0; i < origTerrain.terrainData.treeInstanceCount; i++)
            {
                TreeInstance ti = origTerrain.terrainData.treeInstances[i];
                if (ti.position.x < xMinNorm || ti.position.x >= xMaxNorm)
                    continue;
                if (ti.position.z < zMinNorm || ti.position.z >= zMaxNorm)
                    continue;
                ti.position = new Vector3(((ti.position.x * origTerrain.terrainData.size.x) - xMin) / (xMax - xMin),
                    ti.position.y, ((ti.position.z * origTerrain.terrainData.size.z) - zMin) / (zMax - zMin));
                newTerrain.AddTreeInstance(ti);
            }

            gameObject.transform.position = new Vector3(origTerrain.transform.position.x + xMin,
                origTerrain.transform.position.y, origTerrain.transform.position.z + zMin);
            gameObject.name = newName;

            // Must happen after setting heightmapResolution
            td.size = new Vector3(xMax - xMin, origTerrain.terrainData.size.y, zMax - zMin);

            AssetDatabase.SaveAssets();
        }

        void stitchTerrain(GameObject center, GameObject left, GameObject top)
        {
            if (center == null)
                return;
            Terrain centerTerrain = center.GetComponent<Terrain>();
            float[,] centerHeights = centerTerrain.terrainData.GetHeights(0, 0,
                centerTerrain.terrainData.heightmapWidth, centerTerrain.terrainData.heightmapHeight);
            if (top != null)
            {
                Terrain topTerrain = top.GetComponent<Terrain>();
                float[,] topHeights = topTerrain.terrainData.GetHeights(0, 0, topTerrain.terrainData.heightmapWidth,
                    topTerrain.terrainData.heightmapHeight);
                if (topHeights.GetLength(0) != centerHeights.GetLength(0))
                {
                    Debug.Log("Terrain sizes must be equal");
                    return;
                }

                for (int i = 0; i < centerHeights.GetLength(1); i++)
                {
                    centerHeights[centerHeights.GetLength(0) - 1, i] = topHeights[0, i];
                }
            }

            if (left != null)
            {
                Terrain leftTerrain = left.GetComponent<Terrain>();
                float[,] leftHeights = leftTerrain.terrainData.GetHeights(0, 0, leftTerrain.terrainData.heightmapWidth,
                    leftTerrain.terrainData.heightmapHeight);
                if (leftHeights.GetLength(0) != centerHeights.GetLength(0))
                {
                    Debug.Log("Terrain sizes must be equal");
                    return;
                }

                for (int i = 0; i < centerHeights.GetLength(0); i++)
                {
                    centerHeights[i, 0] = leftHeights[i, leftHeights.GetLength(1) - 1];
                }
            }

            centerTerrain.terrainData.SetHeights(0, 0, centerHeights);
        }
    }
}