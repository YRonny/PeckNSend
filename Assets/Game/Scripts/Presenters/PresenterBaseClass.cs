using System;
using System.ComponentModel;
using UnityEngine;
using PeckNSend.Models;

namespace PeckNSend.Presenters
{
    public abstract class PresenterBaseClass<T> : MonoBehaviour
    where T : UnityModelBaseClass
    {
		private T _model;
		public virtual T Model
		{
			get { return _model; }
			set
			{
				T previousModel = null;
				if (_model == value) return;
				if (_model != null)
				{
					_model.PropertyChanged -= Model_PropertyChanged;
					previousModel = _model;
				}
				_model = value;
				_model.PropertyChanged += Model_PropertyChanged;
				ModelSetInitialization(previousModel);

			}
		}

        protected abstract void Model_PropertyChanged(object sender, PropertyChangedEventArgs e);

		protected virtual void ModelSetInitialization(T previousModel)
		{
		}

		protected virtual void Update()
		{
			Model?.Update(Time.deltaTime);
		}
		protected virtual void FixedUpdate()
		{
			Model?.FixedUpdate(Time.fixedDeltaTime);
		}
	}
}
