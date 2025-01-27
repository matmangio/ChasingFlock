using UnityEngine;

namespace Controllers
{
	public class ObstacleController : MonoBehaviour {

		public float Speed = 5f;
		public float Bound = 50f;
		public float Radius = 1f;
	
		private void FixedUpdate() {
			float newX = transform.position.x + Speed * Time.fixedDeltaTime;
		
			// If new position is out of bounds, invert the speed
			if (Mathf.Abs(newX) > Bound - Radius) {
				Speed = -Speed;
				newX = 2 * transform.position.x - newX;
			}
		
			transform.position = new Vector3(newX, transform.position.y, transform.position.z);
		}
	}
}
