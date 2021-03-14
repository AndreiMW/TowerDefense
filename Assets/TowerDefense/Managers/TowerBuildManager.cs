/**
 * Created Date: 3/10/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using TowerDefense.Tower.Scripts;
using UnityEngine;

namespace TowerDefense.Managers {
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

		/// <summary>
		/// Sets the tower prefab to the tower type it needs to build.
		/// </summary>
		/// <param name="towerPrefab">The tower prefab.</param>
		public void SetTowerPrefab(PlayerTower towerPrefab) {
			this._towerPrefab = towerPrefab;
			this._towerToInstantiateRange = towerPrefab.GetRange();
		}

		/// <summary>
		/// Build the tower.
		/// </summary>
		/// <param name="targetNodeToInstantiate">The map node where to instantiate the tower.</param>
		/// <returns></returns>
		public PlayerTower BuildTower(Transform targetNodeToInstantiate) {
			PlayerTower tower = Instantiate(this._towerPrefab, targetNodeToInstantiate.position + this._positionOffset, targetNodeToInstantiate.rotation);
			this.IsAllowedToBuild = false;
			return tower;
		}

		/// <summary>
		/// Get the tower range.
		/// </summary>
		/// <returns>The range of the tower that will be instantiated.</returns>
		public float GetTowerToInstantiateRange() {
			return this._towerToInstantiateRange;
		}
		
		#endregion
	}
}