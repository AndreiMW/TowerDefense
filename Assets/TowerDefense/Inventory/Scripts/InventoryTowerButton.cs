/**
 * Created Date: 3/12/2021
 * Author: Andrei-Florin Ciobanu
 * 
 * Copyright (c) 2021 Andrei-Florin Ciobanu. All rights reserved. 
 */

using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefense.Inventory.Scripts {
	public class InventoryTowerButton : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler, IPointerDownHandler {
		[SerializeField] 
		private Graphic _targetGraphic;

		[SerializeField] 
		private Color _hoverColor;

		[SerializeField] 
		private Color _notAvailableColor;

		private Color _originalColor;
		[SerializeField] 
		private CanvasGroup _notEnoughMoneyTooltip;
		
		[SerializeField] 
		private float _towerCost;
		
		public UnityEvent<PointerEventData> OnMouseDown = new UnityEvent<PointerEventData>();
		public UnityEvent<PointerEventData> OnEnter = new UnityEvent<PointerEventData>();
		public UnityEvent<PointerEventData> OnExit = new UnityEvent<PointerEventData>();

		#region Lifecycle

		private void Awake() {
			this._originalColor = this._targetGraphic.color;
			this.HideTooltip();
		}

		#endregion
		
		#region Public

		public void ShowTooltip(float availableMoney) {
			if (availableMoney < this._towerCost) {
				this._notEnoughMoneyTooltip.alpha = 1f;
				this._targetGraphic.color = this._notAvailableColor;
			}
		}
		
		public void HideTooltip() {
			this._notEnoughMoneyTooltip.alpha = 0f;
			this._targetGraphic.color = this._originalColor;
		}
		
		#endregion
		#region PointerHandling
		
		public void OnPointerEnter(PointerEventData eventData) {
			this._targetGraphic.color = this._hoverColor;
			this.OnEnter?.Invoke(eventData);
		}

		public void OnPointerExit(PointerEventData eventData) {
			this.HideTooltip();
			this._targetGraphic.color = this._originalColor;
			this.OnExit?.Invoke(eventData);
		}

		public void OnPointerDown(PointerEventData eventData) {
			this.OnMouseDown?.Invoke(eventData);
		}

		#endregion
	}
}