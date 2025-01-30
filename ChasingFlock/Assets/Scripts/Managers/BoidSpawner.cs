using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
	public class BoidSpawner : MonoBehaviour {

		[SerializeField] private int _count = 50;
		[SerializeField] private float _radius = 10f;
		[SerializeField] private GameObject _boidPrefab;

		private void Awake() {
			for (int i = 0; i < _count; i++) {
				Vector3 position = Random.insideUnitCircle * _radius;
				Instantiate(_boidPrefab, position, Quaternion.identity);
			}
		}
	}
}
