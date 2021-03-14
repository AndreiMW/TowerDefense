/**
 * Created Date: 3/10/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using TowerDefense.Managers;
using UnityEngine;

using TowerDefense.Tower.Scripts;

namespace TowerDefense.Map.Scripts {
	public class MapTileNode : MonoBehaviour {
		private TowerBuildManager _towerBuildManager => TowerBuildManager.Instance;
		
		[SerializeField] 
		private CanvasGroup _rangeImage;
		
		private Color _originalColor;
		
		private PlayerTower _tower;
		
		private Renderer _renderer;
		

		#region Lifecycle
		
		private void Awake() {
			this._renderer = this.GetComponent<Renderer>();
			this._originalColor = this._renderer.material.color;
			this._rangeImage.alpha = 0f;
		}

		#endregion
		
		#region MouseHandling

		/// <summary>
		/// Place a tower if the user triggers OnMouseDown.
		/// </summary>
		private void OnMouseDown() {
			if (this._tower || !this._towerBuildManager.IsAllowedToBuild) {
				return;
			}

			this._tower = this._towerBuildManager.BuildTower(this.transform);
			this._rangeImage.alpha = 0f;
			this._renderer.material.color = this._originalColor;
		}

		/// <summary>
		/// Show the tower range if the user triggers OnMouseEnter.
		/// </summary>
		private void OnMouseEnter() {
			if (this._tower || !this._towerBuildManager.IsAllowedToBuild) {
				return;
			}

			this._rangeImage.alpha = 0.2f;
			float towerRange = this._towerBuildManager.GetTowerToInstantiateRange() * 1.8f;
			this._rangeImage.GetComponent<RectTransform>().sizeDelta = new Vector2(towerRange,towerRange);
		}

		/// <summary>
		/// Hide the tower range if the user triggers OnMouseExit.
		/// </summary>
		private void OnMouseExit() {
			if (this._tower || !this._towerBuildManager.IsAllowedToBuild) {
				return;
			}
			this._rangeImage.alpha = 0f;
		}
		
		#endregion
	}
}