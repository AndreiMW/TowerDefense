/**
 * Created Date: 3/12/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System;

using UnityEngine;

namespace TowerDefense.Managers {
	public class GameSceneManager : MonoBehaviour {
		public static GameSceneManager Instance;
		
		[SerializeField] 
		private Animator _blackScreenfadeOutAnimator;

		public event Action<bool> OnGameOver;
		public event Action OnGameRetry;
		
		#region Lifecycle

		private void Awake() {
			if (!Instance) {
				Instance = this;
			}
			this._blackScreenfadeOutAnimator.Play("black_screen_fade_out");
		}

		#endregion
		
		#region Public

		public void ExecuteGameOver(bool isWon) {
			this.OnGameOver?.Invoke(isWon);
		}

		public void ExecuteGameRetry() {
			this.OnGameRetry?.Invoke();
		}
		
		#endregion
	}
}