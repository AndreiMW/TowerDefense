/**
 * Created Date: 3/11/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System;
using TMPro;
using TowerDefense.Tower.Scripts;
using TowerDefense.Towers.Scripts;
using UnityEngine;
using UnityEngine.UI;


namespace TowerDefense.Inventory.Scripts {
	public class Inventory : MonoBehaviour {
		[SerializeField] 
		private PlayerTower _basicTowerPrefab;
		
		[SerializeField] 
		private PlayerTower _fastFireRateTowerPrefab;
		
		[SerializeField] 
		private PlayerTower _powerTowerPrefab;

		[SerializeField]
		private Button _basicTowerButton;
		[SerializeField]
		private Button _fastFireRateTowerButton;
		[SerializeField]
		private Button _powerTowerbutton;

		[SerializeField] 
		private TMP_Text _moneyAmountText;

		private TowerBuildManager _towerBuildInstance;

		private float _turretCost;
		private float _moneyAmount = 120;

		public static Inventory Instance;
		
		#region Lifecycle

		private void Awake() {
			
			if (!Instance) {
				Instance = this;
			}
			
			this._basicTowerButton.onClick.AddListener(this.GetBasicCannon);
			this._fastFireRateTowerButton.onClick.AddListener(this.GetFastFireRateCannon);
			this._powerTowerbutton.onClick.AddListener(this.GetPowerCannon);
		}

		private void Start() {
			this._towerBuildInstance = TowerBuildManager.Instance;
			this.SetMoneyAmountText();
		}

		#endregion
		
		#region Public

		public void AddMoney(float moneyAmount) {
			this._moneyAmount += moneyAmount;
			this.SetMoneyAmountText();
		}
		
		#endregion
		
		#region Private

		private void GetBasicCannon() {
			this._turretCost = 50f;
			if (this._moneyAmount < 50) {
				Debug.Log("Not enough money!");
				return;
			}

			this._moneyAmount -= this._turretCost;
			this.SetMoneyAmountText();
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
			this.SetMoneyAmountText();
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
			this.SetMoneyAmountText();
			this._towerBuildInstance.SetTowerPrefab(this._powerTowerPrefab);
			this._towerBuildInstance.IsAllowedToBuild = true;
		}

		private void SetMoneyAmountText() {
			this._moneyAmountText.text = this._moneyAmount.ToString();
		}
		
		#endregion
	}
}