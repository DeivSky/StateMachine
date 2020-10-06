using System;
using StateMachines;
using FsmTest;
using UnityEngine;

const int count = 60;

var sm = StateMachine<ExampleContext>.Instantiate<ExampleState1>(new ExampleContext() { GameObject = new GameObject() });

for (int i = 0; i < count; i++)
    sm.UpdateState();

Console.WriteLine(sm.Context.GameObject.transform.position.ToString());
