using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviours;
using Managers;
using UnityEngine;

namespace Controllers {
	
	internal struct SteeringGroup {
		internal int Priority;
		internal List<SteeringBehaviour> Behaviours;
	}
	
	public class BoidController : MonoBehaviour {
		
		public Vector3 Velocity { get { return transform.up * BoidShared.Instance.Speed; } }
		
		private Vector3 _updatedDirection;
		private List<SteeringGroup> _steeringGroups;

		private float _fovCosine;
		private LayerMask _boidLayer;
		private Collider2D[] _obstacles;
		private Collider2D[] _boidNeighbours;

		private void Start() {
			_obstacles = new Collider2D[ObstacleSpawner.Instance.Count * 2];
			_boidNeighbours = new Collider2D[BoidSpawner.Instance.Count * 2];
			
			_fovCosine = Mathf.Cos((BoidShared.Instance.FOV / 2f) * Mathf.Deg2Rad);
			_boidLayer = LayerMask.GetMask("Boids");
			
			// Sort all steering behaviours in groups of descending priority
			List<SteeringBehaviour> behaviours = GetComponents<SteeringBehaviour>().ToList();
			behaviours.Sort((x, y) => y.Priority - x.Priority);
			
			_steeringGroups = new List<SteeringGroup>();
			for (int i = 0; i < behaviours.Count; ) {
				SteeringGroup group = new SteeringGroup {
					Priority = behaviours[i].Priority, 
					Behaviours = new List<SteeringBehaviour>()
				};
				while (i < behaviours.Count && behaviours[i].Priority == group.Priority) {
					group.Behaviours.Add(behaviours[i]);
					i++;
				}
				_steeringGroups.Add(group);
			}

			StartCoroutine(ComputeDirection());
		}
		
		private void FixedUpdate() {
			// Look towards the new direction
			if (_updatedDirection != Vector3.zero) {
				Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, _updatedDirection);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, BoidShared.Instance.AngularSpeed * Time.fixedDeltaTime);
			}
			
			transform.position += transform.up * (BoidShared.Instance.Speed * Time.fixedDeltaTime);
		}
		
		private IEnumerator ComputeDirection() {
			while (Application.isPlaying) {
				int obstaclesCount = ObstacleSpawner.Instance.GetObstaclesAroundHeight(transform.position.y, BoidShared.Instance.ObstacleVerticalRange, _obstacles);
				int boidNeighboursCount = Physics2D.OverlapCircleNonAlloc(transform.position, BoidShared.Instance.FOVRadius, _boidNeighbours, _boidLayer);
				boidNeighboursCount = FilterBoidsByFOV(boidNeighboursCount);
				
				Vector3 newDirection = Vector3.zero;
				foreach (SteeringGroup group in _steeringGroups) {
					newDirection = Vector3.zero;
					foreach (SteeringBehaviour behaviour in group.Behaviours) {
						if (behaviour is BoidComponent) {
							newDirection += behaviour.GetDirection(_boidNeighbours, boidNeighboursCount);
						} else if (behaviour is AvoidObstacles) {
							newDirection += behaviour.GetDirection(_obstacles, obstaclesCount);
						} else {
							newDirection += behaviour.GetDirection(null, 0);
						}
					}

					if (newDirection.magnitude > BoidShared.Instance.Epsilon) {
						break;
					}
				}
				_updatedDirection = newDirection.normalized;

				yield return new WaitForSeconds(BoidShared.Instance.DirectionDeltaTime);
			}
		}

		private int FilterBoidsByFOV(int size) {
			int removed = 0;
			for (int i = 0; i < size - removed;) {
				Vector3 neighbourPos = (_boidNeighbours[i].transform.position - transform.position).normalized;
				if (Vector3.Dot(neighbourPos, transform.up) < _fovCosine || _boidNeighbours[i].gameObject == gameObject) {
					_boidNeighbours[i] = _boidNeighbours[size - 1 - removed];
					removed++;
				} else {
					i++;
				}
			}

			return size - removed;
		}
	}
	
}