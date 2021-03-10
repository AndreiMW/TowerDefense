/**
 * Created Date: 3/10/2021
 * Author: Andrei-Florin Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Tower.Scripts {
	public class Tower : MonoBehaviour {
		[SerializeField] 
		private float _range;

		[SerializeField]
		private float turnSpeed = 10;

		[SerializeField] 
		private float _rateOfFire = 1;

		[SerializeField] 
		private Bullet _bulletPrefab;
		
		[SerializeField] 
		private Transform _bulletSpawnPoint;

		[SerializeField] 
		private int _bulletPoolSize;
		
		private Bullet[] _bulletsPool;

		private float _fireCooldown = 0f;
		
		private Transform _currentLockedTarget;

		private List<int> _availableBulletsIndex;

		#region Lifecycle

		private void Awake() {
			this._availableBulletsIndex = new List<int>();
			
			for (int i = 0; i < this._bulletPoolSize; i++) {
				this._availableBulletsIndex.Add(i);
			}

			this._bulletsPool = new Bullet[this._bulletPoolSize];

			for (int i = 0; i < this._bulletPoolSize; i++) {
				this._bulletsPool[i] = Instantiate(this._bulletPrefab, this._bulletSpawnPoint.position, Quaternion.identity);

				Bullet bullet = this._bulletsPool[i];
				
				bullet.gameObject.SetActive(false);
				bullet.SetBulletIndex(i);
				bullet.OnBulletReachedEnemy += ()=> this._availableBulletsIndex.Add(bullet.GetBulletIndex());
			}
		}

		private void Start() {
			InvokeRepeating(nameof(this.UpdateTarget), 0f,0.25f);
		}

		private void Update() {
			if (!this._currentLockedTarget) {
				return;
			}
			this.LockTarget();

			if (this._fireCooldown <= 0f) {
				this.Shoot();
				this._fireCooldown = 1f / this._rateOfFire;
			}

			this._fireCooldown -= Time.deltaTime;
		}
		
		#endregion
		
		#region Private

		private void UpdateTarget() {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			float shortestEnemyDistance = Mathf.Infinity;
			GameObject nearestEnemy = null;

			for (int i = 0; i < enemies.Length; i++) {
				float distanceToEnemy = Vector3.Distance(transform.position, enemies[i].transform.position);
				if (distanceToEnemy < shortestEnemyDistance) {
					shortestEnemyDistance = distanceToEnemy;
					nearestEnemy = enemies[i];
				}
			}

			if (nearestEnemy != null && shortestEnemyDistance <= this._range) {
				this._currentLockedTarget = nearestEnemy.transform;
			} else {
				this._currentLockedTarget = null;
			}
		}

		private void LockTarget() {
			Vector3 lookPosition = this._currentLockedTarget.transform.position - this.transform.position;
			Quaternion lookRotation = Quaternion.LookRotation(lookPosition);
			Vector3 rotation = Quaternion.Lerp(this.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
			this.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
		}

		private void Shoot() {
			int bulletIndex = this._availableBulletsIndex[0];
			this._availableBulletsIndex.Remove(bulletIndex);
			this._bulletsPool[bulletIndex].gameObject.SetActive(true);
			this._bulletsPool[bulletIndex].SetTargetToFollow(this._currentLockedTarget);
		}
		private void OnDrawGizmosSelected() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.transform.position, this._range);
		}
		
		#endregion
	}
}