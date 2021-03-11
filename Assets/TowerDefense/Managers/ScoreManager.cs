/**
 * Created Date: 3/10/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

using TMPro;

namespace TowerDefense.Managers {
	public class ScoreManager : MonoBehaviour {
		public static ScoreManager Instance;
		[SerializeField] 
		private TMP_Text _countdownToNextWaveText;
		
		[SerializeField] 
		private TMP_Text _waveNumberText;


		[SerializeField]
		private CanvasGroup _nextWaveTimerParent;
		
		
		#region Lifecycle

		private void Awake() {
			if (!Instance) {
				Instance = this;
			}
		}

		#endregion
		
		#region Public

		public void ShowNextWaveCountdownTimer() {
			this._nextWaveTimerParent.alpha = 1f;
		}
		
		public void HideNextWaveCountdownTimer() {
			this._nextWaveTimerParent.alpha = 0f;
		}
		
		public void SetCountDownText(int timeToNextWave) {
			this._countdownToNextWaveText.text = $"Next wave in: {timeToNextWave}";
		}

		public void SetWaveNumberText(int waveNumber) {
			this._waveNumberText.text = $"Wave: {waveNumber}";
		}
		
		#endregion
	}
}