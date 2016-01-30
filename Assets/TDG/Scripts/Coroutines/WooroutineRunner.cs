using System;
using System.Collections;
using UnityEngine;

namespace Wooga.Coroutines
{
	public class WooroutineRunner : MonoBehaviour 
	{
		static WooroutineRunner _coroutineRunner;
		static WooroutineRunner Instance {
			get {
				if (_coroutineRunner == null) {
					_coroutineRunner = new GameObject("WooroutineRunner").AddComponent<WooroutineRunner>();
					DontDestroyOnLoad(_coroutineRunner);
					_coroutineRunner.hideFlags = HideFlags.DontSaveInEditor;
					#if UNITY_EDITOR
					{
						UnityEditor.EditorApplication.playmodeStateChanged += () => {
							if (_coroutineRunner && !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) {
								DestroyImmediate(_coroutineRunner.gameObject);
								_coroutineRunner = null;								
							}
						};

					}
					#endif
				}
				return _coroutineRunner;
			}
		}

		public static Wooroutine<T> StartRoutine<T> (IEnumerator routineWithReturnValue, Action<Wooroutine<T>> onComplete = null)
		{			
			return new Wooroutine<T>(routineWithReturnValue, onComplete).Start();
		}

		public static Coroutine StartRoutine (IEnumerator routine, Action onComplete = null)
		{
			return Instance.StartRoutineInternal(routine, onComplete);
		}

		protected Coroutine StartRoutineInternal (IEnumerator routine, Action onComplete = null)
		{
			var coroutine = StartCoroutine(routine);

			if (onComplete != null) {
				StartCoroutine(onCompleteRoutine(coroutine, onComplete));
			}

			return coroutine;
		}

		public static void StopRoutine (Coroutine coroutine)
		{
			Instance.StopCoroutine(coroutine);
		}

		public static void PrepareForReset()
		{
			Instance.StopAllCoroutines ();
			DestroyImmediate (_coroutineRunner);
			_coroutineRunner = null;
		}

		IEnumerator onCompleteRoutine (Coroutine routine, Action onComplete)
		{
			yield return routine;
			if (onComplete != null) {
				onComplete();				
			}
		}

		void OnDestroy() {
			_coroutineRunner = null;
		}
	}
}
