using Managers;
using UnityEngine;

namespace Behaviours {
	public class BoidAlign : BoidComponent {
		public override Vector3 GetDirection(Collider2D[] colliders, int size) {
			Vector3 alignment = Vector3.zero;
			for (int i = 0; i < size; i++) {
				alignment += colliders[i].transform.up;
			}

			return alignment.normalized * BoidShared.Instance.AlignmentWeight;
		}

		protected override void Init() {
			Priority = BoidShared.Instance.AlignmentPriority;
		}
	}
}