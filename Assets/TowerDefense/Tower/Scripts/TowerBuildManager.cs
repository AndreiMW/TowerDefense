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

		[SerializeField]
		private GameObject _towerPrefab;
		
		#region Lifecycle

		private void Awake() {
			if (!Instance) {
				Instance = this;
			}
		}

		#endregion
		
		#region Public

		public GameObject BuildTower(Transform targetNodeToInstantiate) {
			GameObject tower = Instantiate(this._towerPrefab, targetNodeToInstantiate.position, targetNodeToInstantiate.rotation);
			return tower;
		}
		
		#endregion
	}
}