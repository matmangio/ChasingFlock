using Controllers;

using Managers;
using UnityEngine;

namespace Behaviours {
	public class AvoidObstacles : SteeringBehaviour {
		public override Vector3 GetDirection(Collider2D[] colliders, int size) {
			
			Vector3 boidVelocity = transform.up * BoidShared.Instance.Speed;
			Vector3 direction = Vector3.zero;
			for (int i = 0; i < size; i++) {
				float distance = (transform.position - (Vector3) colliders[i].ClosestPoint(transform.position)).magnitude;
				if (distance < BoidShared.Instance.ObstacleAvoidDistance) {
					ObstacleController obstacle = colliders[i].GetComponent<ObstacleController>();
					float timeToReach = Mathf.Min(distance / boidVelocity.magnitude, BoidShared.Instance.MaxLookAheadTime);
					Vector3 obstaclePrediction = obstacle.transform.position + obstacle.Velocity * timeToReach;
					direction += (transform.position - obstaclePrediction).normalized / (distance * distance * 0.0001f);
				}
			}
			
			return direction.normalized * BoidShared.Instance.ObstacleAvoidWeight;
		}

		protected override void Init() {
			Priority = BoidShared.Instance.ObstacleAvoidPriority;
		}
	}
}