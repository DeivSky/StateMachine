namespace StateMachines
{
    /// <summary>
    /// Base class of a state in a state machine.
    /// </summary>
    /// <typeparam name="TContext">Type of the context object.</typeparam>
    public abstract class State<TContext>
    {
        public TContext Context => StateMachine.Context;
        public StateMachine<TContext> StateMachine { get; private set; }

        protected State() { }
        public static TState Instantiate<TState>(StateMachine<TContext> sm) where TState : State<TContext>, new()
        {
            var state = new TState { StateMachine = sm };
            state.Init();
            return state;
        }

        protected virtual void Init() { }
        public abstract void OnStateEnter();
        public abstract void OnStateExit();
        public void OnUpdate()
        {
            PreTransitionUpdate();
            ResolveTransitions();
            PostTransitionUpdate();
        }
        protected abstract void PreTransitionUpdate();
        protected abstract void ResolveTransitions();
        protected abstract void PostTransitionUpdate();
    }
}
