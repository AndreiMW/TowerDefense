/**
 * Created Date: 3/10/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using TowerDefense.Tower.Scripts;
using TowerDefense.Towers.Scripts;
using UnityEngine;


namespace TowerDefense.Map.Scripts {
	public class MapTileNode : MonoBehaviour {
//		[SerializeField]
//		private Color _hoveringColor;

		[SerializeField] 
		private CanvasGroup _rangeImage;
		
		private Color _originalColor;
		
		private PlayerTower _tower;
		
		private Renderer _renderer;

		private TowerBuildManager _towerBuildManager;
		

		#region Lifecycle
		
		private void Awake() {
			this._renderer = this.GetComponent<Renderer>();
			this._originalColor = this._renderer.material.color;
			this._rangeImage.alpha = 0f;
		}

		private void Start() {
			this._towerBuildManager = TowerBuildManager.Instance;
		}
		
		#endregion
		
		#region MouseHandling

		private void OnMouseDown() {
			if (this._tower || !this._towerBuildManager.IsAllowedToBuild) {
				Debug.Log("Can't build here, sry");
				return;
			}

			this._tower = TowerBuildManager.Instance.BuildTower(this.transform);
			this._rangeImage.alpha = 0f;
			this._renderer.material.color = this._originalColor;
		}

		private void OnMouseEnter() {
			if (this._tower || !this._towerBuildManager.IsAllowedToBuild) {
				return;
			}

			this._rangeImage.alpha = 0.2f;
			this._rangeImage.GetComponent<RectTransform>().sizeDelta = new Vector2(TowerBuildManager.Instance.GetTowerToInstantiateRange(), TowerBuildManager.Instance.GetTowerToInstantiateRange());

//			this._renderer.material.color = this._hoveringColor;
		}

		private void OnMouseExit() {
			if (this._tower || !this._towerBuildManager.IsAllowedToBuild) {
				return;
			}
			this._rangeImage.alpha = 0f;
//			this._renderer.material.color = this._originalColor;
		}
		
		#endregion
	}
}