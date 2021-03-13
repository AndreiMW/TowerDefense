/**
 * Created Date: 3/9/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TowerDefense.Inventory.Scripts;
using TowerDefense.Managers;

namespace TowerDefense.Enemy.Scripts {
	public class EnemyWaveSpawner : MonoBehaviour {
		public static EnemyWaveSpawner Instance;

		[SerializeField] 
		private EnemyComponent _enemyComponentPrefab;

		[SerializeField] 
		private int _enemyPoolSize;
		
		[SerializeField] 
		private float _timeBetweenWaves;
		
		private float _countdown = 8f;
		private float _timeBetweenEnemies = 0.7f;
		private float _enemyHealthIncrement = 0f;
		private float _moneyAmountPerKill = 10f;
		private float _bossHealth = 1500f;

		private bool _isWaveComplete = true;
		private bool _shouldSpawnBossNextWave = false;
		private bool _isGameOver = false;
		
		private int _waveNumber = 0;
		private int _numberOfEnemiesInWave = 2;

		private EnemyComponent[] _enemiesPool;

		private UIManager _uiManagerInstance;
		private GameSceneManager _gameSceneManagerInstance;

		private List<EnemyComponent> _activeEnemies;
		private Coroutine _startWaveCoroutine;

		#region Lifecycle

		private void Awake() {
			if (!Instance) {
				Instance = this;
			}
			this._activeEnemies = new List<EnemyComponent>();
			this._enemiesPool = new EnemyComponent[this._enemyPoolSize];

			for (int i = 0; i < this._enemyPoolSize; i++) {
				this._enemiesPool[i] = Instantiate(this._enemyComponentPrefab, this.transform.position, this.transform.rotation);
				EnemyComponent enemyComponent = this._enemiesPool[i];
				enemyComponent.gameObject.SetActive(false);
				enemyComponent.OnDeath += ()=> {
					if (this._isGameOver) {
						return;
					}
					PlayerInventory.Instance.AddMoney(this._moneyAmountPerKill + this._moneyAmountPerKill * this._numberOfEnemiesInWave/4);
					this._activeEnemies.Remove(enemyComponent);
					this.CheckIfWaveIsComplete();
				};
			}
		}

		private void Start() {
			this._uiManagerInstance = UIManager.Instance;
			this._gameSceneManagerInstance = GameSceneManager.Instance;
			
			this._gameSceneManagerInstance.OnGameRetry += this.HandleOnGameRetry;
			this._gameSceneManagerInstance.OnGameOver += this.HandleGameOver;
		}

		private void Update() {
			if (this._isWaveComplete) {
				if (this._countdown <= 0f) {
					this.SpawnWave();
					this._countdown = this._timeBetweenWaves;
					this._isWaveComplete = false;
				}
				this._uiManagerInstance.SetCountDownText(Mathf.FloorToInt(this._countdown + 1));
				this._countdown -= Time.deltaTime;	
			}
		}

		#endregion
		
		#region Public
		
		public EnemyComponent[] GetActiveEnemies() {
			return this._activeEnemies.ToArray();
		}
		
		#endregion

		#region Private

		private void SpawnWave() {
			this._waveNumber++;
			this._uiManagerInstance.SetWaveNumberText(this._waveNumber);
			this._uiManagerInstance.HideNextWaveCountdownTimer();
			this._startWaveCoroutine = StartCoroutine(this.SpawnEnemiesFromPool(this._numberOfEnemiesInWave));
		}

		private IEnumerator SpawnEnemiesFromPool(int enemiesToSpawn) {
			if (this._shouldSpawnBossNextWave) {
				this.SpawnBoss();
			} else {
				for (int i = 0; i < enemiesToSpawn; i++) {
					EnemyComponent enemyComponent = this._enemiesPool[i];
					enemyComponent.gameObject.SetActive(true);
					enemyComponent.SetHealth(enemyComponent.GetHealth() + this._enemyHealthIncrement);
					enemyComponent.StartEnemyMovement();
					this._activeEnemies.Add(enemyComponent);
				
					yield return new WaitForSeconds(this._timeBetweenEnemies);
				}	
			}
		}
		private void SpawnBoss() {
			EnemyComponent bossEnemyComponent = this._enemiesPool[0];
			bossEnemyComponent.gameObject.SetActive(true);
			bossEnemyComponent.MakeBoss(this._bossHealth);
			this._bossHealth += 750f;
			bossEnemyComponent.StartEnemyMovement();
			this._activeEnemies.Add(bossEnemyComponent);
			this._shouldSpawnBossNextWave = false;
		}

		private void CheckIfWaveIsComplete() {
			if (this._activeEnemies.Count == 0) {
				if (this._waveNumber == 10) {
					this._gameSceneManagerInstance.ExecuteGameOver(true);
					return;
				}
				
				this._isWaveComplete = true;
				this._uiManagerInstance.ShowNextWaveCountdownTimer();
				
				if (this._waveNumber % 3 == 0) {
					this._shouldSpawnBossNextWave = true;
				} else {
					this._numberOfEnemiesInWave++;
					this._enemyHealthIncrement += 20;
					this._moneyAmountPerKill += 10;
				}
			}
		}
		
		#region GameStateHandlers

		private void HandleGameOver(bool isWon) {
			Debug.Log("GameOVerr");
			this._isGameOver = true;
			if (this._startWaveCoroutine != null) {
				StopCoroutine(this._startWaveCoroutine);	
			}
		}

		private void HandleOnGameRetry() {
			this._waveNumber = 0;
			this._numberOfEnemiesInWave = 3;
			this._uiManagerInstance.SetWaveNumberText(this._waveNumber);
			this._isWaveComplete = true;
			this._uiManagerInstance.ShowNextWaveCountdownTimer();
			PlayerInventory.Instance.ResetMoneyAmount();
			this._isGameOver = false;
			int activeEnemiesCount = this._activeEnemies.Count;
			
			for (int i = 0; i < activeEnemiesCount; i++) {
				this._activeEnemies[0].ResetProperties();
				this._activeEnemies.Remove(this._activeEnemies[0]);
			}
		}
		#endregion
		
		#endregion
	}
}