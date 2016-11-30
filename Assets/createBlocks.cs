using UnityEngine;
using System.IO;
using System.Collections;

public class createBlocks : MonoBehaviour {


    //public static string fileName = "pattern001.dat";
    //private static string filePath = "Assets/Resources/Patterns/";
    //private static string file = filePath + fileName;
    public static string fileName = "Patterns/pattern01";
    public GameObject prefab;
    Camera mainCam;


    void Start () {
        mainCam = Camera.main;
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

        float cam_pos_x = 0.0f;
        float cam_pos_y = 0.0f;
        float cam_pos_z = 0.0f;

        float cam_rot_x = 0.0f;
        float cam_rot_y = 0.0f;
        float cam_rot_z = 0.0f;

        TextAsset txtAsset = (TextAsset)Resources.Load(filename);
        string[] linesFromfile = txtAsset.text.Split('\n');

        // 1. line = size of array
        string[] sizeOfArray = linesFromfile[0].Split('=')[1].Split('X');
        int.TryParse(sizeOfArray[0], out x_size);
        int.TryParse(sizeOfArray[1], out y_size);
        int.TryParse(sizeOfArray[2], out z_size);

        // 2. line = size of array
        string[] cameraPosition = linesFromfile[1].Split('=')[1].Split('X');
        float.TryParse(cameraPosition[0], out cam_pos_x);
        float.TryParse(cameraPosition[1], out cam_pos_y);
        float.TryParse(cameraPosition[2], out cam_pos_z);

        mainCam.transform.position = new Vector3(cam_pos_x, cam_pos_y, cam_pos_z);

        // 3. line = size of array
        string[] cameraRotation = linesFromfile[2].Split('=')[1].Split('X');
        float.TryParse(cameraRotation[0], out cam_rot_x);
        float.TryParse(cameraRotation[1], out cam_rot_y);
        float.TryParse(cameraRotation[2], out cam_rot_z);

        mainCam.transform.rotation = Quaternion.Euler(cam_rot_x, cam_rot_y, cam_rot_z);
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

                    cube.transform.position = new Vector3(x, y, z);
                    //cube.renderer.material.color = Color.black;
                    //cube.GetComponent<Renderer>().material.color = Color.red;
                    x += 1.0f;
                    //Debug.Log('1');
                }
             
                else if (c == '0') // do not draw
                {

                    x += 1.0f;
                    //Debug.Log('0');
                }

                else if (c == '#') // next row
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
        
    }



	// Update is called once per frame
	void Update () {
	
	}
}


/*
public static void WriteDefaultValues()
{
    using (BinaryWriter writer = new BinaryWriter(File.Open(file, FileMode.Create)))
    {

        string path = Application.dataPath;
        writer.Write(1.250F);
        writer.Write(path + "/Resources");
        writer.Write(10);
        writer.Write(true);
    }
}

public static void DisplayValues()
{
    float aspectRatio;
    string tempDirectory;
    int autoSaveTime;
    bool showStatusBar;

    if (File.Exists(file))
    {

        using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
        {
            aspectRatio = reader.ReadSingle();
            tempDirectory = reader.ReadString();
            autoSaveTime = reader.ReadInt32();
            showStatusBar = reader.ReadBoolean();
        }

        Debug.Log("Aspect ratio set to: " + aspectRatio);
        Debug.Log("Temp directory is: " + tempDirectory);
        Debug.Log("Auto save time set to: " + autoSaveTime);
        Debug.Log("Show status bar: " + showStatusBar);

        using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
        { 
            byte[] allData = reader.ReadBytes(int.MaxValue);
            Debug.Log(allData);
        }


    }
    else
    {
        Debug.Log("File Not FOund");
    }
}

*/
