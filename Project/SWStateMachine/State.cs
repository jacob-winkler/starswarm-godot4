using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.SWStateMachine
{
    /// <summary>
    /// State interface to use in Hierarchical State Machines.
    /// The lowest leaf tries to handle callbacks, and if it can't, it delegates the work to its parent.
    /// It's up to the user to call the parent state's functions, e.g. `_parent.physics_process(delta)`
    /// Use State as a child of a StateMachine node.
    /// </summary>
    public partial class State : Node
    {
        protected StateMachine? _stateMachine;
        protected State? _parent = null;

        public override async void _Ready()
        {
            _stateMachine = (StateMachine?)GetStateMachine(this);
            await ToSignal(Owner, "ready");
            _parent = GetParent() as State;
        }

        public virtual void UnhandledInput(InputEvent inputEvent)
        { }

        public virtual void PhysicsProcess(double delta)
        { }

        public virtual void Enter(Dictionary<String, GodotObject>? msg = null)
        { }

        public virtual void Exit()
        { }

        public Node? GetStateMachine(Node node)
        {
            if (node != null && !node.IsInGroup("state_machine"))
                return GetStateMachine(node.GetParent());
            return node;
        }
    }
}
