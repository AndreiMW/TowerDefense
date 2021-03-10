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

		private float _countdown = 5;
		private int _waveNumber = 0;
		private int _numberOfEnemiesKilled = 0;

		private Enemy[] _enemiesPool;

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
			StartCoroutine(this.SpawnEnemiesFromPool(this._waveNumber));
		}

		private IEnumerator SpawnEnemiesFromPool(int enemiesToSpawn) {
			for (int i = 0; i < enemiesToSpawn; i++) {
				Enemy enemy = this._enemiesPool[i];
				enemy.gameObject.SetActive(true);
				enemy.StartEnemyMovement();
				
				yield return new WaitForSeconds(0.2f);
			}
		}

		private void CheckIfWaveIsComplete() {
			if (this._waveNumber == this._numberOfEnemiesKilled) {
				this._isWaveComplete = true;
				this._numberOfEnemiesKilled = 0;
			}
		}

		#endregion
	}
}