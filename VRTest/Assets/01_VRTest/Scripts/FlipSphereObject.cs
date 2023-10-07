using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSphereObject : MonoBehaviour
{
    // Flip 할 Sphere 오브젝트
    private GameObject flipObj = default;

    private void Awake()
    {
        flipObj = gameObject;
        FlipObject();
    }

    //! 오브젝트의 메쉬에서 폴리곤을 가져온다. 폴리곤의 vertex 를 뒤집어서 Mesh 를 Flip 하는 함수
    private void FlipObject() 
    {
        // { 메쉬 폴리곤의 법선의 역을 구하는 로직
        MeshFilter meshFilter = flipObj.GetComponent<MeshFilter>();
        Vector3[] normals = meshFilter.mesh.normals;
        Debug.LogFormat("메쉬 폴리곤의 개수 : {0}" , normals.Length);

        for(int i = 0; i < normals.Length; i++) 
        {
            normals[i] = -normals[i];
        }
        meshFilter.mesh.normals = normals;
        // } 메쉬 폴리곤의 법선의 역을 구하는 로직

        // { 폴리곤을 구성하는 삼각형의 세 점중에 가운데를 제외한 나머지 두 점을 Swap 하여 뒤집는 로직
        int[] triangles = meshFilter.mesh.triangles;
        int tempTriangle = default;
        Debug.LogFormat("삼각형의 개수 : {0}", triangles.Length);

        // 삼각형을 뒤집어야 하므로 3배수로 i 가 늘어나야함
        for (int i = 0; i < triangles.Length; i+=3) 
        {
            tempTriangle = triangles[i];
            triangles[i] = triangles[i + 2];
            triangles[i + 2] = tempTriangle;
        }
        meshFilter.mesh.triangles = triangles;
        // } 폴리곤을 구성하는 삼각형의 세 점중에 가운데를 제외한 나머지 두 점을 Swap 하여 뒤집는 로직
    }
}
