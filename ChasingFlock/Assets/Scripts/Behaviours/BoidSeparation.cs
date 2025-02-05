using Managers;
using UnityEngine;

namespace Behaviours {
	public class Separation : BoidComponent {
		public override Vector3 GetDirection(Collider2D[] colliders, int size) {
			Vector3 separation = Vector3.zero;
			for (int i = 0; i < size; i++) {
				Vector3 tmp = transform.position - colliders[i].transform.position;
				separation += tmp.normalized / (tmp.magnitude + 0.0001f);
			}
			return separation * BoidShared.Instance.SeparationWeight;
		}

		protected override void Init() {
			Priority = BoidShared.Instance.SeparationPriority;
		}
	}
}