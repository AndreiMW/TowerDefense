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
		private EnemyComponent[] _enemiesPool;
		private List<EnemyComponent> _activeEnemies;

		[SerializeField] 
		private int _enemyPoolSize;
		
		private int _waveNumber = 0;
		private int _numberOfEnemiesInWave = 2;
		
		[SerializeField] 
		private float _timeBetweenWaves;
		
		private float _countdown = 8f;
		private float _timeBetweenEnemies = 0.7f;
		private float _enemyHealthIncrement = 0f;
		private float _moneyAmountPerKill = 10f;
		private float _bossHealth = 1500f;

		private bool _isWaveComplete = true;
		private bool _shouldSpawnBoss = false;
		private bool _isGameOver = false;

		private UIManager _uiManagerInstance => UIManager.Instance;

		private GameSceneManager _gameSceneManagerInstance => GameSceneManager.Instance;

		private Coroutine _startWaveCoroutine;

		#region Lifecycle

		private void Awake() {
			if (!Instance) {
				Instance = this;
			}
			this._activeEnemies = new List<EnemyComponent>();
			this._enemiesPool = new EnemyComponent[this._enemyPoolSize];

			//instantiate enemy pool
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
		
		/// <summary>
		/// Get the list of active enemies.
		/// </summary>
		/// <returns>Array of active enemies.</returns>
		public EnemyComponent[] GetActiveEnemies() {
			return this._activeEnemies.ToArray();
		}
		
		#endregion

		#region Private

		/// <summary>
		/// Spawn a wave.
		/// </summary>
		private void SpawnWave() {
			this._waveNumber++;
			this._uiManagerInstance.SetWaveNumberText(this._waveNumber);
			this._uiManagerInstance.HideNextWaveCountdownTimer();
			this._startWaveCoroutine = StartCoroutine(this.ActivateEnemiesFromPool(this._numberOfEnemiesInWave));
		}

		/// <summary>
		/// Activate enemies from pool.
		/// </summary>
		/// <param name="numberOfEnemiesToActivate">Number of enemies to activate</param>
		/// <returns></returns>
		private IEnumerator ActivateEnemiesFromPool(int numberOfEnemiesToActivate) {
			if (this._shouldSpawnBoss) {
				this.ActivateBossEnemy();
			} else {
				for (int i = 0; i < numberOfEnemiesToActivate; i++) {
					EnemyComponent enemy = this._enemiesPool[i];
					this.ActivateEnemy(enemy, false);
					yield return new WaitForSeconds(this._timeBetweenEnemies);
				}	
			}
		}
		
		/// <summary>
		/// Activates a boss enemy.
		/// </summary>
		private void ActivateBossEnemy() {
			EnemyComponent bossEnemyComponent = this._enemiesPool[0];
			this.ActivateEnemy(bossEnemyComponent,true);
		}

		/// <summary>
		/// Checks if the wave is completed when an enemy dies.
		/// </summary>
		private void CheckIfWaveIsComplete() {
			if (this._activeEnemies.Count == 0) {
				if (this._waveNumber == 10) {
					this._gameSceneManagerInstance.ExecuteGameOver(true);
					return;
				}
				
				this._isWaveComplete = true;
				this._uiManagerInstance.ShowNextWaveCountdownTimer();
				
				if (this._waveNumber % 3 == 0) {
					this._shouldSpawnBoss = true;
				} else {
					this._numberOfEnemiesInWave++;
					this._enemyHealthIncrement += 20;
					this._moneyAmountPerKill += 10;
				}
			}
		}
		
		#region GameStateHandlers

		/// <summary>
		/// Logic to execute if the game is over.
		/// </summary>
		/// <param name="isWon">If game was won or lost.</param>
		private void HandleGameOver(bool isWon) {
			this._isGameOver = true;
			if (this._startWaveCoroutine != null) {
				StopCoroutine(this._startWaveCoroutine);	
			}
		}

		/// <summary>
		/// Logic to execute when the user presses the retry button.
		/// </summary>
		private void HandleOnGameRetry() {
			this._waveNumber = 0;
			this._numberOfEnemiesInWave = 3;
			
			this._isWaveComplete = true;
			this._isGameOver = false;
			
			this._uiManagerInstance.SetWaveNumberText(this._waveNumber);
			this._uiManagerInstance.ShowNextWaveCountdownTimer();
			PlayerInventory.Instance.ResetMoneyAmount();
			
			int activeEnemiesCount = this._activeEnemies.Count;
			for (int i = 0; i < activeEnemiesCount; i++) {
				this._activeEnemies[0].ResetProperties();
				this._activeEnemies.Remove(this._activeEnemies[0]);
			}
		}

		/// <summary>
		/// Activates an enemy from the enemy pool.
		/// </summary>
		/// <param name="enemy">The enemy to activate.</param>
		/// <param name="isBoss">Is the enemy a boss?</param>
		private void ActivateEnemy(EnemyComponent enemy, bool isBoss) {
			enemy.gameObject.SetActive(true);
			enemy.StartEnemyMovement();
			
			this._activeEnemies.Add(enemy);

			if (isBoss) {
				enemy.MakeBoss(this._bossHealth);
				this._bossHealth += 750f;
				this._shouldSpawnBoss = false;
			} else {
				enemy.SetHealth(enemy.GetHealth() + this._enemyHealthIncrement);
			}
		}
		
		#endregion
		
		#endregion
	}
}