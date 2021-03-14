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
		private int _bulletDamage;
		
		private bool _shouldResetBulletIfTargetWasLost = true;

		public event Action OnBulletReachedEnemy;
		
		#region Lifecycle

		private void Awake() {
			this._originalPosition = this.transform.position;
		}

		private void Update() {
			if (!this._target || !this._target.gameObject.activeInHierarchy){
				if (this._shouldResetBulletIfTargetWasLost) {
					this.BulletReachedEnemy();
					this._shouldResetBulletIfTargetWasLost = false;
				}
				return;
			}
			
			//move bullet towards enemy
			Vector3 bulletDirection = this._target.transform.position - this.transform.position;
			float distanceThisFrame = this._speed * Time.deltaTime;
			this.transform.Translate(bulletDirection.normalized * distanceThisFrame, Space.World);
		}

		#endregion
		
		#region Public
		
		/// <summary>
		/// Sets the target that the bullet has to follow.
		/// </summary>
		/// <param name="target">The target to follow.</param>
		public void SetTargetToFollow(EnemyComponent target) {
			this._shouldResetBulletIfTargetWasLost = true;
			this._target = target;
		}

		/// <summary>
		/// Set the bullet index for the tower bullet pool.
		/// </summary>
		/// <param name="index">This bullet's index.</param>
		public void SetBulletIndex(int index) {
			this._bulletIndex = index;
		}

		/// <summary>
		/// Gets the bullet index for the tower bullet pool.
		/// </summary>
		/// <returns>This bullets' index.</returns>
		public int GetBulletIndex() {
			return this._bulletIndex;
		}

		/// <summary>
		/// Set the bullet damage.
		/// </summary>
		/// <param name="bulletDamage">The damage of the bullet.</param>
		public void SetBulletDamage(int bulletDamage) {
			this._bulletDamage = bulletDamage;
		}

		/// <summary>
		/// Gets the bullet damage.
		/// </summary>
		/// <returns>The amount of damage of the bullet.</returns>
		public int GetBulletDamage() {
			return this._bulletDamage;
		}

		/// <summary>
		/// Logic to execute when the bullet collides with an enemy.
		/// </summary>
		public void BulletReachedEnemy() {
			this._target = null;
			this.transform.position = this._originalPosition;
			this.gameObject.SetActive(false);
			this.OnBulletReachedEnemy?.Invoke();
		}
		
		#endregion
	}
}