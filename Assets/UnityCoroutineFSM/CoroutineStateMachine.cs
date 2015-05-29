using UnityEngine;
using System.Collections;

namespace BrainBit.StateMachine
{
	public class MonoBehaveProxy:MonoBehaviour{}
	public class CoroutineStateMachine
	{

		private MonoBehaveProxy _proxy;

		public CoroutineStateMachine()
		{
			GameObject g = new GameObject("smProxy");
			this._proxy = g.AddComponent<MonoBehaveProxy> ();
		}

		public MonoBehaveProxy Proxy{
			get{
				return this._proxy;
				}
		}

		public Coroutine Tick (State state)
		{
			Coroutine result = null;
			if (state != null) {
				if (state.Check) {
					result = this._proxy.StartCoroutine (this._tick (state));
				}
			}
			return result;
		}

		private IEnumerator _tick (State state)
		{

			if (state != null) {
				state.StateRunning = true;

				state.Action(this._proxy);
				while (state.ActionRunning) {
					yield return new WaitForFixedUpdate ();
				}
				
				if (state.Children.Count > 0) {
					foreach (State child in state.Children) {
						bool check = child.Check;

						if (check) {
							this._proxy.StartCoroutine (this._tick (child));
							while (child.StateRunning) {
								yield return new WaitForFixedUpdate ();
							}
						}

						if (!check && state.Type == StateType.AND) {
							break;
						}

					}
				}
				state.StateRunning = false;
			}

			yield return null;
		}
	}
}
