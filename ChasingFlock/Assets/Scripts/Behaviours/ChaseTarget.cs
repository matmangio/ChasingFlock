using Managers;
using UnityEngine;

namespace Behaviours {
	
	public class ChaseTarget : SteeringBehaviour {

		[Header("Chase Parameters")]
		private GameObject _target;
		
		public override Vector3 GetDirection(Collider2D[] colliders, int size) {
			Vector3 direction = (_target.transform.position - transform.position).normalized;
			return direction * BoidShared.Instance.ChaseWeight;
		}

		protected override void Init() {
			_target = BoidShared.Instance.ChaseTarget;
			Priority = BoidShared.Instance.ChasePriority;
		}
	}
	
}