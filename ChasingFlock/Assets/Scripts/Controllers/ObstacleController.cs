using UnityEngine;

namespace Controllers
{
	public class ObstacleController : MonoBehaviour {

		public float Speed;
		
		public Vector3 Velocity { get { return Speed * Vector3.right; } }
		
		private void FixedUpdate() {
			float newX = transform.position.x + Speed * Time.fixedDeltaTime;
		
			// If new position is out of bounds, invert the speed and adjust it
			if (Mathf.Abs(newX) > 49f) {
				Speed = -Speed;
				newX = 2 * transform.position.x - newX;
			}
		
			transform.position = new Vector3(newX, transform.position.y, 0f);
		}
	}
}
