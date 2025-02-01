using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Controllers;

using Utility;

namespace Managers
{
	public class ObstacleSpawner : Singleton<ObstacleSpawner> {
	
		[SerializeField, Range(10, 30)] 
		private int _obstacleCount = 20;
	
		[SerializeField]
		private GameObject _obstaclePrefab;

		private List<Collider2D> _obstacles;
		
		// TODO: parametrize
		private void Start() {
			_obstacles = new ();
			List<float> obstacleHeights = new();
			for (int i = 0; i < _obstacleCount; i++) {
				float height;
				do {
					height = Random.Range(-49f, 49f);
				} while (obstacleHeights.Exists(h => Mathf.Abs(height - h) < 2f));
				obstacleHeights.Add(height);
			
				Vector3 startingPosition = new Vector3(Random.Range(-49f, 49f), height, 0f);
				GameObject obstacle = Instantiate(_obstaclePrefab, startingPosition, Quaternion.identity);
				_obstacles.Add(obstacle.GetComponent<Collider2D>());
			
				ObstacleController controller = obstacle.GetComponent<ObstacleController>();
				controller.Speed = Random.Range(5f, 20f) * ((Random.value > 0.5f)? 1f : -1f);
				controller.Bound = 49f;
			}
			_obstacles.Sort((x, y) => (int) Mathf.Sign(x.transform.position.y - y.transform.position.y));
		}

		public int GetObstaclesAroundHeight(float height, float radius, Collider2D[] returnArray) {
			int start = -1, count = 0;
			for (int i = 0; i < _obstacles.Count; i++) {
				float y = _obstacles[i].transform.position.y;
				if (Mathf.Abs(y - height) <= radius) {
					if (start == -1) {
						start = i;
					}
					count++;
				} else if (y > height + radius) {
					break;
				}
			}

			if (start != -1) {
				_obstacles.GetRange(start, count).CopyTo(returnArray);
			}
			return count;
		}
	}
}
