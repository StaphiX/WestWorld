using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TileMapFloor : MonoBehaviour {

    private Mesh mesh;
    public int xSize, ySize;

    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = Camera.main.aspect * cameraHeight;
        Vector2 cameraSize = new Vector2(cameraWidth, cameraHeight);

        Vector2 scale = transform.localScale;

        float fScreenWidth = cameraWidth * scale.x;
        float fScreenHeight = cameraHeight * scale.y;

        int iTileWidth = (int)(fScreenWidth / xSize);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        float fXStart = -fScreenWidth / 2;
        float fYStart = -fScreenHeight / 4;
        Vector3[] vVertices = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                float fXPos = fXStart + iTileWidth * x;
                float fYPos = fYStart + iTileWidth * y;
                vVertices[i] = new Vector3(fXPos, fYPos);
            }
        }
        mesh.vertices = vVertices;

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
