
using System;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// Container class to allow for arrays of multiple scriptable objects that derive from a common base class.
    /// </summary>
    [Serializable]
    public class ScriptableObjectContainer<T> where T : ScriptableObject
    {
        #region Variables

        public virtual bool UseScriptableObject
        {
            get { return _useScriptableObject; }
            set { _useScriptableObject = value; }
        }
        [SerializeField]
        bool _useScriptableObject = true;
        
        public T ScriptableObject
        {
            get { return _scriptableObject; }
            set { _scriptableObject = value; }
        }
        [SerializeField]
        T _scriptableObject;

        #endregion Variables

    }
}