using System;
using StateMachines;
using UnityEngine;

namespace FsmTest
{
    public class ExampleContext
    {
        public GameObject GameObject;
        public ExampleComponent Component;
    }

    public abstract class ExampleStateBase : State<ExampleContext>
    {
        protected override void Init()
        {
            Console.WriteLine("Initializing state " + this.GetType().Name);
            Context.Component ??= Context.GameObject.AddComponent<ExampleComponent>();
        }
        public override void OnStateEnter() => Console.WriteLine("Entering state " + this.GetType().Name);
        public override void OnStateExit() => Console.WriteLine("Leaving state " + this.GetType().Name);
    }

    public sealed class ExampleState1 : ExampleStateBase
    {
        protected override void PreTransitionUpdate()
        {
            Context.Component.Value++;
            Console.WriteLine(Context.Component.Value);
        }

        protected override void ResolveTransitions()
        {
            if (Context.Component.Value > 5)
                StateMachine.SetState<ExampleState2>();
        }

        protected override void PostTransitionUpdate() { }
    }

    public sealed class ExampleState2 : ExampleStateBase
    {
        private Transform transform;

        protected override void Init()
        {
            base.Init();
            transform = Context.GameObject.transform;
        }

        protected override void PreTransitionUpdate()
        {
            transform.position += Vector3.one;
            Context.Component.Value--;
            Console.WriteLine(Context.Component.Value);
        }

        protected override void ResolveTransitions()
        {
            if (Context.Component.Value < -2)
                StateMachine.SetState<ExampleState1>();
        }

        protected override void PostTransitionUpdate() { }
    }
}
