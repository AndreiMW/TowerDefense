/**
 * Created Date: 3/10/2021
 * Author: Andrei-Florin Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System;

using UnityEngine;

using TowerDefense.Enemy.Scripts;

namespace TowerDefense.Tower.Scripts {
	public class Bullet : MonoBehaviour {
		private EnemyComponent _target;
		[SerializeField] 
		private float _speed;

		private Vector3 _originalPosition;
		private int _bulletIndex;
		public event Action OnBulletReachedEnemy;

		private int _bulletDamage;
		private bool _shouldResetBulletIfTargetWasLost = true;

		#region Lifecycle

		private void Update() {
			if (!this._target || !this._target.gameObject.activeInHierarchy){
				if (this._shouldResetBulletIfTargetWasLost) {
					this.BulletReachedEnemy();
					this._shouldResetBulletIfTargetWasLost = false;
				}
				return;
			}
			
			Vector3 bulletDirection = this._target.transform.position - this.transform.position;
			float distanceThisFrame = this._speed * Time.deltaTime;
			this.transform.Translate(bulletDirection.normalized * distanceThisFrame, Space.World);
		}

		private void OnEnable() {
			this._originalPosition = this.transform.position;
		}

		#endregion
		
		#region Public
		
		public void SetTargetToFollow(EnemyComponent target) {
			this._shouldResetBulletIfTargetWasLost = true;
			this._target = target;
		}

		public void SetBulletIndex(int index) {
			this._bulletIndex = index;
		}

		public int GetBulletIndex() {
			return this._bulletIndex;
		}

		public void SetBulletDamage(int bulletDamage) {
			this._bulletDamage = bulletDamage;
		}

		public int GetBulletDamage() {
			return this._bulletDamage;
		}

		public void BulletReachedEnemy() {
			this._target = null;
			this.transform.position = this._originalPosition;
			this.gameObject.SetActive(false);
			this.OnBulletReachedEnemy?.Invoke();
		}
		
		#endregion
	}
}