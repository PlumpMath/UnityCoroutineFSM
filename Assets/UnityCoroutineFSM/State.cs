using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BrainBit.StateMachine
{
	public enum StateType
	{
		AND, 
		OR
	}

	public class State: IEquatable<State>
	{
		public delegate bool stateCheck ();

		public delegate IEnumerator stateAction (State state);
	
		private Guid _id;
		private stateCheck _check;
		private stateAction _action;
		private List<State> _children;
		private StateType _type;
		private State _parent;
		private bool _stateRunning;
		private bool _actionRunning;
	
		public State ()
		{
			this._init ();
		}
	
		public State (StateType type)
		{
			this._init ();
			this._type = type;
		}
	
		public State (stateAction action, StateType type)
		{
			this._init ();
			this._type = type;
			this._action = action;
		}
	
		public State (stateAction action)
		{
			this._init ();
			this._action = action;
		}
	
		public State (stateCheck check, stateAction action, StateType type)
		{
			this._init ();
			this._check = check;
			this._action = action;
			this._type = type;
		}
	
		public State (stateCheck check, stateAction action)
		{
			this._init ();
			this._check = check;
			this._action = action;
		}
	
		public State (stateCheck check, stateAction action, StateType type, List<State> children)
		{
			this._id = Guid.NewGuid ();
			this._check = check;
			this._action = action;
			this._type = type;
			if (children != null) {
				this._children = children;
			} else {
				this._children = new List<State> ();
			}
		}
	
		private void _init ()
		{
			this._id = Guid.NewGuid ();
			this._children = new List<State> ();
			this._type = StateType.AND;
			this._action = null;
			this._check = () => {
				return true;
			};
		}

		public State Parent {
			get {
				return this._parent;
			}
			set {
				if (this._parent != null) {
					this._parent.Children.Remove (this);
				}
			
				this._parent = value; 
				if (this._parent != null) {
					this._parent.Children.Add (this);
				}
			}
		}

		public List<State> Children {
			get {
				return this._children;
			}
		}

		public bool StateRunning {
			get {
				return this._stateRunning;
			}
			set {
				this._stateRunning = value;
			}
		}
	
		public bool ActionRunning {
			get {
				return this._actionRunning;
			}
			set {
				this._actionRunning = value;
			}
		}
		public bool Check {
			get {
				if (this._check != null) {
					return this._check ();
				} else {
					return true;
				}
			}
		}

		public StateType Type{
			get{
				return this._type;
			}
		}

		public void Action(MonoBehaveProxy proxy)
		{
			if (this._action != null) {
				this._actionRunning = true;
				proxy.StartCoroutine(this._action(this));
			}
		}

	#region IEquatable implementation
		public bool Equals (State other)
		{
			return this._id == other._id;
		}
	#endregion
	}
}
