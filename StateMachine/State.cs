namespace StateMachines
{
    /// <summary>
    /// Base class of a state in a state machine.
    /// </summary>
    /// <typeparam name="TContext">Type of the context object.</typeparam>
    public abstract class State<TContext>
    {
        public TContext Context => StateMachine.Context;
        public StateMachine<TContext> StateMachine { get; internal set; }

        internal void Initialize() => Init();
        internal void Enter() => OnStateEnter();
        internal void Exit() => OnStateExit();
        internal void Update() => OnUpdate();         
        internal void HandleTransitions() => ResolveTransitions();
        internal void LateUpdate() => OnLateUpdate();

        /// <summary>
        /// Called the first time an instance of the state is added to the state machine.
        /// </summary>
        protected virtual void Init() { }
        /// <summary>
        /// Called when transitioning to this state.
        /// </summary>
        protected abstract void OnStateEnter();
        /// <summary>
        /// Called when transitioning from this state.
        /// </summary>
        protected abstract void OnStateExit();
        /// <summary>
        /// Called when updating the state machine.
        /// </summary>
        protected abstract void OnUpdate();
        /// <summary>
        /// Called when updating the state machine, right after <see cref="OnUpdate"/> gets called.
        /// </summary>
        protected abstract void ResolveTransitions();
        /// <summary>
        /// Called when updating the state machine, right after <see cref="ResolveTransitions"/> gets called.
        /// This means that code here will be the first to run after <see cref="OnStateEnter"/>, but won't get called after <see cref="OnStateExit"/>.
        /// </summary>
        protected abstract void OnLateUpdate();
    }
}
