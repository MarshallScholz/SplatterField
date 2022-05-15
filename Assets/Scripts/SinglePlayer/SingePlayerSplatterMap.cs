using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingePlayerSplatterMap : MonoBehaviour
{
    Texture3D texture3D;
    public Transform cube;
    public Texture2D currentColour;
    //public GameObject point;
    //public Texture2D splat;

    public int gridSizeBox;
    Vector3Int gridSize;
    Vector3Int gridExtents;
    Vector2Int splatExtents;

    public int pixelMultiplyer = 1;
    public int lastPixelMultipleyer = 1;
    public int paintSplat = 10;
    public int lastPaintSplat = 10;

    public Vector3 paintOffset;

    public GameObject hitPoint;

    public float minPaintArea = 1;
    public float drawChance = 50;

    public float player1Score = 0;
    public float player2Score = 0;

    public Color paintColour = new Color(1, 0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        ResetSplatterMap();
    }

    void ResetSplatterMap()
    {
        gridSize = new Vector3Int(gridSizeBox, gridSizeBox, gridSizeBox);
        gridExtents = new Vector3Int(gridSize.x / 2, gridSize.y / 2, gridSize.z / 2);

        //creates a texture 3d (grid)
        texture3D = new Texture3D(gridSize.x * pixelMultiplyer, gridSize.y * pixelMultiplyer, gridSize.z * pixelMultiplyer, TextureFormat.ARGB32, true);
        int pixelCount = (gridSize.x * pixelMultiplyer) * (gridSize.y * pixelMultiplyer) * (gridSize.z * pixelMultiplyer);
        lastPaintSplat = paintSplat;
        lastPixelMultipleyer = pixelMultiplyer;
        Color[] cols = new Color[pixelCount];

        UpdateColour(currentColour);
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material.SetVector("_worldPosition", transform.position);
            mesh.material.SetFloat("_gridSize", gridExtents.x);
            mesh.material.SetFloat("_pixelMultiplyer", pixelMultiplyer);
            mesh.material.SetFloat("_paintSplat", paintSplat);
            mesh.material.SetTexture("_PaintColour", currentColour);

        }

        texture3D.SetPixels(cols);
        texture3D.Apply();

        //Sets "_voxels" in each shader to textured3D, so all gameobjects using this are updated at the same time when the texture3D updates
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material.SetTexture("_TextureMask1", texture3D);
        }
    }

    private void Update()
    {
        if (paintSplat != lastPaintSplat)
            foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
            {

                mesh.material.SetFloat("_paintSplat", paintSplat);
                lastPaintSplat = paintSplat;
            }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ResetSplatterMap();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            paintColour = new Color(1, 0, 0, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            paintColour = new Color(0, 1, 0, 1);
        }
    }

    //LOOK AT ARTICLE TO OPTIMIZE upto 15x faster https://answers.unity.com/questions/266170/for-different-texture-sizes-which-is-faster-setpix.html
    public void UpdatePaint(Vector3 collisionPosition)
    {
        //collisionPosition += new Vector3(1, 0, 1);
        Vector3 position = collisionPosition - this.transform.position;

        //========================== Added offset for better splat centers ====================================
        Vector3Int pixelPosition = new Vector3Int((Mathf.RoundToInt(position.x - gridExtents.x + paintOffset.x) * pixelMultiplyer), (Mathf.RoundToInt(position.y - gridExtents.y + paintOffset.y) * pixelMultiplyer), (Mathf.RoundToInt(position.z - gridExtents.z + paintOffset.z) * pixelMultiplyer));
        //Vector3 pixelPosition = collisionPosition;
        //Vector3Int pixelPosition = new Vector3Int((int)position.x, (int)position.y, (int)position.z);
        texture3D.SetPixel(pixelPosition.x, pixelPosition.y, pixelPosition.z, paintColour);
        int paintRadius = 3 * pixelMultiplyer;
        //Debug.Log("Raw Position : " + position);
        //Debug.Log("Splatter map position: " + pixelPosition);
        for (int i = pixelPosition.x - paintRadius; i < pixelPosition.x + paintRadius; i++)
        {
            //y   xxx
            for (int j = pixelPosition.y - paintRadius; j < pixelPosition.y + paintRadius; j++)
            {
                //z
                for (int k = pixelPosition.z - paintRadius; k < pixelPosition.z + paintRadius; k++)
                {
                    //Makes it look more splatty with smaller radius'
                    Vector3 delta = new Vector3(i - pixelPosition.x, j - pixelPosition.y, k - pixelPosition.z);
                    if (delta.magnitude < paintRadius / minPaintArea /* paintRadius*/)
                    {
                        //Vector3Int pixelLocation = pixelPosition;// + new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
                        //NOT WORKING BECAUSE THE PIXEL COLOUR WOULD BE THE PAINTED TEXTURES PIXEL COLOUR. NOT RGB FROM HERE
                        Color previousColour = texture3D.GetPixel(i, j, k);
                        texture3D.SetPixel(i, j, k, paintColour);
                        Color currentColour = texture3D.GetPixel(i, j, k);
                        if (previousColour == new Color(0, 1, 0, 1) && currentColour == new Color(1, 0, 0, 1))
                        {
                            player1Score--;
                        }
                        else if (previousColour == new Color(1, 0, 0, 1) && currentColour == new Color(0, 1, 0, 1))
                        {
                            player2Score--;
                        }

                        if (currentColour == new Color(1, 0, 0, 1) && previousColour != currentColour)
                        {
                            player1Score++;
                        }
                        else if (currentColour == new Color(0, 1, 0, 1) && previousColour != currentColour)
                        {
                            player2Score++;
                        }
                        //hitPoint.transform.position = pixelPosition;
                        //Debug.Log("Splatter map position: " + pixelPosition);
                    }
                    else
                    {
                        //decrease the drawchance as the i, j and k values increase to make nicer looking splats2
                        //USE A NOISE INSTEAD?
                        float drawPixel = Random.Range(0, 100);
                        if (drawChance > drawPixel)
                        {
                            texture3D.SetPixel(i, j, k, paintColour);
                        }
                    }
                }
            }
        }
        //sending data to GPU
        // upating texture buffer
        texture3D.Apply();
        cube.transform.position = collisionPosition;
    }

    public void UpdateColour(Texture2D newColour)
    {
        currentColour = newColour;
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material.SetTexture("_PaintColour", currentColour);
        }
    }
}

