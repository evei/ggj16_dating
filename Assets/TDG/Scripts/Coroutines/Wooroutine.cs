using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Wooga.Coroutines
{
	public class Wooroutine<T> {
		public T ReturnValue {
			get {
				if (!Completed) {
					throw new Exception(typeof(Wooroutine<T>).FullName + " did not complete.");
				}
				if (_exception != null) {
					throw _exception;
				}
				return _returnValue;
			}
		}
		
		public Exception Exception { get { return _exception; } }
		
		public bool Completed { get { return _success || HasError; } }
		
		public bool Canceled { get { return _enumerator == null; } }
		
		public bool HasError { get { return _exception != null; } }
		
		public event Action<Wooroutine<T>> OnComplete {
			add {
				if (Completed) {
					value(this);
				}
				else {
					_onComplete += value;
				}
			}
			remove {
				_onComplete -= value;
			}
		}
		Action<Wooroutine<T>> _onComplete;
		
		public event Action<Wooroutine<T>> OnCanceled {
			add {
				if (_enumerator == null) {
					value(this);
				}
				else {
					_onCanceled += value;
				}
			}
			remove {
				_onCanceled -= value;
			}
		}
		Action<Wooroutine<T>> _onCanceled;
		
		IEnumerator _enumerator;
		
		Coroutine _coroutine;
		
		List<Coroutine> _awaitCoroutines;
		
		T _returnValue;
		
		bool _success = false;
		
		Exception _exception;

		public Wooroutine(IEnumerator routine, Action<Wooroutine<T>> onComplete = null)
		{			
			_enumerator = routine;
			if (onComplete != null) {
				_onComplete += onComplete;				
			}
		}
				
		public Wooroutine<T> Start()
		{
			_coroutine = WooroutineRunner.StartRoutine(WrapRoutine());
			return this;
		}
		
		public Coroutine Await ()
		{
			Assert.IsTrue(_coroutine != null || Completed, "Start must be called before awaiting coroutine");
			
			if (Completed) {
				return WooroutineRunner.StartRoutine(YieldOnCoroutine());
			}
			
			Coroutine co;
			
			if (_awaitCoroutines == null) {
				_awaitCoroutines = new List<Coroutine>();
				co = WooroutineRunner.StartRoutine(YieldOnCoroutine());
			}				
			else {
				co = WooroutineRunner.StartRoutine(YieldOnResultRoutine());
			}
			
			_awaitCoroutines.Add(co);
			
			return co;
		}
		
		IEnumerator WrapRoutine() {
			while (true) {
				try {
					if (!_enumerator.MoveNext()) {
						_returnValue = (T)_enumerator.Current;
						_success = true;
					}
				} catch (Exception ex) {
//					WoogaDebug.LogError(_enumerator.Current, typeof(T), ex);
					_exception = ex;
				}
				
				if (Completed) {
					SendComplete();
					Clear();
					yield break;
				}
				
				yield return _enumerator.Current;
			}
		}			
		
		IEnumerator YieldOnCoroutine ()
		{
			if (!Completed) {
				yield return _coroutine;
			}
		}
		
		IEnumerator YieldOnResultRoutine() 
		{
			while (!Completed) {
				yield return null;
			}
		}
		
		public void Stop ()
		{
			if (_awaitCoroutines != null) {
				foreach (var awaitRoutine in _awaitCoroutines) {
					WooroutineRunner.StopRoutine(awaitRoutine);				
				}			
			}
			if (_coroutine != null) {
				WooroutineRunner.StopRoutine(_coroutine);				
				SendCanceled();
			}
			
			Clear();
		}
		
		void SendComplete()
		{
			if (_onComplete != null) {
				_onComplete(this);
			}
		}
		
		void SendCanceled()
		{
			if (_onCanceled != null) {
				_onCanceled(this);
			}
		}
		
		void Clear()
		{
			_awaitCoroutines = null;
			_coroutine = null;
			_enumerator = null;
			_onComplete = null;
			_onCanceled = null;
		}
	}
}