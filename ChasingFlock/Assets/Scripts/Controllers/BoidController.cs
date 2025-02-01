using System;
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
		
		private LayerMask _obstacleLayer;
		private LayerMask _boidLayer;
		private Collider2D[] _obstacles = new Collider2D[50];
		private Collider2D[] _boidNeighbours = new Collider2D[100];

		private void Start() {
			_obstacleLayer = LayerMask.GetMask("Obstacles");
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
			// Go in the current direction
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
				// TODO: refine based on FOV
				
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
	}
	
}