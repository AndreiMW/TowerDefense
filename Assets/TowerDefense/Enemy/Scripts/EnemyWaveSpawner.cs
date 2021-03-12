/**
 * Created Date: 3/9/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System.Collections;
using System.Collections.Generic;

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
		private bool _shouldSpawnBossNextWave = false;

		private float _countdown = 3;
		private int _waveNumber = 0;
		private int _numberOfEnemiesInWave = 3;
		
		private int _numberOfEnemiesKilled = 0;

		private Enemy[] _enemiesPool;

		private float _timeBetweenEnemies = 0.7f;

		private float _enemyHealthIncrement = 0f;
		private float _moneyAmountPerKill = 10f;

		private ScoreManager _scoreManagerInstance;
		private SceneManager _sceneManagerInstance;

		private List<Enemy> _activeEnemies;
		private Coroutine _startWaveCoroutine;

		#region Lifecycle

		private void Awake() {
			this._activeEnemies = new List<Enemy>();
			this._enemiesPool = new Enemy[this._enemyPoolSize];

			for (int i = 0; i < this._enemyPoolSize; i++) {
				this._enemiesPool[i] = Instantiate(this._enemyPrefab, this.transform.position, this.transform.rotation);
				Enemy enemy = this._enemiesPool[i];
				enemy.gameObject.SetActive(false);
				enemy.OnDeath += ()=> {
					Inventory.Scripts.Inventory.Instance.AddMoney(this._moneyAmountPerKill * (enemy.GetHealth()/100));
					enemy.gameObject.SetActive(false);
					this._numberOfEnemiesKilled++;
					this._activeEnemies.Remove(enemy);
					this.CheckIfWaveIsComplete();
				};
			}
		}

		private void Start() {
			this._scoreManagerInstance = ScoreManager.Instance;
			this._sceneManagerInstance = SceneManager.Instance;
			
			this._sceneManagerInstance.OnGameRetry += this.HandleOnGameRetry;
			this._sceneManagerInstance.OnGameOver += this.HandleGameOver;
		}

		private void Update() {
			if (this._isWaveComplete) {
				if (this._countdown <= 0f) {
					this.SpawnWave();
					this._countdown = this._timeBetweenWaves;
					this._isWaveComplete = false;
				}
				this._scoreManagerInstance.SetCountDownText(Mathf.FloorToInt(this._countdown + 1));
				this._countdown -= Time.deltaTime;	
			}
		}

		#endregion

		#region Private

		private void SpawnWave() {
			this._waveNumber++;
			this._scoreManagerInstance.SetWaveNumberText(this._waveNumber);
			this._scoreManagerInstance.HideNextWaveCountdownTimer();
			this._startWaveCoroutine = StartCoroutine(this.SpawnEnemiesFromPool(this._numberOfEnemiesInWave));
		}

		private IEnumerator SpawnEnemiesFromPool(int enemiesToSpawn) {
			if (this._shouldSpawnBossNextWave) {
				this.SpawnBoss();
			} else {
				for (int i = 0; i < enemiesToSpawn; i++) {
					Enemy enemy = this._enemiesPool[i];
					enemy.gameObject.SetActive(true);
					enemy.SetHealth(enemy.GetHealth() + this._enemyHealthIncrement);
					enemy.StartEnemyMovement();
					this._activeEnemies.Add(enemy);
				
					yield return new WaitForSeconds(this._timeBetweenEnemies);
				}	
			}
		}

		private void CheckIfWaveIsComplete() {
			if (this._activeEnemies.Count == 0) {
				this._isWaveComplete = true;
				this._numberOfEnemiesKilled = 0;
				this._scoreManagerInstance.ShowNextWaveCountdownTimer();
				
				if (this._waveNumber % 3 == 0) {
					this._shouldSpawnBossNextWave = true;
				} else {
					this._numberOfEnemiesInWave += 2;
					this._enemyHealthIncrement += 20;
					this._moneyAmountPerKill += 10;
				}
			}
		}

		private void HandleOnGameRetry() {
			for (int i = 0; i < this._activeEnemies.Count; i++) {
				this._waveNumber = 0;
				this._numberOfEnemiesKilled = 0;
				this._scoreManagerInstance.SetWaveNumberText(this._waveNumber);
				this._isWaveComplete = true;
				this._activeEnemies[i].KillEnemy();
				this._scoreManagerInstance.ShowNextWaveCountdownTimer();
				Inventory.Scripts.Inventory.Instance.ResetMoneyAmount();
			}
		}

		private void HandleGameOver() {
			StopCoroutine(this._startWaveCoroutine);
		}

		private void SpawnBoss() {
			Enemy bossEnemy = this._enemiesPool[0];
			bossEnemy.gameObject.SetActive(true);
			bossEnemy.MakeBoss();
			bossEnemy.StartEnemyMovement();
			this._shouldSpawnBossNextWave = false;
		}
		

		#endregion
	}
}