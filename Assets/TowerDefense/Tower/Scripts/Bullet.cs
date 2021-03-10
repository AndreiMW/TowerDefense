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
		

		private Vector3 _originalPosition;
		private int _bulletIndex;
		public event Action OnBulletReachedEnemy;

		#region Lifecycle

		private void Update() {
//			if (!this._target) {
//				Destroy(this.gameObject);
//				return;
//			}

			Vector3 bulletDirection = this._target.position - this.transform.position;
			float distanceThisFrame = this._speed * Time.deltaTime;
			this.transform.Translate(bulletDirection.normalized * distanceThisFrame, Space.World);
		}

		private void OnEnable() {
			this._originalPosition = this.transform.position;
		}

		private void OnTriggerEnter(Collider other) {
			if (other.tag.Equals("Enemy")) {
				this.transform.position = this._originalPosition;
				this.OnBulletReachedEnemy?.Invoke();
				this.gameObject.SetActive(false);
			}
		}

		#endregion
		
		#region Public
		
		public void SetTargetToFollow(Transform target) {
			this._target = target;
		}

		public void SetBulletIndex(int index) {
			this._bulletIndex = index;
		}

		public int GetBulletIndex() {
			return this._bulletIndex;
		}
		
		#endregion
	}
}