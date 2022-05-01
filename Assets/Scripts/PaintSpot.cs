using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSpot : MonoBehaviour
{
    Texture2D textureMask;
    public Transform cube;
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        Texture2D mainTexture = mesh.material.GetTexture("_MainMaterial") as Texture2D;
        textureMask = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, true);
        int pixelCount = textureMask.width * textureMask.height;
        Color[] cols = new Color[pixelCount];
        for (int x = 0; x < textureMask.width; x++)//z
        {
            for (int y = 0; y < textureMask.height; y++)
            {
                cols[x + y] = Color.white;
            }
        }

        //for(int i = 0; i < 200000; i++)
        //{
        //       cols[i] = Color.red;
        //}
        textureMask.SetPixels(cols);
        textureMask.Apply();
        //textureMask.SetPixel(10, 10, new Color(1, 1, 1, 1));
        mesh.material.SetTexture("_TextureMask", textureMask);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = cube.transform.position - this.transform.position;
            Vector2Int pixelPosition = new Vector2Int((int)position.x - textureMask.width, (int)position.y - textureMask.height);

            //not on this splatter map
            //if (pixelPosition.x < -40 || pixelPosition.x > 0 || //x
            //    pixelPosition.y < -40 || pixelPosition.y > 0 || //y 
            //    return;
            //for (int i = pixelPosition.x - 1; i < pixelPosition.x + 1; i++)
            //{
            //    //y
            //    for (int j = pixelPosition.y - 1; j < pixelPosition.y + 1; j++)
            //    {
            //        //z
            //        for (int k = pixelPosition.z - 1; k < pixelPosition.z + 1; k++)
            //        {
            //            Vector3Int pixelLocation = pixelPosition;// + new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
            //            texture3D.SetPixel(i, j, k, new Color(1, 1, 1, 1));
            //            //point.transform.position = pixelLocation;
            //            Debug.Log(pixelLocation);

            //        }
            //    }
            //}

            //textureMask.SetPixel(pixelPosition.x * textureMask.width, pixelPosition.y * textureMask.height, Color.white);
            ////sending data to GPU
            //// upating texture buffer
            //textureMask.Apply();
        }
    }

    public void UpdateTexture(Vector2 lightmapCoord)
    {
        int width = 10;
        int height = 10;
        for (int x = -width; x < width; x++)
        {
            for(int y = -height; y < height; y++)
            {
                textureMask.SetPixel((int)(lightmapCoord.x * textureMask.width) + x, (int)(lightmapCoord.y * textureMask.height) + y, Color.white);
            }
        }
        //for (int i = -width; i < 0; i++) 
        //{
        //    textureMask.SetPixel((int)(lightmapCoord.x * textureMask.width), (int)(lightmapCoord.y * textureMask.height) + i, Color.white);    
        //}
        //for (int i = -height; i < 0; i++)
        //{
        //    textureMask.SetPixel((int)(lightmapCoord.x * textureMask.width), (int)(lightmapCoord.y * textureMask.height) + i, Color.white);
        //}
        //sending data to GPU
        // upating texture buffer
        textureMask.Apply();
    }
}
