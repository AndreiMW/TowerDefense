/**
 * Created Date: 3/11/2021
 * Author: Andrei Ciobanu
 * 
 * Copyright (c) 2021 ATiStudios. All rights reserved. 
 */

using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.Enemy.Scripts {
	public class HealthBar : MonoBehaviour {
		[SerializeField] 
		private Image _healthProgressImage;
		
		[SerializeField] 
		private Color _fullHpColor = Color.green;
		
		[SerializeField] 
		private Color _halfHpColor = Color.yellow;
		
		[SerializeField] 
		private Color _lowHpColor = Color.red;
		
		#region Public

		public void SetHealth(float health) {
			this._healthProgressImage.fillAmount = health / 100;
			this.ChangeHealthBarColor(health);
		}
		
		#endregion
		
		#region Private

		private void ChangeHealthBarColor(float health) {
			if (health <= 25) {
				this._healthProgressImage.color = this._lowHpColor;
			} else {
				if (health > 25 && health <= 75) {
					this._healthProgressImage.color = this._halfHpColor;	
				} else {
					this._healthProgressImage.color = this._fullHpColor;
				}
			}
		}
		
		#endregion
	}
}