using UnityEngine;

///<summary>Class for generic singleton</summary>
public class Singleton<T> : MonoBehaviour where T: MonoBehaviour 
{
	static T _instance;

    ///<summary>Instance used to refer to Singleton (e.g. MyClass.Instance)</summary>
	public static T Instance
	{
		get 
		{
            // if no instance is found, find the first GameObject of type T
			if (_instance == null) 
			{
				_instance = GameObject.FindObjectOfType<T> ();

                // if no instance exists in the Scene, create a new GameObject and add the Component T 
				if (_instance == null) 
				{
					GameObject singleton = new GameObject (typeof(T).Name);
					_instance = singleton.AddComponent<T> ();
				}
			}
			return _instance;
		}
	}

	public virtual void Awake()
	{
		if (_instance == null) 
		{
			_instance = this as T;

            //uncomment DontDestroyOnLoad line to persist on Level loads
            //DontDestroyOnLoad (this.gameObject);
        }
        else 
		{
			Destroy (gameObject);
		}
	}
}
