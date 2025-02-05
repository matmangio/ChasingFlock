using Managers;
using UnityEngine;

namespace Behaviours {
	public class AvoidWalls : SteeringBehaviour {
		
		public override Vector3 GetDirection(Collider2D[] colliders, int size) {
			Vector3 direction = Vector3.zero;
			if (Mathf.Abs(transform.position.x) >= 50f - BoidShared.Instance.WallAvoidDistance) {
				direction += Vector3.left * Mathf.Sign(transform.position.x);
			}
			if (Mathf.Abs(transform.position.y) >= 50f - BoidShared.Instance.WallAvoidDistance) {
				direction += Vector3.down * Mathf.Sign(transform.position.y);
			}

			return direction.normalized * BoidShared.Instance.WallAvoidWeight;
		}

		protected override void Init() {
			Priority = BoidShared.Instance.WallAvoidPriority;
		}
	}
}