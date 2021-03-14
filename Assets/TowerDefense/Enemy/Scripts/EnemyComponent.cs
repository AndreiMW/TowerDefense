/**
 * Created Date: 3/9/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System;

using UnityEngine;

using TowerDefense.Managers;
using TowerDefense.Map.Scripts;
using TowerDefense.Tower.Scripts;

namespace TowerDefense.Enemy.Scripts {
	public class EnemyComponent : MonoBehaviour {
		
		private WaypointManager _waypointManager => WaypointManager.Instance;
		
		[SerializeField] 
		private float _speed;
		
		[SerializeField] 
		private float _health;
		
		[SerializeField] 
		private HealthBar _enemyHealthBar;

		private float _originalHealth;
		private float _originalSpeed;
		
		private bool _shouldMove = false;
		private bool _isBoss = false;
		
		private int _waypointIndex = 0;
		
		private Vector3 _originalPosition;
		
		public event Action OnDeath;
		
		#region Lifecycle

		private void Awake() {
			//caches original properties
			this._originalPosition = this.transform.position;
			this._originalHealth = this._health;
			this._originalSpeed = this._speed;
			
			//sets maximum health for the healthbar and updates it
			this._enemyHealthBar.SetMaxHealthAndUpdateHealthBar(this._health);
		}

		private void Start() {
			//if it's game over, the enemy should stop
			GameSceneManager.Instance.OnGameOver += isWon => this._shouldMove = false;
		}

		private void Update() {
			if (!this._shouldMove) {
				return;
			}
			
			this.MoveEnemyTowardsWaypoint(this._waypointManager.GetWaypointAtIndex(this._waypointIndex));
			this.CheckDistanceBetweenEnemyAndWaypoint();
		}
		#endregion
		
		#region Collision

		private void OnTriggerEnter(Collider other) {
			if (other.gameObject.tag.Equals("Bullet")) {
				Bullet bullet = other.GetComponent<Bullet>();
				bullet.BulletReachedEnemy();
				this.TakeDamage(bullet.GetBulletDamage());
			}
		}
		
		#endregion
		
		#region Public

		/// <summary>
		/// Start the enemy movement.
		/// </summary>
		public void StartEnemyMovement() {
			this._shouldMove = true;
		}

		/// <summary>
		/// Get the current health of the enemy.
		/// </summary>
		/// <returns>The enemy health value.</returns>
		public float GetHealth() {
			return this._health;
		}
		
		/// <summary>
		/// Update the health of the enemy.
		/// </summary>
		/// <param name="health">The health amount of the enemy.</param>
		public void SetHealth(float health) {
			this._health = health;
			this._enemyHealthBar.SetMaxHealthAndUpdateHealthBar(health);
		}
		/// <summary>
		/// Kill the enemy.
		/// </summary>
		public void KillEnemy() {
			this.Kill();
		}
		
		/// <summary>
		/// Reset the properties of the enemy.
		/// </summary>
		public void ResetProperties() {
			//if enemy was boss, reset properties modified when upgrading enemy to boss
			if (this._isBoss) {
				this.transform.localScale = Vector3.one;
				this._speed = this._originalSpeed;
				this._isBoss = false;
			}
			this._shouldMove = false;
			this._waypointIndex = 0;
			this.transform.position = this._originalPosition;
			this._health = this._originalHealth;
			this._enemyHealthBar.SetMaxHealthAndUpdateHealthBar(this._originalHealth);
		}

		/// <summary>
		/// Update enemy to boss status.
		/// </summary>
		/// <param name="bossHealth">The amount of health for the boss.</param>
		public void MakeBoss(float bossHealth) {
			this._isBoss = true;
			this._health = bossHealth;
			this.transform.localScale += Vector3.one;
			this._speed--;
			this._enemyHealthBar.SetMaxHealthAndUpdateHealthBar(this._health);
		}

		#endregion
		
		#region Private

		/// <summary>
		/// Move the enemy towoards the next waypoint.
		/// </summary>
		/// <param name="waypoint">The waypoint to move the enemy to.</param>
		private void MoveEnemyTowardsWaypoint(Transform waypoint) {
			Vector3 distance = waypoint.position - this.transform.position;
			this.transform.Translate(this._speed * Time.deltaTime * distance.normalized, Space.World);
		}
		
		/// <summary>
		/// Checks for the distance between enemy and next waypoint.
		/// </summary>
		private void CheckDistanceBetweenEnemyAndWaypoint() {
			float distance = Vector3.Distance(this.transform.position, this._waypointManager.GetWaypointAtIndex(this._waypointIndex).position);

			if (distance <= 0.5f) {
				this._waypointIndex++;
			}
		}
		
		/// <summary>
		/// Loweres enemy health based on the amount of damage.
		/// </summary>
		/// <param name="damage">The amount of damage that needs substracted from the health.</param>
		private void TakeDamage(int damage) {
			this._health -= damage;
			this._enemyHealthBar.UpdateHealth(this._health);

			if (this._health <= 0) {
				this.Kill();
			}
		}

		/// <summary>
		/// Kill the enemy.
		/// </summary>
		private void Kill() {
			this.OnDeath?.Invoke();
			this.gameObject.SetActive(false);
			this.ResetProperties();
		}
		
		#endregion
	}
}
