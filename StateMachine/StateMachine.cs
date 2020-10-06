using System;
using System.Collections.Generic;

namespace StateMachines
{
    /// <summary>
    /// Generic state machine.
    /// </summary>
    /// <typeparam name="TContext">Type of the context object.</typeparam>
    public class StateMachine<TContext>
    {
        public TContext Context { get; }
        public State<TContext> ActiveState { get; private set; }

        private readonly Dictionary<Type, State<TContext>> states = new Dictionary<Type, State<TContext>>();

        public StateMachine(TContext context) => Context = context;

        /// <summary>
        /// Sets the active state of the state machine to the provided type.
        /// </summary>
        /// <typeparam name="TState">Type of the state to transition to.</typeparam>
        public void SetState<TState>() where TState : State<TContext>, new() 
            => SetState(GetState<TState>(), true);

        /// <summary>
        /// Sets the active state of the state machine to the provided one.
        /// </summary>
        /// <param name="state">State to transition to.</param>
        /// <returns></returns>
        [Obsolete("Use generic method when possible")] 
        public void SetState(State<TContext> state) 
            => SetState(state, false);

        private void SetState(State<TContext> state, bool safe)
        {
            if (!safe)
            {
                if (state is null)
                    throw new ArgumentNullException(nameof(state));
                if (state.StateMachine != this)
                {
                    state.StateMachine = this;
                    state.Initialize();
                }

                var type = state.GetType();
                if (states.ContainsKey(type))
                    states[type] = state;
                else
                    states.Add(type, state);
            }

            ActiveState?.Exit();
            ActiveState = state;
            ActiveState.Enter();
        }

        private TState GetState<TState>() where TState : State<TContext>, new()
        {
            var type = typeof(TState);
            if (!states.TryGetValue(type, out var value))
            {
                value = new TState() { StateMachine = this };
                value.Initialize();
                states.Add(type, value);
            }

            return (TState)value;
        }

        /// <summary>
        /// Update the currently active state.
        /// </summary>
        public void Update()
        {
            ActiveState?.Update();
            ActiveState?.HandleTransitions();
            ActiveState?.LateUpdate();
        }
    }
}
