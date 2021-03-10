/**
 * Created Date: 3/10/2021
 * Author: Andrei-Florin Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System;
using UnityEngine;

namespace TowerDefense.Tower.Scripts {
	public class Bullet : MonoBehaviour {
		private Transform _target;
		[SerializeField] 
		private float _speed;

		public void SetTargetToFollow(Transform target) {
			this._target = target;
		}

		private void Update() {
			if (!this._target) {
				Destroy(this.gameObject);
				return;
			}

			Vector3 bulletDirection = this._target.position - this.transform.position;
			float distanceThisFrame = this._speed * Time.deltaTime;
			//bullet hit logic, change with observer
			if (bulletDirection.magnitude <= distanceThisFrame) {
				this.Hit();
				return;
			}
			this.transform.Translate(bulletDirection.normalized * distanceThisFrame, Space.World);
		}

		private void Hit() {
			Destroy(this.gameObject);
			Destroy(this._target.gameObject);
		}
	}
}