using UnityEngine;
using System.Collections;
using BrainBit.StateMachine;

public class Example : MonoBehaviour {

	private string label;
	private string otherLabel;
	private State subState;
	private CoroutineStateMachine _machine;

	void Awake()
	{
		_machine = new CoroutineStateMachine ();

	}

	void Start()
	{

		//Core
		State sRoot = new State (this._testOne, StateType.AND);
		
		State sOne = new State(this._testTwo, StateType.AND);
		sOne.Parent = sRoot;
		
		State sTwo = new State (this._testThree, StateType.AND);
		sTwo.Parent = sRoot;
		
		
		//Sub State
		this.subState = new State (this._testFour, StateType.OR);
		
		State subTwo = new State (()=>{return false;},this._testSix, StateType.AND);
		subTwo.Parent = this.subState;
		
		State subOne = new State (this._testFive, StateType.AND);
		subOne.Parent = this.subState;
		_machine.Tick (sRoot);

	}

	void OnGUI()
	{
		GUILayout.Label (label);
		GUILayout.Label (otherLabel);
	}

	private IEnumerator _testOne(State state)
	{
		this.label = "First Test";
		yield return new WaitForSeconds (1);

		for (int i = 0; i<5; i++) {
			this._machine.Tick(this.subState);
			while(this.subState.StateRunning)
			{
				yield return new WaitForFixedUpdate();
			}
		}

		state.ActionRunning = false;
	}

	private IEnumerator _testTwo(State state)
	{
		this.label = "FIN";
		yield return new WaitForSeconds (1.5f);
		state.ActionRunning = false;
	}

	private IEnumerator _testThree(State state)
	{
		this.label = "Just joking.";
		yield return null;
		state.ActionRunning = false;
	}

	private IEnumerator _testFour(State state)
	{
		for (int i = 3; i> -1; i--) {
			this.otherLabel = i.ToString();
			yield return new WaitForSeconds(1);
		}
		state.ActionRunning = false;
	}

	private IEnumerator _testFive(State state)
	{
		this.otherLabel = "Done";
		yield return new WaitForSeconds (1);
		state.ActionRunning = false;
	}

	private IEnumerator _testSix(State state)
	{
		this.otherLabel = "Shouldn't happen";
		yield return new WaitForSeconds(1);
		state.ActionRunning = false;
	}
}
