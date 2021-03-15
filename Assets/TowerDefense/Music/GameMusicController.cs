/**
 * Created Date: 3/13/2021
 * Author: Andrei-Florin Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

namespace TowerDefense {
	public class GameMusicController : MonoBehaviour {
		private static GameMusicController s_instance;
		
		#region Lifecycle
		
		private void Awake() {
			if (!s_instance) {
				s_instance = this;
				DontDestroyOnLoad(this.gameObject);
			} else {
				Destroy(this.gameObject);
			}
		}
		
		#endregion
	}
}
