using Managers;
using UnityEngine;

namespace Behaviours {
	public class AvoidWalls : SteeringBehaviour {
		/*
		public override Vector3 GetDirection(Vector3 currentDirection, Collider2D[] colliders, int size) {
			Vector3 lookaheadPosition = transform.position + currentDirection * BoidShared.Instance.FOVRadius;
			
			Vector3 direction = Vector3.zero;
			float m = (lookaheadPosition.y - transform.position.y) / (lookaheadPosition.x - transform.position.x);
			if (Mathf.Abs(lookaheadPosition.x) >= 50f) {
				float sign = Mathf.Sign(lookaheadPosition.x);
				float x = sign * 50f;
				float y = m * (x - lookaheadPosition.x) + lookaheadPosition.y;
				
				Vector3 hitPos = new Vector3(x, y, 0);
				Vector3 targetPosition = hitPos + Vector3.left * (sign * BoidShared.Instance.WallAvoidDistance);
				direction += (targetPosition - transform.position);
			}
			if (Mathf.Abs(lookaheadPosition.y) >= 50f) {
				float sign = Mathf.Sign(lookaheadPosition.y);
				float y = sign * 50f;
				float x = (y - lookaheadPosition.y) / m + lookaheadPosition.x;
				
				Vector3 hitPos = new Vector3(x, y, 0);
				Vector3 targetPosition = hitPos + Vector3.down * (sign * BoidShared.Instance.WallAvoidDistance);
				direction += (targetPosition - transform.position);
			}

			return direction.normalized * BoidShared.Instance.WallAvoidWeight;
		}
		*/

		public override Vector3 GetDirection(Vector3 currentDirection, Collider2D[] colliders, int size) {
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