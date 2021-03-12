/**
 * Created Date: 3/11/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

using TMPro;

using TowerDefense.Tower.Scripts;
using TowerDefense.Towers.Scripts;


namespace TowerDefense.Inventory.Scripts {
	public class PlayerInventory : MonoBehaviour {
		[SerializeField] 
		private PlayerTower _basicTowerPrefab;
		
		[SerializeField] 
		private PlayerTower _fastFireRateTowerPrefab;
		
		[SerializeField] 
		private PlayerTower _powerTowerPrefab;

		[SerializeField]
		private InventoryTowerButton _basicTowerButton;
		[SerializeField]
		private InventoryTowerButton _fastFireRateTowerButton;
		[SerializeField]
		private InventoryTowerButton _powerTowerbutton;

		[SerializeField] 
		private TMP_Text _moneyAmountText;

		private TowerBuildManager _towerBuildInstance;

		private float _turretCost;
		private float _moneyAmount = 120;
		private float _originalMoneyAmount;

		public static PlayerInventory Instance;

		#region Lifecycle

		private void Awake() {
			
			if (!Instance) {
				Instance = this;
			}

			this._basicTowerButton.OnMouseDown.AddListener(eventData => this.GetBasicCannon());
			this._fastFireRateTowerButton.OnMouseDown.AddListener(eventData => this.GetFastFireRateCannon());
			this._powerTowerbutton.OnMouseDown.AddListener(eventData => this.GetPowerCannon());
			
			this._basicTowerButton.OnEnter.AddListener(eventData=> this._basicTowerButton.ShowTooltip(this._moneyAmount));
			this._fastFireRateTowerButton.OnEnter.AddListener(eventData=> this._fastFireRateTowerButton.ShowTooltip(this._moneyAmount));
			this._powerTowerbutton.OnEnter.AddListener(eventData=> this._powerTowerbutton.ShowTooltip(this._moneyAmount));


			this._originalMoneyAmount = this._moneyAmount;
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

		public void ResetMoneyAmount() {
			this._moneyAmount = this._originalMoneyAmount;
			this.SetMoneyAmountText();
		}
		
		#endregion
		
		#region Private

		private void GetBasicCannon() {
			this._turretCost = 50f;
			if (this._moneyAmount < this._turretCost) {
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