using System;
using System.Collections.Generic;

namespace StateMachines
{
    /// <summary>
    /// Generic state machine.
    /// </summary>
    /// <typeparam name="TContext">Type of the context object.</typeparam>
    public sealed class StateMachine<TContext>
    {
        public TContext Context { get; }
        public State<TContext> ActiveState { get; private set; }

        private readonly Dictionary<Type, State<TContext>> states = new Dictionary<Type, State<TContext>>();

        /// <summary>
        /// Instantiate a new state machine with the state <typeparamref name="TState"/>.
        /// </summary>
        /// <typeparam name="TState">Type of the initial state of the state machine.</typeparam>
        /// <param name="context">Context object.</param>
        /// <returns>The newly created state machine.</returns>
        public static StateMachine<TContext> Instantiate<TState>(TContext context) where TState : State<TContext>, new()
        {
            var sm = new StateMachine<TContext>(context);
            sm.SetState<TState>();
            return sm;
        }
        private StateMachine(TContext context) => 
            Context = context != null ? context : throw new ArgumentNullException(nameof(context));

        /// <summary>
        /// Sets the active state of the state machine to the provided type.
        /// </summary>
        /// <typeparam name="TState">Type of the state to transition to.</typeparam>
        public void SetState<TState>() where TState : State<TContext>, new()
        {
            ActiveState?.OnStateExit();
            ActiveState = GetState<TState>();
            ActiveState.OnStateEnter();
        }

        /// <summary>
        /// Fetches the state object specified.
        /// </summary>
        /// <typeparam name="TState">Type of the required state.</typeparam>
        /// <returns></returns>
        private TState GetState<TState>() where TState : State<TContext>, new()
        {
            var type = typeof(TState);
            if (!states.TryGetValue(type, out var value))
            {
                value = State<TContext>.Instantiate<TState>(this);
                states.Add(type, value);
            }

            return (TState)value;
        }

        /// <summary>
        /// Update the currently active state.
        /// </summary>
        public void UpdateState() => ActiveState?.OnUpdate();
    }
}
