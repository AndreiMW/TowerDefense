/**
 * Created Date: 3/10/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 ATiStudios. All rights reserved. 
 */

using UnityEngine;

namespace TowerDefense.Tower.Scripts {
	public class TowerBuildManager : MonoBehaviour {
		public static TowerBuildManager Instance;
		
		private GameObject _towerPrefab;

		[SerializeField] 
		private Vector3 _positionOffset;
		
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

		public void SetTowerPrefab(GameObject towerPrefab) {
			this._towerPrefab = towerPrefab;
		}

		public GameObject BuildTower(Transform targetNodeToInstantiate) {
			GameObject tower = Instantiate(this._towerPrefab, targetNodeToInstantiate.position + this._positionOffset, targetNodeToInstantiate.rotation);
			this.IsAllowedToBuild = false;
			return tower;
		}
		
		#endregion
	}
}