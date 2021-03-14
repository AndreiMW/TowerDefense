/**
 * Created Date: 3/10/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace TowerDefense.Managers {
	public class UIManager : MonoBehaviour {
		public static UIManager Instance;
		
		[Header("Text elements")]
		[SerializeField] 
		private TMP_Text _countdownToNextWaveText;
		
		[SerializeField] 
		private TMP_Text _waveNumberText;
		
		[SerializeField] 
		private TMP_Text _gameResultViewText;
		

		[Header("Canvas groups")]
		[SerializeField]
		private CanvasGroup _nextWaveTimerParent;

		[SerializeField] 
		private CanvasGroup _gameResultView;

		[Header("Buttons")]
		[SerializeField]
		private Button _retryButton;

		#region Lifecycle

		private void Awake() {
			if (!Instance) {
				Instance = this;
			}

			this._gameResultView.alpha = 0f;
			this._retryButton.onClick.AddListener(this.RetryButtonClickListener);

			GameSceneManager.Instance.OnGameOver += this.HandleOnGameOverEvent;
		}

		#endregion
		
		#region Public

		/// <summary>
		/// Shows the next wave countdown.
		/// </summary>
		public void ShowNextWaveCountdownTimer() {
			this._nextWaveTimerParent.alpha = 1f;
		}
		
		/// <summary>
		/// Hides the next wave countdown.
		/// </summary>
		public void HideNextWaveCountdownTimer() {
			this._nextWaveTimerParent.alpha = 0f;
		}
		
		/// <summary>
		/// Sets the countdown text.
		/// </summary>
		/// <param name="timeToNextWave">Time to next wave.</param>
		public void SetCountDownText(int timeToNextWave) {
			this._countdownToNextWaveText.text = $"Next wave in: {timeToNextWave}";
		}

		/// <summary>
		/// Sets the current wave number.
		/// </summary>
		/// <param name="waveNumber">The current wave number.</param>
		public void SetWaveNumberText(int waveNumber) {
			this._waveNumberText.text = $"Wave: {waveNumber}";
		}
		
		#endregion
		
		#region Private

		/// <summary>
		/// Logic to execute when the game is over.
		/// </summary>
		/// <param name="isWon"></param>
		private void HandleOnGameOverEvent(bool isWon) {
			this._gameResultView.alpha = 1f;

			//if the game is won, display "You won!", else display "Game over".
			this._gameResultViewText.text = isWon ? "You won!" : "Game over";

		}

		/// <summary>
		/// The listener for retry button onClick.
		/// </summary>
		private void RetryButtonClickListener() {
			this._gameResultView.alpha = 0f;
			GameSceneManager.Instance.ExecuteGameRetry();
		}
		
		#endregion
	}
}