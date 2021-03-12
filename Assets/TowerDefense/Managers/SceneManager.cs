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

		public event Action OnGameOver;
		
		#region Lifecycle

		private void Awake() {
			if (!Instance) {
				Instance = this;
			}
		}

		#endregion
		
		#region Public

		public void ExecuteGameOver() {
			Debug.Log("GAME OVER!");
			this.OnGameOver?.Invoke();
		}
		
		#endregion
	}
}