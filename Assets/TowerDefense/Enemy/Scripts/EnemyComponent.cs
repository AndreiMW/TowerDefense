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
		
		[SerializeField] 
		private float _speed;

		[SerializeField] 
		private float _health;

		[SerializeField] 
		private HealthBar _enemyHealthBar;

		private float _originalHealth;
		private float _originalSpeed;
		
		private WaypointManager _waypointManager => WaypointManager.Instance;
		private int _waypointIndex = 0;
		private Vector3 _originalPosition;

		private bool _shouldMove = false;
		private bool _isBoss = false;

		public event Action OnDeath;
		
		#region Lifecycle

		private void Awake() {
			this._originalPosition = this.transform.position;
			this._originalHealth = this._health;
			this._originalSpeed = this._speed;
			
			this._enemyHealthBar.SetMaxHealthAndUpdateHealthBar(this._health);
		}

		private void Start() {
			SceneManager.Instance.OnGameOver += isWon => this._shouldMove = false;
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

		public void StartEnemyMovement() {
			this._shouldMove = true;
		}

		public void StopEnemyMovement() {
			this._shouldMove = false;
		}

		public float GetHealth() {
			return this._health;
		}
		
		public void SetHealth(float health) {
			this._health = health;
			this._enemyHealthBar.SetMaxHealthAndUpdateHealthBar(health);
		}

		public float GetSpeed() {
			return this._speed;
		}

		public void SetSpeed(float speed) {
			this._speed = speed;
		}

		public void KillEnemy() {
			this.Kill();
		}

		public void MakeBoss(float bossHealth) {
			this._isBoss = true;
			this._health = bossHealth;
			this.transform.localScale += Vector3.one;
			this._speed--;
			this._enemyHealthBar.SetMaxHealthAndUpdateHealthBar(this._health);
		}

		#endregion
		
		#region Private

		private void MoveEnemyTowardsWaypoint(Transform waypoint) {
			Vector3 distance = waypoint.position - this.transform.position;
			this.transform.Translate(this._speed * Time.deltaTime * distance.normalized, Space.World);
		}
		
		private void CheckDistanceBetweenEnemyAndWaypoint() {
			float distance = Vector3.Distance(this.transform.position, this._waypointManager.GetWaypointAtIndex(this._waypointIndex).position);

			if (distance <= 0.5f) {
				this._waypointIndex++;
			}
		}
		
		private void TakeDamage(int damage) {
			this._health -= damage;
			this._enemyHealthBar.UpdateHealth(this._health);

			if (this._health <= 0) {
				this.Kill();
			}
		}

		private void Kill() {
			this.OnDeath?.Invoke();
			this.gameObject.SetActive(false);
			this.ResetProperties();
		}

		public void ResetProperties() {
			if (this._isBoss) {
				this.transform.localScale = Vector3.one;
				this._isBoss = false;
				this._speed = this._originalSpeed;
			}
			this._shouldMove = false;
			this._waypointIndex = 0;
			this.transform.position = this._originalPosition;
			this._health = this._originalHealth;
			this._enemyHealthBar.SetMaxHealthAndUpdateHealthBar(this._originalHealth);
		}
		
		#endregion
	}
}
