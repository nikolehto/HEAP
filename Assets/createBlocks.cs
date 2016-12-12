using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class createBlocks : MonoBehaviour {

    public GameObject prefab;
    public static string filePathReadFiles = "Patterns/";
    public static string taskPrefix = "task";
    public static int startIndex = 1;
    private GameObject original, rotated;
    private Text label;
    enum GoSwitch { original_t, rotated_t };

    [HideInInspector]
    public int currentIndex;

    void Awake()
    {
        label = GameObject.Find("TaskNumberText").GetComponent<Text>();
    }

    void Start () {
        currentIndex = startIndex;

        string fileName = getNameFor(currentIndex);

        original = new GameObject();
        original.name = "Original figure";

        rotated = new GameObject();
        rotated.name = "Rotated figure";

        bool doesExist = readAndInitTask(fileName);

        if (!doesExist)
        {
            Debug.Log(fileName + " does not exist");
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

        name = filePathReadFiles + taskPrefix + name;
        return name;
    }

    public void nextPattern()
    {
        currentIndex += 1;

        Destroy(rotated);
        Destroy(original);

        original = new GameObject();
        original.name = "Original figure";
        rotated = new GameObject();
        rotated.name = "Rotated figure";

        string fileName = getNameFor(currentIndex);
        bool doesExist = readAndInitTask(fileName);

        if (!doesExist)
        {
            // TODO: callback?
            // inform that survey has ended 

            // debug memory test start over
            Debug.Log("was last, starting over");
            currentIndex = startIndex;
            fileName = getNameFor(currentIndex);
            
            doesExist = readAndInitTask(fileName);
            if(!doesExist)
            {
                Debug.Log("Cannot start over");
            }
                
        }
    }

    public void previousPattern()
    {
        currentIndex -= 1;

        if(currentIndex < startIndex)
        {
            currentIndex = startIndex;
        }

        Destroy(rotated);
        Destroy(original);

        original = new GameObject();
        original.name = "Original figure";
        rotated = new GameObject();
        rotated.name = "Rotated figure";

        string fileName = getNameFor(currentIndex);
        bool doesExist = readAndInitTask(fileName);

        if (!doesExist)
        {
            // TODO: callback?
            // inform that survey has ended 

            // debug memory test start over
            Debug.Log("Previous does not exist");
            Debug.Log("");
        }
    }

    bool readPattern(string filename, GoSwitch go_switch, bool isMirrored = false)
    {
        label.text = "Task " + currentIndex.ToString() ;

        // read pattern and save it to Figure
        // if read fail return false
        bool parseOk = true;

        TextAsset txtAsset = (TextAsset)Resources.Load(filename);
        if (txtAsset == null)
        {
            Debug.Log("ReadPatternFail");
            return false;
        }

        string[] linesFromfile = txtAsset.text.Split('\n');

        int x_size = -1;
        int y_size = -1;
        int z_size = -1;

        // 1. line = size of array
        string[] sizeOfArray = linesFromfile[0].Split('=')[1].Split('X');
        parseOk = parseOk && int.TryParse(sizeOfArray[0], out x_size);
        parseOk = parseOk && int.TryParse(sizeOfArray[1], out y_size);
        parseOk = parseOk && int.TryParse(sizeOfArray[2], out z_size);

        if (parseOk == false)
        {
            Debug.Log("ParsePatternFail");
            return false;
        }

        float x;
        float increment_x;

        if (isMirrored == false)
        {
            x = -(x_size / 2);
            increment_x = 1.0f;
        }
        else
        {
            x = +(x_size / 2);
            increment_x = -1.0f;
        }
        float y = (y_size / 2);
        float z = -(z_size / 2);

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
                    
                    switch (go_switch)
                    {
                        case GoSwitch.original_t:
                            cube.transform.parent = original.transform;
                            break;
                        case GoSwitch.rotated_t:
                            cube.transform.parent = rotated.transform;
                            break;
                    }

                    cube.transform.position = new Vector3(x, y, z);
                    x += increment_x;
                }

                else if (c == '0') // do not draw
                {

                    x += increment_x;
                }

                else if (c == ',') // next row
                {
                    y -= 1.0f;
                    x = x_start;
                    break;
                }

                else
                {

                }
            }
        }

        return true;

    }

    bool readAndInitTask(string filename)
    {
        Vector3 original_pos = new Vector3();
        Vector3 original_rot = new Vector3();

        Vector3 rotated_pos = new Vector3();
        Vector3 rotated_rot = new Vector3();

        string pattern1, pattern2;
        bool isMirrored;

        TextAsset txtAsset = (TextAsset)Resources.Load(filename);
        if (txtAsset == null)
        {
            Debug.Log("txtAsset not exists");
            return false;
        }

       string[] linesFromfile = txtAsset.text.Split('\n');

        // 1. line = pattern name
        pattern1 = linesFromfile[0].Split('=')[1].Trim();
        pattern1 = filePathReadFiles + pattern1;

        if (!readPattern(pattern1, GoSwitch.original_t))
        {
            Debug.Log("Read pattern1 failure :" + pattern1);
        }

        // 2. line = location of array
        string[] figurePosition = linesFromfile[1].Split('=')[1].Split('X');
        float.TryParse(figurePosition[0], out original_pos.x);
        float.TryParse(figurePosition[1], out original_pos.y);
        float.TryParse(figurePosition[2], out original_pos.z);

        // 3. line = rotation of array
        string[] figureRotation = linesFromfile[2].Split('=')[1].Split('X');
        float.TryParse(figureRotation[0], out original_rot.x);
        float.TryParse(figureRotation[1], out original_rot.y);
        float.TryParse(figureRotation[2], out original_rot.z);

        // 4. line = pattern name
        pattern2 = linesFromfile[3].Split('=')[1].Trim();
        pattern2 = filePathReadFiles + pattern2;

        // 5. line = pattern is mirrored
        string isMirrored_s = linesFromfile[4].Split('=')[1];
        bool.TryParse(isMirrored_s, out isMirrored);

        if (!readPattern(pattern2, GoSwitch.rotated_t, isMirrored))
        {
            Debug.Log("Read 'pattern2' failure :" + pattern2);
        }

        // 6. line = location offset of array
        string[] r_figurePosition = linesFromfile[5].Split('=')[1].Split('X');
        float.TryParse(r_figurePosition[0], out rotated_pos.x);
        float.TryParse(r_figurePosition[1], out rotated_pos.y);
        float.TryParse(r_figurePosition[2], out rotated_pos.z);

        // 7. line = rotation offset of array
        string[] r_figureRotation = linesFromfile[6].Split('=')[1].Split('X');
        float.TryParse(r_figureRotation[0], out rotated_rot.x);
        float.TryParse(r_figureRotation[1], out rotated_rot.y);
        float.TryParse(r_figureRotation[2], out rotated_rot.z);

        rotated_pos += original_pos;
        rotated_rot += original_rot;

        original.transform.rotation = Quaternion.Euler(original_rot);
        original.transform.position = original_pos;

        rotated.transform.rotation = Quaternion.Euler(rotated_rot);
        rotated.transform.position = rotated_pos;

        return true;
    }


    

	// Update is called once per frame
	void Update () {
	
	}
}
