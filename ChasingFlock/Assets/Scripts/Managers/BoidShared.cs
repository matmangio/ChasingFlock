using System;
using UnityEngine;
using Utility;

namespace Managers {
	
	[Serializable]
	public class BoidShared : Singleton<BoidShared> {

		[Header("General Parameters")]
		[Min(0)] public float Speed;
		[Min(0)] public float AngularSpeed;
		
		[Space]
		[Min(0)] public float FOVRadius;
		[Range(90, 360)] public float FOV;

		[Space]
		[Min(0)] public float DirectionDeltaTime;
		[Min(0)] public float Epsilon;
		
		[Header("Chase Parameters")]
		public GameObject ChaseTarget;
		[Space]
		public int ChasePriority;
		[Min(0)] public float ChaseWeight;

		[Header("Flocking Parameters")]
		[SerializeField] private bool _breath;
		[SerializeField] private float _breathSpeed;
		[SerializeField] private float _breathAmplitude;
		[Space]
		public int SeparationPriority;
		[Min(0)] public float SeparationWeight;

		[Space]
		public int CohesionPriority;
		[Min(0)] public float CohesionWeight;

		[Space]
		public int AlignmentPriority;
		[Min(0)] public float AlignmentWeight;

		[Header("Avoidance Parameters")]
		[Min(0)] public float ObstacleVerticalRange;
		[Min(1.5f)] public float ObstacleTouchDistance;
		[Min(1.5f)] public float ObstacleAvoidDistance;
		[Min(0)] public float WallAvoidDistance;
		[Min(0)] public float MaxLookAheadTime;
		
		[Space]
		public int ObstacleAvoidPriority;
		[Min(0)] public float ObstacleAvoidWeight;
		
		[Space]
		public int WallAvoidPriority;
		[Min(0)] public float WallAvoidWeight;

		private float _originalSeparation;
		private float _originalCohesion;

		private void Start() {
			_originalSeparation = SeparationWeight;
			_originalCohesion = CohesionWeight;
		}
		
		private void Update() {
			if (_breath) {
				CohesionWeight = _originalCohesion + (Mathf.Cos(Time.realtimeSinceStartup * _breathSpeed) * (_breathAmplitude / 2f));
				SeparationWeight = _originalSeparation + (Mathf.Sin(Time.realtimeSinceStartup * _breathSpeed) * (_breathAmplitude / 2f));
			}
		}
	}
}