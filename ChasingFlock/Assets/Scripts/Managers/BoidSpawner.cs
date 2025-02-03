using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Managers
{
	public class BoidSpawner : Singleton<BoidSpawner> {

		[SerializeField] private int _count = 50;
		public int Count => _count;
		[SerializeField] private float _radius = 10f;
		[SerializeField] private GameObject _boidPrefab;

		private new void Awake() {
			base.Awake();
			for (int i = 0; i < _count; i++) {
				Vector3 position = Random.insideUnitCircle * _radius;
				Instantiate(_boidPrefab, position, Quaternion.identity);
			}
		}
	}
}
