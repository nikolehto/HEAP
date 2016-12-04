using UnityEngine;
using System.IO;
using System.Collections;

public class createBlocks : MonoBehaviour {

    public static string fileName = "Patterns/pattern01";
    public GameObject prefab;
    GameObject figure;


    void Start () {
        figure = new GameObject();
        //Debug.Log(fileName);

        readAndInitPattern(fileName);
    }


    void clearAll()
    {
        // TODO: remove all cubes

    }

    void readAndInitPattern(string filename)
    {
         // Grab a reference to the camera

        int x_size = -1;
        int y_size = -1;
        int z_size = -1;

        float fig_pos_x = 0.0f;
        float fig_pos_y = 0.0f;
        float fig_pos_z = 0.0f;

        float fig_rot_x = 0.0f;
        float fig_rot_y = 0.0f;
        float fig_rot_z = 0.0f;

        TextAsset txtAsset = (TextAsset)Resources.Load(filename);
        string[] linesFromfile = txtAsset.text.Split('\n');

        // 1. line = size of array
        string[] sizeOfArray = linesFromfile[0].Split('=')[1].Split('X');
        int.TryParse(sizeOfArray[0], out x_size);
        int.TryParse(sizeOfArray[1], out y_size);
        int.TryParse(sizeOfArray[2], out z_size);

        // 2. line = size of array
        string[] figurePosition = linesFromfile[1].Split('=')[1].Split('X');
        float.TryParse(figurePosition[0], out fig_pos_x);
        float.TryParse(figurePosition[1], out fig_pos_y);
        float.TryParse(figurePosition[2], out fig_pos_z);

        // 3. line = size of array
        string[] figureRotation = linesFromfile[2].Split('=')[1].Split('X');
        float.TryParse(figureRotation[0], out fig_rot_x);
        float.TryParse(figureRotation[1], out fig_rot_y);
        float.TryParse(figureRotation[2], out fig_rot_z);


        // TODO : Read cameraposition

        //Debug.Log(x_size);
        //Debug.Log(y_size);
        //Debug.Log(z_size);

        float x = -(x_size/2);
        float y = (y_size/2);
        float z = -(z_size/2);

        float x_start = x;
        float y_start = y;

        foreach (string s in linesFromfile)
        {
            if (s.StartsWith("!")) // comment
            {
                continue;
            }

            else if (s.StartsWith("#"))
            {
                z += 1.0f;
                y = y_start;
                continue;
            }

            // else
            
            foreach (char c in s)
            {
                if (c == '1') // draw cube
                {
                    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    GameObject cube = Instantiate(prefab) as GameObject;
                    cube.transform.parent = figure.transform;
                    cube.transform.position = new Vector3(x, y, z);
                    x += 1.0f;
                    //Debug.Log('1');
                }
             
                else if (c == '0') // do not draw
                {

                    x += 1.0f;
                    //Debug.Log('0');
                }

                else if (c == ',') // next row
                {
                    y -= 1.0f;
                    x = x_start;
                    break;
                    //Debug.Log("OK");
                }

                else
                {
                    //Debug.Log("Unknown char");
                    //Debug.Log(c);
                }
            }
        }
        figure.transform.rotation = Quaternion.Euler(fig_rot_x, fig_rot_y, fig_rot_z);
        figure.transform.position = new Vector3(fig_pos_x, fig_pos_y, fig_pos_z);
    }




	// Update is called once per frame
	void Update () {
	
	}
}
