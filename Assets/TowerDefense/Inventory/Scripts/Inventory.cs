/**
 * Created Date: 3/11/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System;
using TowerDefense.Tower.Scripts;
using UnityEngine;
using UnityEngine.UI;


namespace TowerDefense.Inventory.Scripts {
	public class Inventory : MonoBehaviour {
		[SerializeField] 
		private GameObject _basicTowerPrefab;
		
		[SerializeField] 
		private GameObject _fastFireRateTowerPrefab;
		
		[SerializeField] 
		private GameObject _powerTowerPrefab;

		[SerializeField]
		private Button _basicTowerButton;
		[SerializeField]
		private Button _fastFireRateTowerButton;
		[SerializeField]
		private Button _powerTowerbutton;

		private TowerBuildManager _towerBuildInstance;

		private float _turretCost;
		private float _moneyAmount = 100;
		
		#region Lifecycle

		private void Awake() {
			this._basicTowerButton.onClick.AddListener(this.GetBasicCannon);
			this._fastFireRateTowerButton.onClick.AddListener(this.GetFastFireRateCannon);
			this._powerTowerbutton.onClick.AddListener(this.GetPowerCannon);
		}

		private void Start() {
			this._towerBuildInstance = TowerBuildManager.Instance;
		}

		#endregion
		
		#region Public

		private void GetBasicCannon() {
			this._turretCost = 50f;
			if (this._moneyAmount < 50) {
				Debug.Log("Not enough money!");
				return;
			}

			this._moneyAmount -= this._turretCost;
			this._towerBuildInstance.SetTowerPrefab(this._basicTowerPrefab);
			this._towerBuildInstance.IsAllowedToBuild = true;
		}
		
		private void GetFastFireRateCannon() {
			this._turretCost = 70f;
			if (this._moneyAmount < this._turretCost) {
				Debug.Log("Not enough money!");
				return;
			}
			this._moneyAmount -= this._turretCost;
			this._towerBuildInstance.SetTowerPrefab(this._fastFireRateTowerPrefab);
			this._towerBuildInstance.IsAllowedToBuild = true;
		}

		private void GetPowerCannon() {
			this._turretCost = 120f;
			if (this._moneyAmount < this._turretCost) {
				Debug.Log("Not enough money!");
				return;
			}
			this._moneyAmount -= this._turretCost;
			this._towerBuildInstance.SetTowerPrefab(this._powerTowerPrefab);
			this._towerBuildInstance.IsAllowedToBuild = true;
		}
		
		#endregion
	}
}