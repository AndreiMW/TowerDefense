/**
 * Created Date: 3/10/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using TowerDefense.Tower.Scripts;
using UnityEngine;

namespace TowerDefense.Towers.Scripts {
	public class TowerBuildManager : MonoBehaviour {
		public static TowerBuildManager Instance;
		
		private PlayerTower _towerPrefab;

		[SerializeField] 
		private Vector3 _positionOffset;

		private float _towerToInstantiateRange;
		
		[HideInInspector]
		public bool IsAllowedToBuild = false;
		
		#region Lifecycle

		private void Awake() {
			if (!Instance) {
				Instance = this;
			}
		}

		#endregion
		
		#region Public

		public void SetTowerPrefab(PlayerTower towerPrefab) {
			this._towerPrefab = towerPrefab;
			this._towerToInstantiateRange = towerPrefab.GetRange();
		}

		public PlayerTower BuildTower(Transform targetNodeToInstantiate) {
			PlayerTower tower = Instantiate(this._towerPrefab, targetNodeToInstantiate.position + this._positionOffset, targetNodeToInstantiate.rotation);
			this.IsAllowedToBuild = false;
			return tower;
		}

		public float GetTowerToInstantiateRange() {
			return this._towerToInstantiateRange * 1.8f;
		}
		
		#endregion
	}
}