/**
 * Created Date: 3/10/2021
 * Author: Andrei-Florin Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System.Collections.Generic;
using TowerDefense.Enemy.Scripts;
using UnityEngine;

using TowerDefense.Managers;

namespace TowerDefense.Tower.Scripts {
	public class PlayerTower : MonoBehaviour {
		[Header("Tower properties")]
		[SerializeField] 
		private float _range;

		[SerializeField]
		private float turnSpeed = 10;

		[SerializeField] 
		private float _rateOfFire = 1;

		[Header("Bullet properties")]
		[SerializeField] 
		private Bullet _bulletPrefab;
		
		[SerializeField] 
		private Transform _bulletSpawnPoint;

		[SerializeField] 
		private int _bulletPoolSize;

		[SerializeField] 
		private int _bulletDamage;

		private Bullet[] _bulletsPool;

		private float _fireCooldown = 0f;
		
		private EnemyComponent _currentLockedTarget;

		private List<int> _availableBulletsIndex;

		private bool _isGameOver = false;

		#region Lifecycle

		private void Awake() {
			this._availableBulletsIndex = new List<int>();
			
			//mark all bullets as available
			for (int i = 0; i < this._bulletPoolSize; i++) {
				this._availableBulletsIndex.Add(i);
			}

			this._bulletsPool = new Bullet[this._bulletPoolSize];

			//instantiate bullet pool
			for (int i = 0; i < this._bulletPoolSize; i++) {
				this._bulletsPool[i] = Instantiate(this._bulletPrefab, this._bulletSpawnPoint.position, Quaternion.identity);

				Bullet bullet = this._bulletsPool[i];
				
				bullet.SetBulletDamage(this._bulletDamage);
				bullet.gameObject.SetActive(false);
				bullet.SetBulletIndex(i);
				bullet.OnBulletReachedEnemy += ()=> this._availableBulletsIndex.Add(bullet.GetBulletIndex());
			}

		}

		private void Start() {
			GameSceneManager.Instance.OnGameOver += this.HandleGameOver;
			GameSceneManager.Instance.OnGameRetry += this.HandleRetry;
			
			InvokeRepeating(nameof(this.UpdateTarget), 0f,0.25f);
		}

		private void Update() {
			if (!this._currentLockedTarget || this._isGameOver) {
				return;
			}
			this.LockTarget();

			if (this._fireCooldown <= 0f) {
				this.Shoot();
				this._fireCooldown = 1f / this._rateOfFire;
			}

			this._fireCooldown -= Time.deltaTime;
		}

		private void OnDestroy() {
			GameSceneManager.Instance.OnGameOver -= this.HandleGameOver;
			GameSceneManager.Instance.OnGameRetry -= this.HandleRetry;
		}

		#endregion
		
		#region Public

		/// <summary>
		/// Get the range of the tower.
		/// </summary>
		/// <returns>The tower range.</returns>
		public float GetRange() {
			return this._range;
		}
		
		#endregion
		
		#region Private

		/// <summary>
		/// Search for all active enemies and update to the closest one.
		/// </summary>
		private void UpdateTarget() {
			EnemyComponent[] enemies = EnemyWaveSpawner.Instance.GetActiveEnemies();
			float shortestEnemyDistance = Mathf.Infinity;
			EnemyComponent nearestEnemyComponent = null;

			for (int i = 0; i < enemies.Length; i++) {
				float distanceToEnemy = Vector3.Distance(transform.position, enemies[i].transform.position);
				if (distanceToEnemy < shortestEnemyDistance) {
					shortestEnemyDistance = distanceToEnemy;
					nearestEnemyComponent = enemies[i];
				}
			}

			if (nearestEnemyComponent != null && nearestEnemyComponent.gameObject.activeInHierarchy && shortestEnemyDistance <= this._range) {
				this._currentLockedTarget = nearestEnemyComponent;
			} else {
				this._currentLockedTarget = null;
			}
		}

		/// <summary>
		/// Lock tower to the closest enemy.
		/// </summary>
		private void LockTarget() {
			if (this._currentLockedTarget.gameObject.activeInHierarchy) {
				Vector3 lookPosition = this._currentLockedTarget.transform.position - this.transform.position;
				Quaternion lookRotation = Quaternion.LookRotation(lookPosition);
				Vector3 rotation = Quaternion.Lerp(this.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
				this.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);	
			}
		}

		/// <summary>
		/// Activate bullet from available bullets pool and shoot towards locked enemy.
		/// </summary>
		private void Shoot() {
			int bulletIndex = this._availableBulletsIndex[0];
			this._availableBulletsIndex.Remove(bulletIndex);
			this._bulletsPool[bulletIndex].gameObject.SetActive(true);
			this._bulletsPool[bulletIndex].SetTargetToFollow(this._currentLockedTarget);
		}
		
		/// <summary>
		/// Gizmo to see the range in the editor.
		/// </summary>
		private void OnDrawGizmosSelected() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.transform.position, this._range);
		}

		/// <summary>
		/// Logic to execute when the game is over.
		/// </summary>
		/// <param name="isWon">Is the game won?</param>
		private void HandleGameOver(bool isWon) {
			this._isGameOver = true;
		}

		/// <summary>
		/// Logic to execute when retries game.
		/// </summary>
		private void HandleRetry() {
			Destroy(this.gameObject);
		}
		
		#endregion
	}
}