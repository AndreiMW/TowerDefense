/**
 * Created Date: 3/11/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

using TMPro;
using TowerDefense.Managers;
using TowerDefense.Tower.Scripts;


namespace TowerDefense.Inventory.Scripts {
	public class PlayerInventory : MonoBehaviour {
		public static PlayerInventory Instance;
		
		private TowerBuildManager _towerBuildInstance => TowerBuildManager.Instance;
		
		[Header("Tower prefabs")]
		[SerializeField] 
		private PlayerTower _basicTowerPrefab;
		
		[SerializeField] 
		private PlayerTower _fastFireRateTowerPrefab;
		
		[SerializeField] 
		private PlayerTower _powerTowerPrefab;

		[Header("Inventory buttons")]
		[SerializeField]
		private InventoryTowerButton _basicTowerButton;
		[SerializeField]
		private InventoryTowerButton _fastFireRateTowerButton;
		[SerializeField]
		private InventoryTowerButton _powerTowerButton;
		
		[Header("Money text reference")]

		[SerializeField] 
		private TMP_Text _moneyAmountText;

		private float _turretCost;
		private float _moneyAmount = 120;
		private float _originalMoneyAmount;

		#region Lifecycle

		private void Awake() {
			if (!Instance) {
				Instance = this;
			}

			this._basicTowerButton.OnMouseDown.AddListener(eventData => this.GetBasicCannon());
			this._fastFireRateTowerButton.OnMouseDown.AddListener(eventData => this.GetFastFireRateCannon());
			this._powerTowerButton.OnMouseDown.AddListener(eventData => this.GetPowerCannon());
			
			this._basicTowerButton.OnEnter.AddListener(eventData=> this._basicTowerButton.ShowTooltip(this._moneyAmount));
			this._fastFireRateTowerButton.OnEnter.AddListener(eventData=> this._fastFireRateTowerButton.ShowTooltip(this._moneyAmount));
			this._powerTowerButton.OnEnter.AddListener(eventData=> this._powerTowerButton.ShowTooltip(this._moneyAmount));


			this._originalMoneyAmount = this._moneyAmount;
		}

		private void Start() {
			this.SetMoneyAmountText();
		}

		#endregion
		
		#region Public

		/// <summary>
		/// Adds money to inventory.
		/// </summary>
		/// <param name="moneyAmount">The amount of money to add.</param>
		public void AddMoney(float moneyAmount) {
			this._moneyAmount += moneyAmount;
			this.SetMoneyAmountText();
		}

		/// <summary>
		/// Reset the money amount.
		/// </summary>
		public void ResetMoneyAmount() {
			this._moneyAmount = this._originalMoneyAmount;
			this.SetMoneyAmountText();
		}

		/// <summary>
		/// Disable inventory buttons so user cannot place towers.
		/// </summary>
		public void DisableInventoryButtons() {
			this._basicTowerButton.enabled = false;
			this._fastFireRateTowerButton.enabled = false;
			this._powerTowerButton.enabled = false;
		}

		/// <summary>
		/// Enable inventory buttons.
		/// </summary>
		public void EnableInventoryButtons() {
			this._basicTowerButton.enabled = true; 
			this._fastFireRateTowerButton.enabled = true; 
			this._powerTowerButton.enabled = true;
		}
		
		#endregion
		
		#region Private

		/// <summary>
		/// Gets the basic turret.
		/// </summary>
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
		
		/// <summary>
		/// Gets the fast fire rate turret.
		/// </summary>
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

		/// <summary>
		/// Gets the high power turret.
		/// </summary>
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

		/// <summary>
		/// Sets the money amount text.
		/// </summary>
		private void SetMoneyAmountText() {
			this._moneyAmountText.text = this._moneyAmount.ToString();
		}
		
		#endregion
	}
}