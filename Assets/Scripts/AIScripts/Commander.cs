﻿using UnityEngine;
using System.Collections;
using System;

public class Commander : BaseRole
{
    public CaptainGroup _captainGroup;

    void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start()
    {
        //base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //base.Update();
    }

    public new void ExecuteCommand(BaseCommand command)
    {
        base.ExecuteCommand(command);
        // Tell Capt Group to do command too
        if (_captainGroup != null)
            _captainGroup.ExecuteCommand(command);
    }

    public override void PerformOwnAction(string _action)
    {
        //_animator.speed = 1f;
        //_animator.Play(_action);
    }

    public new void InitializeRole()
    {
        base.InitializeRole();
        Debug.Log("Commander init.");
    }
}
