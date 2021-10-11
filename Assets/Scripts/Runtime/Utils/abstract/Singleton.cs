using UnityEngine;

namespace HuntroxGames.Utils
{
	public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		protected static T _instance;
		public static T instance
		{
			get
			{
				if (!_instance)
				{
					_instance = FindObjectOfType<T>();

					if (!_instance)
					{
						_instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
					}
				}

				return _instance;
			}
		}

		public static bool HasInstance
		{
			get
			{
				return _instance != null;
			}
		}

		protected virtual void Awake()
		{
			if (_instance == null)
			{
				_instance = (T)this;

#if UNITY_EDITOR
				if (UnityEditor.EditorApplication.isPlaying)
#endif
					DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}