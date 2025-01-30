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
		
		public Vector3 Direction { get; private set; }
		
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
			if (Direction != Vector3.zero) {
				Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, Direction);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, BoidShared.Instance.AngularSpeed * Time.fixedDeltaTime);
			}
			
			transform.position += transform.up * (BoidShared.Instance.Speed * Time.fixedDeltaTime);
		}
		
		private IEnumerator ComputeDirection() {
			while (Application.isPlaying) {
				int obstaclesCount = Physics2D.OverlapCircleNonAlloc(transform.position, BoidShared.Instance.FOVRadius, _obstacles, _obstacleLayer);
				int boidNeighboursCount = Physics2D.OverlapCircleNonAlloc(transform.position, BoidShared.Instance.FOVRadius, _boidNeighbours, _boidLayer);
				
				Vector3 newDirection = Vector3.zero;
				foreach (SteeringGroup group in _steeringGroups) {
					newDirection = Vector3.zero;
					foreach (SteeringBehaviour behaviour in group.Behaviours) {
						if (behaviour is BoidComponent) {
							newDirection += behaviour.GetDirection(Direction, _boidNeighbours, boidNeighboursCount);
						} else if (behaviour is AvoidObstacles) {
							newDirection += behaviour.GetDirection(Direction, _obstacles, obstaclesCount);
						} else {
							newDirection += behaviour.GetDirection(Direction, null, 0);
						}
					}

					if (newDirection.magnitude > BoidShared.Instance.Epsilon) {
						break;
					}
				}
				Direction = newDirection.normalized;

				yield return new WaitForSeconds(BoidShared.Instance.DirectionDeltaTime);
			}
		}
	}
	
}