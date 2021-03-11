/**
 * Created Date: 3/9/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System.Collections;

using UnityEngine;

using TowerDefense.Managers;

namespace TowerDefense.Enemy.Scripts {
	public class EnemyWaveSpawner : MonoBehaviour {

		[SerializeField] 
		private Enemy _enemyPrefab;

		[SerializeField] 
		private int _enemyPoolSize;
		
		[SerializeField] 
		private float _timeBetweenWaves;

		private bool _isWaveComplete = true;

		private float _countdown = 8;
		private int _waveNumber = 0;
		private int _numberOfEnemiesInWave = 3;
		
		private int _numberOfEnemiesKilled = 0;

		private Enemy[] _enemiesPool;

		private float _timeBetweenEnemies = 0.7f;

		private float _enemyHealthIncrement = 0f;

		#region Lifecycle

		private void Awake() {
			this._enemiesPool = new Enemy[this._enemyPoolSize];

			for (int i = 0; i < this._enemyPoolSize; i++) {
				this._enemiesPool[i] = Instantiate(this._enemyPrefab, this.transform.position, this.transform.rotation);
				Enemy enemy = this._enemiesPool[i];
				enemy.gameObject.SetActive(false);
				enemy.OnDeath += ()=> {
					enemy.gameObject.SetActive(false);
					this._numberOfEnemiesKilled++;
					this.CheckIfWaveIsComplete();
				};
			}
		}

		private void Update() {
			if (this._isWaveComplete) {
				if (this._countdown <= 0f) {
					this.SpawnWave();
					this._countdown = this._timeBetweenWaves;
					this._isWaveComplete = false;
				}
				ScoreManager.Instance.SetCountDownText(Mathf.FloorToInt(this._countdown + 1));
				this._countdown -= Time.deltaTime;	
			}
		}

		#endregion

		#region Private

		private void SpawnWave() {
			this._waveNumber++;
			ScoreManager.Instance.SetWaveNumberText(this._waveNumber);
			ScoreManager.Instance.HideNextWaveCountdownTimer();
			StartCoroutine(this.SpawnEnemiesFromPool(this._numberOfEnemiesInWave));
		}

		private IEnumerator SpawnEnemiesFromPool(int enemiesToSpawn) {
			for (int i = 0; i < enemiesToSpawn; i++) {
				Enemy enemy = this._enemiesPool[i];
				enemy.gameObject.SetActive(true);
				enemy.SetHealth(enemy.GetHealth() + this._enemyHealthIncrement);
				enemy.StartEnemyMovement();
				
				yield return new WaitForSeconds(this._timeBetweenEnemies);
			}
		}

		private void CheckIfWaveIsComplete() {
			if (this._numberOfEnemiesInWave == this._numberOfEnemiesKilled) {
				this._isWaveComplete = true;
				this._numberOfEnemiesKilled = 0;
				ScoreManager.Instance.ShowNextWaveCountdownTimer();

				if (this._waveNumber % 2 == 0) {
					this._numberOfEnemiesInWave += 2;
					this._enemyHealthIncrement += 20;
				}
			}
		}

		#endregion
	}
}