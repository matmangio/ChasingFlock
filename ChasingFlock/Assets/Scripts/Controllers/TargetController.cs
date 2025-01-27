using System;

using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
	public class TargetController : MonoBehaviour {

		[SerializeField] private float _maxDistance = 10f;
		[SerializeField] private LayerMask _layerMask;

		private int _lastCornerCode = -1;
		
		private void Start() {
			PlaceInRandomCorner();
		}

		private void OnCollisionEnter2D(Collision2D other) {
			if (_layerMask == (_layerMask | (1 << other.gameObject.layer))) {
				PlaceInRandomCorner();
			}
		}
		
		private void PlaceInRandomCorner() {
			Vector2 cornerPosition;
			int cornerCode;
			do {
				cornerPosition = Random.insideUnitCircle;
				cornerCode = CornerCode(cornerPosition);
			} while (cornerCode == _lastCornerCode);
			_lastCornerCode = cornerCode;
			
			Vector3 position = new Vector3(-50f, -50f, 0f);
			position.x *= Mathf.Sign(Vector2.Dot(cornerPosition, Vector2.right));
			position.y *= Mathf.Sign(Vector2.Dot(cornerPosition, Vector2.up));
			position += (Vector3) cornerPosition * _maxDistance;
			
			transform.position = position;
		}
		
		private int CornerCode(Vector2 position) {
			int code = 0;
			code += (Vector2.Dot(position, Vector2.right) > 0) ? 0 : 1;
			code += (Vector2.Dot(position, Vector2.up) > 0) ? 0 : 2;
			return code;
		}

		// TODO: remove testing function
		private void Update() {
			if (Input.GetKeyDown(KeyCode.Space)) {
				PlaceInRandomCorner();
			}
		}
	}
}
