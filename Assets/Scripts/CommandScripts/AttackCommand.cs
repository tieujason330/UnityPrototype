﻿using UnityEngine;
using System.Collections;
using System;

public class AttackCommand : BaseCommand
{

    public BaseWorldCharacter _targetCharacter;

    public AttackCommand(BaseWorldCharacter targetCharacter)
    {
        _commandType = CommandType.Attack;
        _targetCharacter = targetCharacter;
    }

    void Awake()
    {
        
    }

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}
