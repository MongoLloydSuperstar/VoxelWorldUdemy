using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Block : MonoBehaviour
{
    private enum Cubeside
    {
        BOTTOM,
        TOP,
        LEFT,
        RIGHT,
        FRONT,
        BACK
    };

    public enum BlockType
    {
        GRASS,
        DIRT,
        STONE,
        AIR
    };


    private BlockType blockType;
    public bool isSolid;

    private Material cubeMaterial;
    private GameObject parent;
    private Vector3 position;

    
    private Vector2[,] blockUVs =
    {
        {   // GRASS TOP [0, 0-3]
            new Vector2(0.125f, 0.375f),   new Vector2(0.1875f, 0.375f),
            new Vector2(0.125f, 0.4375f),  new Vector2(0.1875f, 0.4375f)
        },
        {   // GRASS SIDE [1, 0-3]
            new Vector2(0.1875f, 0.9375f), new Vector2(0.25f, 0.9375f),
            new Vector2(0.1875f, 1.0f),    new Vector2(0.25f, 1.0f)
        },
        {   // DIRT [2, 0-3]
            new Vector2(0.125f, 0.9375f),  new Vector2(0.1875f, 0.9375f),
            new Vector2(0.125f, 1.0f),     new Vector2(0.1875f, 1.0f)
        },
        {   // STONE [3, 0-3]
            new Vector2(0, 0.875f),        new Vector2(0.0625f, 0.875f),
            new Vector2(0, 0.9375f),       new Vector2(0.0625f, 0.9375f)
        },
    };


    public Block(BlockType b, Vector3 pos, GameObject p, Material c)
    {
        blockType = b;
        parent = p;
        position = pos;
        cubeMaterial = c;
        isSolid = blockType != BlockType.AIR;
    }

    private void CreateQuad(Cubeside side)
    {
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh";

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];

        // All possible UVs
        Vector2 uv00 = new Vector2(0f, 0f);
        Vector2 uv10 = new Vector2(1f, 0f);
        Vector2 uv01 = new Vector2(0f, 1f);
        Vector2 uv11 = new Vector2(1f, 1f);

        if (blockType == BlockType.GRASS && side == Cubeside.TOP)
        {
            uv00 = blockUVs[0, 0];
            uv10 = blockUVs[0, 1];
            uv01 = blockUVs[0, 2];
            uv11 = blockUVs[0, 3];
        }
        else if (blockType == BlockType.GRASS && side == Cubeside.BOTTOM)
        {
            uv00 = blockUVs[(int)(BlockType.DIRT+1), 0];
            uv10 = blockUVs[(int)(BlockType.DIRT+1), 1];
            uv01 = blockUVs[(int)(BlockType.DIRT+1), 2];
            uv11 = blockUVs[(int)(BlockType.DIRT+1), 3];
        }
        else
        {
            uv00 = blockUVs[(int)(blockType + 1), 0];
            uv10 = blockUVs[(int)(blockType + 1), 1];
            uv01 = blockUVs[(int)(blockType + 1), 2];
            uv11 = blockUVs[(int)(blockType + 1), 3];
        }

        // All possible vertices
        Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);


        switch (side)
        {
            case Cubeside.BOTTOM:
                vertices = new[] { p0, p1, p2, p3 };
                normals = new[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                uvs = new[] { uv11, uv01, uv00, uv10 };
                triangles = new[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.TOP:
                vertices = new[] { p7, p6, p5, p4 };
                normals = new[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                uvs = new[] { uv11, uv01, uv00, uv10 };
                triangles = new[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.LEFT:
                vertices = new[] { p7, p4, p0, p3 };
                normals = new[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                uvs = new[] { uv11, uv01, uv00, uv10 };
                triangles = new[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.RIGHT:
                vertices = new[] { p5, p6, p2, p1 };
                normals = new[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                uvs = new[] { uv11, uv01, uv00, uv10 };
                triangles = new[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.FRONT:
                vertices = new[] { p4, p5, p1, p0 };
                normals = new[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                uvs = new[] { uv11, uv01, uv00, uv10 };
                triangles = new[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.BACK:
                vertices = new[] { p6, p7, p3, p2 };
                normals = new[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                uvs = new[] { uv11, uv01, uv00, uv10 };
                triangles = new[] { 3, 1, 0, 3, 2, 1 };
                break;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("Quad");
        quad.transform.position = position;
        quad.transform.parent = parent.transform;

        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;

        //MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        //renderer.material = cubeMaterial;
    }

    public bool HasSolidNeighbour(int x, int y, int z)
    {
        Block[,,] chunks = parent.GetComponent<Chunk>().chunkData;
        try
        {
            return chunks[x, y, z].isSolid;
        }
        catch (System.IndexOutOfRangeException e) { }

        return false;
    }

    public void Draw()
    {
        if (blockType == BlockType.AIR) return;

        if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z + 1))
            CreateQuad(Cubeside.FRONT);
        if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z - 1))
            CreateQuad(Cubeside.BACK);
        if (!HasSolidNeighbour((int)position.x, (int)position.y + 1, (int)position.z)) 
            CreateQuad(Cubeside.TOP);
        if (!HasSolidNeighbour((int)position.x, (int)position.y - 1, (int)position.z))
            CreateQuad(Cubeside.BOTTOM);
        if (!HasSolidNeighbour((int)position.x + 1, (int)position.y, (int)position.z))
            CreateQuad(Cubeside.RIGHT);
        if (!HasSolidNeighbour((int)position.x - 1, (int)position.y, (int)position.z))
            CreateQuad(Cubeside.LEFT);
    }

}
