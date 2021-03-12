/**
 * Created Date: 3/10/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

using TMPro;
using UnityEngine.UI;

namespace TowerDefense.Managers {
	public class ScoreManager : MonoBehaviour {
		public static ScoreManager Instance;
		[SerializeField] 
		private TMP_Text _countdownToNextWaveText;
		
		[SerializeField] 
		private TMP_Text _waveNumberText;


		[SerializeField]
		private CanvasGroup _nextWaveTimerParent;

		[SerializeField] 
		private CanvasGroup _gameOverView;

		[SerializeField]
		private Button _retryButton;
		
		
		#region Lifecycle

		private void Awake() {
			if (!Instance) {
				Instance = this;
			}

			this._gameOverView.alpha = 0f;
			this._retryButton.onClick.AddListener(this.HandleRetryButton);

			SceneManager.Instance.OnGameOver += this.HandleOnGameOverEvent;
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
		
		#region Private

		private void HandleOnGameOverEvent() {
			this._gameOverView.alpha = 1f;
		}

		private void HandleRetryButton() {
			this._gameOverView.alpha = 0f;
			SceneManager.Instance.ExecuteGameRetry();
		}
		
		#endregion
	}
}