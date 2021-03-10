/**
 * Created Date: 3/10/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

namespace TowerDefense.Map.Scripts {
	public class MapTileNode : MonoBehaviour {
		[SerializeField]
		private Color _hoveringColor;

		private Color _originalColor;
		
		private Renderer _renderer;

		#region Lifecycle
		
		private void Awake() {
			this._renderer = this.GetComponent<Renderer>();
			this._originalColor = this._renderer.material.color;
		}
		
		#endregion
		
		#region MouseHandling
		
		private void OnMouseEnter() {
			this._renderer.material.color = this._hoveringColor;
		}

		private void OnMouseExit() {
			this._renderer.material.color = this._originalColor;
		}
		
		#endregion
	}
}