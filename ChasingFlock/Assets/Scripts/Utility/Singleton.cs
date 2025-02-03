using UnityEngine;

namespace Utility {
	
	public class Singleton<T> : MonoBehaviour {
		
		public static T Instance { get; private set; }

		protected virtual void Awake() {
			if (Instance != null) {
				Destroy(this.gameObject);
			} else {
				Instance = GetComponent<T>();
			}
		}
	}
}