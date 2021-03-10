/**
 * Created Date: 3/10/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 ATiStudios. All rights reserved. 
 */

using UnityEngine;

using TMPro;

namespace TowerDefense.Managers {
	public class ScoreManager : MonoBehaviour {
		public static ScoreManager Instance;
		[SerializeField] 
		private TMP_Text _countdownToNextWaveText;
		
		
		#region Lifecycle

		private void Awake() {
			if (!Instance) {
				Instance = this;
			}
		}

		#endregion
		
		#region Public

		public void SetCountDownText(int timeToNextWave) {
			this._countdownToNextWaveText.text = $"Next wave in: {timeToNextWave}";
		}
		
		#endregion
	}
}