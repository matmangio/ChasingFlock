using Managers;
using UnityEngine;

namespace Behaviours {
	public class AvoidWalls : SteeringBehaviour {
		
		public override Vector3 GetDirection(Collider2D[] colliders, int size) {
			Vector3 direction = Vector3.zero;
			float distance = 0f;
			if (Mathf.Abs(transform.position.x) >= 50f - BoidShared.Instance.WallAvoidDistance) {
				direction += Vector3.left * Mathf.Sign(transform.position.x);
				distance = 50f - Mathf.Abs(transform.position.x);
			}
			if (Mathf.Abs(transform.position.y) >= 50f - BoidShared.Instance.WallAvoidDistance) {
				direction += Vector3.down * Mathf.Sign(transform.position.y);
				if (distance > 0) {
					distance = Mathf.Min(distance, 50f - Mathf.Abs(transform.position.y));
				} else {
					distance = 50f - Mathf.Abs(transform.position.y);
				}
				
			}
			direction = direction.normalized / (distance * distance + 0.0001f);

			return direction * BoidShared.Instance.WallAvoidWeight;
		}

		protected override void Init() {
			Priority = BoidShared.Instance.WallAvoidPriority;
		}
	}
}