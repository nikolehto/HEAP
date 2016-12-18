using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class State : NetworkBehaviour

{

    public GameObject controller;
    public GameObject head;
    public static int startIndex = 1;


    [HideInInspector]
    [SyncVar(hook = "OnChangeTask")]
    public int taskIndex;
    [HideInInspector]
    [SyncVar(hook = "OnSyncCall")]
    public bool rotationState;

    private createBlocks block;
    private sensorRotator sr;

    void Awake()
    {
        sr = head.GetComponent<sensorRotator>();
        block = controller.GetComponent<createBlocks>();
    }

	// Use this for initialization
	void Start () {
        if (isServer)
        {
            taskIndex = startIndex;
            rotationState = false;
        }
        //block.setTask(taskIndex);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        block.setTask(taskIndex);
    }


    public void NextTask()
    {
        taskIndex += 1;
    }

    public void PreviousTask()
    {
        taskIndex -= 1;
    }

    public void SyncRotation()
    {
        rotationState = !rotationState;
    }

    void OnChangeTask(int newTask)
    {
        block.setTask(newTask);
    }
	
    void OnSyncCall(bool sync)
    {
        sr.setOffSet();
    }
}
