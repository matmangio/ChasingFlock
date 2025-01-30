using Managers;
using UnityEngine;

namespace Behaviours {
	public class AvoidObstacles : SteeringBehaviour {
		public override Vector3 GetDirection(Vector3 currentDirection, Collider2D[] colliders, int size) {
			throw new System.NotImplementedException();
		}

		protected override void Init() {
			Priority = BoidShared.Instance.ObstacleAvoidPriority;
		}
	}
}