using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.SWStateMachine
{
	public class StateMachine : Node
	{
		[Export]
		public NodePath InitialState { get; set; } = new NodePath();

		private State state = new State();
		public State State {get { return state; } set {
				state = value;
				_stateName = State.Name;
			}
		}

		protected string? _stateName;

		public StateMachine()
		{
			AddToGroup("state_machine");
		}

		public override async void _Ready()
		{
			State = (State)GetNode(InitialState);
			_stateName = State.Name;
			await ToSignal(Owner, "ready");
			State.Enter();
		}

		public override void _UnhandledInput(InputEvent inputEvent)
		{
		   State.UnhandledInput(inputEvent);
		}

		public override void _PhysicsProcess(float delta)
		{
			State.PhysicsProcess(delta);
		}

		public void TransitionTo(string targetStatePath, Dictionary<string, Godot.Object>? msg = null)
		{
			msg ??= new Dictionary<string, Godot.Object>();

			if(!HasNode(targetStatePath))
				return;

			var targetState = GetNode<State>(targetStatePath);

			State.Exit();
			this.State = targetState;
			State.Enter(msg);
		}
	}
}
