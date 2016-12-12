using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class createBlocks : MonoBehaviour {

    public GameObject prefab;
    public static string filePathReadFiles = "Patterns/";
    public static string patternPrefix = "pattern";
    public static int startIndex = 1;
    private GameObject figure, r_figure;

    [HideInInspector]
    public int currentIndex;

    void Start () {
        currentIndex = startIndex;

        string fileName = getNameFor(currentIndex);

        figure = new GameObject();
        figure.name = "Original figure";
        //r_figure = new GameObject();
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

        Destroy(r_figure);
        Destroy(figure);


        figure = new GameObject();
        figure.name = "Original figure";
        //r_figure = new GameObject();

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

        Vector3 fig_pos = new Vector3();
        Vector3 fig_rot = new Vector3();

        Vector3 r_fig_pos = new Vector3();
        Vector3 r_fig_rot = new Vector3();

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

        // 2. line = location of array
        string[] figurePosition = linesFromfile[1].Split('=')[1].Split('X');
        float.TryParse(figurePosition[0], out fig_pos.x);
        float.TryParse(figurePosition[1], out fig_pos.y);
        float.TryParse(figurePosition[2], out fig_pos.z);

        // 3. line = rotation of array
        string[] figureRotation = linesFromfile[2].Split('=')[1].Split('X');
        float.TryParse(figureRotation[0], out fig_rot.x);
        float.TryParse(figureRotation[1], out fig_rot.y);
        float.TryParse(figureRotation[2], out fig_rot.z);

        // 4. line = location offset of array
        string[] r_figurePosition = linesFromfile[3].Split('=')[1].Split('X');
        float.TryParse(r_figurePosition[0], out r_fig_pos.x);
        float.TryParse(r_figurePosition[1], out r_fig_pos.y);
        float.TryParse(r_figurePosition[2], out r_fig_pos.z);

        // 5. line = rotation offset of array
        string[] r_figureRotation = linesFromfile[4].Split('=')[1].Split('X');
        float.TryParse(r_figureRotation[0], out r_fig_rot.x);
        float.TryParse(r_figureRotation[1], out r_fig_rot.y);
        float.TryParse(r_figureRotation[2], out r_fig_rot.z);

        r_fig_pos += fig_pos;
        r_fig_rot += fig_rot;

        float x = -(x_size/2);
        float y = (y_size/2);
        float z = -(z_size/2);

        float x_start = x;
        float y_start = y;

        foreach (string s in linesFromfile)
        {
            if (s.StartsWith("!") || s.StartsWith("//")) // comment
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


        
        figure.transform.rotation = Quaternion.Euler(fig_rot);
        figure.transform.position = fig_pos;

        r_figure = Instantiate(figure) as GameObject;
        //r_figure.transform.parent = figure.transform;

        r_figure.transform.rotation = Quaternion.Euler(r_fig_rot);
        r_figure.transform.position = r_fig_pos;


        Debug.Log(r_fig_pos);
        Debug.Log(r_fig_rot);

        r_figure.name = "Rotated figure";

        return true;
    }


    

	// Update is called once per frame
	void Update () {
	
	}
}
