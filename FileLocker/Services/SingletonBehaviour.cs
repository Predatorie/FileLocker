////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Services\SingletonBehaviour.cs
//
// summary:	Implements the singleton behaviour class
////////////////////////////////////////////////////////////////////////////////////////////////////

namespace FileLocker.Services
{
    using System;

    /// <summary> The singleton behaviour. </summary>
    ///
    /// <typeparam name="T"> Type of T. </typeparam>
    public abstract class SingletonBehaviour<T>
    {
        /// <summary>
        /// Backing field for the T Instance property
        /// </summary>
        private static T instance;

        /// <summary>
        /// Gets the Instance of type T
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance != null && !instance.Equals(null))
                {
                    return instance;
                }

                instance = Activator.CreateInstance<T>();

                return instance;
            }
        }
    }
}