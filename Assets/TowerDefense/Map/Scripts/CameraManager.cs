/**
 * Created Date: 3/10/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using UnityEngine;

namespace TowerDefense.Map.Scripts {
	public class CameraManager : MonoBehaviour {

		[SerializeField]
		private float _panSpeed = 30f;
		[SerializeField]
		private float _panBorderThickness = 10f;
		[SerializeField]
		private float _scrollSpeed = 5f;
		[SerializeField]
		private float _minY = 10f;
		[SerializeField]
		private float _maxY = 80f;
	
		#region Lifecycle
		private void Update() {
			

			if (Input.mousePosition.y >= Screen.height - this._panBorderThickness) {
				this.transform.Translate(this._panSpeed * Time.deltaTime * Vector3.forward, Space.World);
			}

			if (Input.mousePosition.y <= this._panBorderThickness) {
				this.transform.Translate(this._panSpeed * Time.deltaTime * Vector3.back, Space.World);
			}

			if (Input.mousePosition.x >= Screen.width - this._panBorderThickness) {
				this.transform.Translate(this._panSpeed * Time.deltaTime * Vector3.right, Space.World);
			}

			if (Input.mousePosition.x <= this._panBorderThickness) {
				this.transform.Translate(this._panSpeed * Time.deltaTime * Vector3.left, Space.World);
			}

			float scroll = Input.GetAxis("Mouse ScrollWheel");

			Vector3 pos = this.transform.position;

			pos.y -= scroll * 1000 * _scrollSpeed * Time.deltaTime;
			pos.y = Mathf.Clamp(pos.y, _minY, _maxY);

			this.transform.position = pos;

		}
		
		#endregion
	}
}