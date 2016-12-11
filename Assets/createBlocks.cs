using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class createBlocks : MonoBehaviour {

    public GameObject prefab;
    public static string filePathReadFiles = "Patterns/";
    public static string patternPrefix = "pattern";
    public static int startIndex = 1;
    private GameObject figure;

    [HideInInspector]
    public int currentIndex;

    void Start () {
        currentIndex = startIndex;

        string fileName = getNameFor(currentIndex);

        figure = new GameObject();
        bool doesExist = readAndInitPattern(fileName);

        if (!doesExist)
        {
            Debug.Log("Check filenames1");
        }

    }

    string getNameFor(int index)
    {
        /**
         * return: Name that is in same form than files in /filePathReadFiles/ folder
         * */
        string name;

        name = index.ToString();

        if(name.Length == 1)
        {
            name = "00" + name;
        }

        else if(name.Length == 2)
        {
            name = "0" + name;
        }

        // three is just fine
        // four is too much
        else if(name.Length > 3 )
        {
            Debug.Log("Limit of patterns 999 -  pattern naming rules and createBlocks::getNameFor must be changed");
        }

        name = filePathReadFiles + patternPrefix + name;
        return name;
    }

    public void nextPattern()
    {
        currentIndex += 1;

        Destroy(figure);
        figure = new GameObject();
        string fileName = getNameFor(currentIndex);
        bool doesExist = readAndInitPattern(fileName);


        if (!doesExist)
        {
            // TODO: callback?
            // inform that survey has ended 
            
            Debug.Log("was last, starting over");

            // debug memory test start over
            currentIndex = startIndex;
            fileName = getNameFor(currentIndex);
            
            doesExist = readAndInitPattern(fileName);
            if(!doesExist)
            {
                Debug.Log("Check filenames1");
            }
                
        }

        
    }

    /*
    public void clearAll()
    {
        //Debug.Log("Poistin kaikki cubet");
        // TODO: remove all cubes

    }
    */
    bool readAndInitPattern(string filename)
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
        if (txtAsset == null)
        {
            return false;
        }

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

        return true;
    }




	// Update is called once per frame
	void Update () {
	
	}
}
