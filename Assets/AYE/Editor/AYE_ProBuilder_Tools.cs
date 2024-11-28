// 需要ProBuilder

/*
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEditor.ProBuilder;
using System.Collections.ObjectModel;

public class AYE_ProBuilder_Tools : MonoBehaviour
{
    // 把選到的ProBuilderMesh裡面複選的東西設定為整數
    [MenuItem("AYETools/ProBuilder/SelectedTo0")]
    static void ProBuilder_SelectedTo0()
    {
        // 選重的對象
        ProBuilderMesh quad = Selection.gameObjects[0].GetComponent<ProBuilderMesh>();

        // 找出所有點
        Vertex[] vertices = quad.GetVertices();

        // 找出所有選重的點
        ReadOnlyCollection<int> svs = quad.selectedVertices;

        // 記錄所有要改動的點
        List<int> ogPoint = new List<int>();

        // 捕捉原始要改動的點
        for (int i = 0; i < quad.vertexCount; i++)
        {
            if (svs.Contains(i))
                ogPoint.Add(i);
        }

        List<int> temp = new List<int>();
        // 捕捉所有靠近原始點的點
        for (int i = 0; i < quad.vertexCount; i++)
        {
            for (int j = 0; j < ogPoint.Count; j++)
            {
                if (Vector3.Distance(vertices[i].position, vertices[ogPoint[j]].position) < 0.01f)
                    temp.Add(i);
            }
        }

        // 合併原始點和後來找到要改動的點
        ogPoint.AddRange(temp);

        // 進行改動
        for (int i = 0; i < ogPoint.Count; i++)
        {   
            Vector3 n = vertices[ogPoint[i]].position;

            n.x = (float)System.Math.Round(n.x);
            n.y = (float)System.Math.Round(n.y);
            n.z = (float)System.Math.Round(n.z);

            vertices[ogPoint[i]].position = n;
        }

        // Rebuild the triangle and submesh arrays, and apply vertex positions & submeshes to `MeshFilter.sharedMesh`
        quad.SetVertices(vertices);

        // If in editor, generate UV2 and collapse duplicate vertices with
        EditorMeshUtility.Optimize(quad, true);

        quad.ToMesh();

        // Recalculate UVs, Normals, Tangents, Collisions, then apply to Unity Mesh.
        quad.Refresh();
    }
    // 把選到的ProBuilderMesh設定為整數
    [MenuItem("AYETools/ProBuilder/To0")]
    static void ProBuilder_To0()
    {
        // 選重的對象
        ProBuilderMesh quad = Selection.gameObjects[0].GetComponent<ProBuilderMesh>();

        // 找出所有點
        Vertex[] vertices = quad.GetVertices();

        // 找出所有選重的點
        ReadOnlyCollection<int> svs = quad.selectedVertices;

        // 進行改動
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 n = vertices[i].position;

            n.x = (float)System.Math.Round(n.x);
            n.y = (float)System.Math.Round(n.y);
            n.z = (float)System.Math.Round(n.z);

            vertices[i].position = n;
        }

        // Rebuild the triangle and submesh arrays, and apply vertex positions & submeshes to `MeshFilter.sharedMesh`
        quad.SetVertices(vertices);

        // If in editor, generate UV2 and collapse duplicate vertices with
        EditorMeshUtility.Optimize(quad, true);

        quad.ToMesh();

        // Recalculate UVs, Normals, Tangents, Collisions, then apply to Unity Mesh.
        quad.Refresh();
    }
}
*/
