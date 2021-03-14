/**
 * Created Date: 3/13/2021
 * Author: Andrei-Florin Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense.Managers {
	public class MainMenuSceneManager : MonoBehaviour {
		[SerializeField] 
		private Animator _blackScreenFadeInAnimator;

		private AsyncOperation _gameSceneLoadingAsyncOperation;
		
		#region Lifecycle

		private void Start() {
			//loads game scene but does not activate it.
			this._gameSceneLoadingAsyncOperation = SceneManager.LoadSceneAsync("GameScene");
			this._gameSceneLoadingAsyncOperation.allowSceneActivation = false;
		}

		private void Update() {
			if (Input.GetKeyDown(KeyCode.Space)) {
				StartCoroutine(this.ActivateSceneOnAnimationEnd());
			}
		}
		
		#endregion
		
		#region Private

		/// <summary>
		/// When the fade in animation for the black image overlay finished, then it activates the GameScene.
		/// </summary>
		/// <returns></returns>
		private IEnumerator ActivateSceneOnAnimationEnd() {
			this._blackScreenFadeInAnimator.Play("black_screen_fade_in");
			yield return new WaitForSeconds(this._blackScreenFadeInAnimator.GetCurrentAnimatorStateInfo(0).length);
			this._gameSceneLoadingAsyncOperation.allowSceneActivation = true;
		}
		
		#endregion
	}
}