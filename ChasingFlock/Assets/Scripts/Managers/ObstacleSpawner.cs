using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Controllers;

namespace Managers
{
	public class ObstacleSpawner : MonoBehaviour {
	
		[SerializeField, Range(10, 30)] 
		private int _obstacleCount = 20;
	
		[SerializeField]
		private GameObject _obstaclePrefab;
	
		// TODO: parametrize
		private void Start() {
			List<float> obstacleHeights = new();
			for (int i = 0; i < _obstacleCount; i++) {
				float height;
				do {
					height = Random.Range(-49f, 49f);
				} while (obstacleHeights.Exists(h => Mathf.Abs(height - h) < 2f));
				obstacleHeights.Add(height);
			
				Vector3 startingPosition = new Vector3(Random.Range(-49f, 49f), height, 0f);
				GameObject obstacle = Instantiate(_obstaclePrefab, startingPosition, Quaternion.identity);
			
				ObstacleController controller = obstacle.GetComponent<ObstacleController>();
				controller.Speed = Random.Range(5f, 20f) * ((Random.value > 0.5f)? 1f : -1f);
				controller.Bound = 50f;
				controller.Radius = 1f;
			}
		}
	}
}
