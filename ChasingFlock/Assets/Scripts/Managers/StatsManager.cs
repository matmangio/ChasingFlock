using TMPro;
using UnityEngine;

namespace Managers
{
	public class StatsManager : MonoBehaviour {

		[Header("Options")]
		[SerializeField]
		[Range(0, 1)] private float _fpsUpdateInterval = 0.3f;
		
		[Header("References")]
		[SerializeField]
		private TextMeshProUGUI _FPSText;

		private int _frames = 0;
		private float _deltaTime = 0.0f;
		
		private void Update() {
			_frames++;
			_deltaTime += Time.deltaTime;
			if (_deltaTime >= _fpsUpdateInterval) {
				int fps = Mathf.RoundToInt(_frames / _deltaTime);
				_FPSText.text = fps.ToString();
				
				_deltaTime = 0.0f;
				_frames = 0;
			}
		}
	}
}
