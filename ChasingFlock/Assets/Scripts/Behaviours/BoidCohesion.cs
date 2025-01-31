using Managers;
using UnityEngine;

namespace Behaviours {
	public class BoidCohesion : BoidComponent {
		public override Vector3 GetDirection(Collider2D[] colliders, int size) {
			Vector3 cohesion = Vector3.zero;
			for (int i = 0; i < size; i++) {
				cohesion += colliders[i].transform.position;
			}
			cohesion /= size;
			return -cohesion.normalized * BoidShared.Instance.CohesionWeight;
		}

		protected override void Init() {
			Priority = BoidShared.Instance.CohesionPriority;
		}
	}
}