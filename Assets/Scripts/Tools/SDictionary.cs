using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SDictionary<T1,T2> : Dictionary<T1,T2>
{
	new public T2 this [T1 key] {
		set { 
			if (ContainsKey (key)) {
				if (value == null)
					base.Remove (key);
				else
					base [key] = value;
			} else
				base.Add (key, value);
		}
		get {
			if (ContainsKey (key))
				return base [key];
			return default (T2);
		}
	}

	public SDictionary () : base()
	{
	}
	
	public SDictionary (IDictionary<T1, T2> dictionary) : base( dictionary, null )
	{
	}

	List<T1> needRemove = new List<T1> ();

	public void RemoveFromCondition (System.Func<T2,bool> condition = null)
	{
		needRemove.Clear ();
		foreach (var kvp in this) {
			if (condition (kvp.Value)) {
				needRemove.Add (kvp.Key);
			}
		}
		needRemove.ForEach (delegate(T1 key) {
			Remove (key);
		});
	}

	public void RemoveFromCondition (System.Func<T1,bool> condition = null)
	{
		needRemove.Clear ();
		foreach (var kvp in this) {
			if (condition (kvp.Key)) {
				needRemove.Add (kvp.Key);
			}
		}
		needRemove.ForEach (delegate(T1 key) {
			Remove (key);
		});
	}
}