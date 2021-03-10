/**
 * Created Date: 3/10/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System;
using TowerDefense.Tower.Scripts;
using UnityEngine;

namespace TowerDefense.Map.Scripts {
	public class MapTileNode : MonoBehaviour {
		[SerializeField]
		private Color _hoveringColor;
		private Color _originalColor;
		
		private GameObject _turret;
		
		private Renderer _renderer;

		#region Lifecycle
		
		private void Awake() {
			this._renderer = this.GetComponent<Renderer>();
			this._originalColor = this._renderer.material.color;
		}
		
		#endregion
		
		#region MouseHandling

		private void OnMouseDown() {
			if (this._turret) {
				Debug.Log("Can't build here, sry");
				return;
			}

			this._turret = TowerBuildManager.Instance.BuildTower(this.transform);
		}

		private void OnMouseEnter() {
			if (this._turret) {
				return;
			}
			this._renderer.material.color = this._hoveringColor;
		}

		private void OnMouseExit() {
			if (this._turret) {
				return;
			}
			this._renderer.material.color = this._originalColor;
		}
		
		#endregion
	}
}