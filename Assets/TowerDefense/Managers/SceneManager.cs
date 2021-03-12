/**
 * Created Date: 3/12/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System;

using UnityEngine;

namespace TowerDefense.Managers {
	public class SceneManager : MonoBehaviour {
		public static SceneManager Instance;

		public event Action<bool> OnGameOver;
		public event Action OnGameRetry;
		
		#region Lifecycle

		private void Awake() {
			if (!Instance) {
				Instance = this;
			}
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