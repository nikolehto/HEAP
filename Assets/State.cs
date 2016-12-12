using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class State : NetworkBehaviour

{

    public GameObject controller;
    public static int startIndex = 1;

    [HideInInspector]
    [SyncVar(hook = "OnChangeTask")]
    public int taskIndex;

    private createBlocks block;

    void Awake()
    {
        block = controller.GetComponent<createBlocks>();
    }

	// Use this for initialization
	void Start () {
        if (isServer)
        {
            taskIndex = startIndex;
        }
        //block.setTask(taskIndex);
    }


     
   public void NextTask()
    {
        taskIndex += 1;
    }

    public void PreviousTask()
    {
        taskIndex -= 1;
    }

    void OnChangeTask(int newTask)
    {
        block.setTask(newTask);
    }
	
}
