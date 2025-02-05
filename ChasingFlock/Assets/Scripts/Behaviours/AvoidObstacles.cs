using Controllers;
using Managers;
using UnityEngine;

namespace Behaviours {
	public class AvoidObstacles : SteeringBehaviour {
		public override Vector3 GetDirection(Collider2D[] colliders, int size) {
			Vector3 boidVelocity = transform.up * BoidShared.Instance.Speed;
			Vector3 direction = Vector3.zero;
			float minTimeToCrash = float.MaxValue;
			for (int i = 0; i < size; i++) {
				ObstacleController obstacle = ObstacleSpawner.Instance.Controllers[colliders[i]];
				float currentDistance = (obstacle.transform.position - transform.position).magnitude;
				if (currentDistance < BoidShared.Instance.ObstacleTouchDistance) {
					direction = (transform.position - obstacle.transform.position).normalized;
					break;
				}
				
				float relativeSpeed = (obstacle.Velocity - boidVelocity).magnitude + 0.0001f;
				float timeToCrash = Mathf.Min(currentDistance / relativeSpeed, BoidShared.Instance.MaxLookAheadTime);
				if (timeToCrash < minTimeToCrash) {
					minTimeToCrash = timeToCrash;
					
					Vector3 obstaclePrediction = obstacle.transform.position + obstacle.Velocity * timeToCrash;
					if (Mathf.Abs(obstaclePrediction.x) > 49f) {
						obstaclePrediction.x = 100f * Mathf.Sign(obstaclePrediction.x) - obstaclePrediction.x;
					}
					Vector3 boidPrediction = transform.position + boidVelocity * timeToCrash;
					float futureDistance = (obstaclePrediction - boidPrediction).magnitude;
					if (futureDistance < BoidShared.Instance.ObstacleAvoidDistance) {
						direction = (boidPrediction - obstaclePrediction).normalized;
					}
				}
			}
			
			return direction.normalized * BoidShared.Instance.ObstacleAvoidWeight;
		}

		protected override void Init() {
			Priority = BoidShared.Instance.ObstacleAvoidPriority;
		}
	}
}