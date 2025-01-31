using UnityEngine;

namespace Behaviours
{
	public abstract class SteeringBehaviour : MonoBehaviour {
		public int Priority { get; protected set; }
		public float Weight { get; protected set; }
		
		public abstract Vector3 GetDirection(Collider2D[] colliders, int size);
		
		protected abstract void Init();

		private void Awake() {
			Init();
		}
	}
	
	public abstract class BoidComponent : SteeringBehaviour {}
}
