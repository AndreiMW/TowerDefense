/**
 * Created Date: 3/11/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System;
using TowerDefense.Tower.Scripts;
using UnityEngine;

namespace TowerDefense.Inventory.Scripts {
	public class Inventory : MonoBehaviour {
		[SerializeField] 
		private GameObject _basicCannonPrefab;
		
		[SerializeField] 
		private GameObject _fastFireRateCannonPrefab;
		
		[SerializeField] 
		private GameObject _powerCannonPrefab;

		private TowerBuildManager _towerBuildInstance;
		
		#region Lifecycle

		private void Start() {
			this._towerBuildInstance = TowerBuildManager.Instance;
		}

		#endregion
		
		#region Public

		public void GetBasicCannon() {
			this._towerBuildInstance.SetTowerPrefab(this._basicCannonPrefab);
			this._towerBuildInstance.IsAllowedToBuild = true;
		}
		
		public void GetFastFireRateCannon() {
			this._towerBuildInstance.SetTowerPrefab(this._fastFireRateCannonPrefab);
			this._towerBuildInstance.IsAllowedToBuild = true;
		}

		public void GetPowerCannon() {
			this._towerBuildInstance.SetTowerPrefab(this._powerCannonPrefab);
			this._towerBuildInstance.IsAllowedToBuild = true;
		}
		
		#endregion
	}
}