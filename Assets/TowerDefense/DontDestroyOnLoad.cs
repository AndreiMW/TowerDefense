/**
 * Created Date: 3/13/2021
 * Author: Andrei-Florin Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

namespace TowerDefense {
	public class DontDestroyOnLoad : MonoBehaviour{
		private void Awake() {
			DontDestroyOnLoad(this.gameObject);
		}
	}
}
