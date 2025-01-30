using System;
using UnityEngine;
using Utility;

namespace Managers {
	
	[Serializable]
	public class BoidShared : Singleton<BoidShared> {

		[Header("General Parameters")]
		public float Speed;
		public float AngularSpeed;
		
		[Space]
		public float FOVRadius;
		[Range(0, 360)] public float FOV;

		[Space]
		public float DirectionDeltaTime;
		public float Epsilon;
		
		[Header("Chase Parameters")]
		public GameObject ChaseTarget;
		[Space]
		[Range(0, 5)] public int ChasePriority;
		[Range(0, 5)] public float ChaseWeight;

		[Header("Flocking Parameters")]
		[SerializeField] private bool _breath;
		[SerializeField] private float _breathSpeed;
		[SerializeField] private float _breathAmplitude;
		[Space]
		[Range(0, 5)] public int SeparationPriority;
		[Range(0, 5)] public float SeparationWeight;

		[Space]
		[Range(0, 5)] public int CohesionPriority;
		[Range(0, 5)] public float CohesionWeight;

		[Space]
		[Range(0, 5)] public int AlignmentPriority;
		[Range(0, 5)] public float AlignmentWeight;

		[Header("Avoidance Parameters")]
		[Range(0, 5)] public int ObstacleAvoidPriority;
		[Range(0, 5)] public float ObstacleAvoidWeight;

		[Space]
		public float WallAvoidDistance;
		[Space]
		[Range(0, 5)] public int WallAvoidPriority;
		[Range(0, 5)] public float WallAvoidWeight;

		private float _originalSeparation;
		private float _originalCohesion;

		private void Start() {
			_originalSeparation = SeparationWeight;
			_originalCohesion = CohesionWeight;
		}
		
		private void Update() {
			if (_breath) {
				CohesionWeight = _originalCohesion - (Mathf.Cos(Time.realtimeSinceStartup * _breathSpeed) * (_breathAmplitude / 2f));
				SeparationWeight = _originalSeparation - (Mathf.Sin(Time.realtimeSinceStartup * _breathSpeed) * (_breathAmplitude / 2f));
			}
		}
	}
}