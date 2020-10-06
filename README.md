# StateMachine
Base implementation of a finite state machine with type safety.

Only two classes are present:

`public class StateMachine<TContext>`

`public abstract class State<TContext>`



## What is `<TContext>` ?
This is the type of the context object of the states. Usually, in other implementations, the state machine itself serves as the context object; by providing a `public abstract class StateMachine` of sorts, so any implementation/client/product-specific state machine(context) can derive from it.



## Design principles
* The state machine class should be entirely reusable, as its only purposes are to hold the currently active state and the context object, and to provide the methods needed to update the active state and to transition to the next one.
* The context is the object that the states actually act and react upon. 
* Each state reads and writes to the context as needed, and calls the transition to a different state on the state machine when any set of conditions are met. 
* There has to be compile time type safety.



#### What does compile time type safety mean?
It is the reason why I'm using generics and separating the context from the state machine. Generics allow me to instantiate a context-specific state machine, which can only hold context-specific states. Non-generic, inheritance-based approaches lead to being able to easily set any state of any state machine to any other state machine, requiring more careful coding and safety checks at runtime to avoid undesired behaviour.



#### This seems rather complicated
```
public class StateMachine<TContext>
{
    //...//
    public void SetState<TState>() where TState : State<TContext>, new()
    //...//
}
```
It's not. This simply means that you can transition to any state `<TState>` as long as it derives from a State with the same type of context `<TContext>` as the state machine you're using. The `new()` constraint allows the state machine to create a new instance of the state in case it needs to. 

It might seem weird to constrain the constructor of a State, but if you need some initialization code, the base State class provides you with `protected virtual void Init()`, which is called internally when instantiating the State from the State Machine, so you can override it in case there is any initialization you might need. 



### I need my constructor!!! 
If you still need to instantiate your State from outside or you need to have parameters in your constructor for some reason, you can still use 

`[Obsolete] public void SetState(State<TContext> state)`

Note that you can't manually set the State Machine that your State object is pointing to. If you set the State this way, additional checks are made to make sure you're not passing null, in which case a exception is thrown; and that it's pointing to the right State Machine, in case it's not, it's reassigned and `Init()` gets called. Also, setting it this way will make your state override the one cached in the dictionary, if there is one.

