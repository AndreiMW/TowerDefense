/**
 * Created Date: 3/9/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System;
using UnityEngine;

namespace TowerDefense.Enemy.Scripts {
	public class EnemyWaveSpawner : MonoBehaviour {
		[SerializeField] 
		private Enemy[] _enemies;

		[SerializeField] 
		private float _timeBetweenEnemies = 1f;
		private int _enemyIndex = 0;
		private int _numberOfDeadEnemies = 0;
		private bool _isWaveStarted = false;
		
		#region Lifecycle

		private void Start() {
			foreach (Enemy enemy in this._enemies) {
				enemy.OnDeath += this.HandleOnEnemyDeath;
			}

			this._isWaveStarted = true;
		}

		private void Update() {
			if (this._isWaveStarted) {
				if (this._timeBetweenEnemies <= 0f) {
					this._enemies[this._enemyIndex].StartEnemyMovement();
				
					if (this._enemyIndex.Equals(this._enemies.Length - 1)) {
						this._enemyIndex = 0;
						this._isWaveStarted = false;
					} else {
						this._enemyIndex++;
					}
				
					this._timeBetweenEnemies = 1.5f;
				}

				this._timeBetweenEnemies -= Time.deltaTime;	
			}
		}

		#endregion

		#region Private

		private void HandleOnEnemyDeath() {
			this._numberOfDeadEnemies++;

			if (this._numberOfDeadEnemies.Equals(this._enemies.Length)) {
				Debug.Log("WAVE COMPLETE");
			}
		}

		#endregion
	}
}