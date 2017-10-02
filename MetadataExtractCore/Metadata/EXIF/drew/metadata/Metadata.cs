using System;
using System.Collections.Generic;
using System.Reflection;

namespace com.drew.metadata
{
	[Serializable]

	public sealed class Metadata 
	{
        private IDictionary<Type, AbstractDirectory> directoryMap;

		/// <summary>
		/// Creates a new instance of Metadata. 
		/// </summary>
		public Metadata() : base()
		{
            this.directoryMap = new Dictionary<Type, AbstractDirectory>();
		}

		/// <summary>
		/// Creates an Iterator over the tag types set against this image, preserving the 
		/// order in which they were set.  Should the same tag have been set more than once, 
		/// it'str first position is maintained, even though the final value is used. 
		/// </summary>
		/// <returns>an Iterator of tag types set for this image</returns>
		public IEnumerator<AbstractDirectory> GetDirectoryIterator() 
		{
            return this.directoryMap.Values.GetEnumerator();
		}

		/// <summary>
		/// Gets a directory regarding its type
		/// </summary>
        /// <param name="aTypeStr">the type you are looking for</param>
		/// <returns>the directory found</returns>
		/// <exception cref="ArgumentException">if aType is not a Directory like class</exception>
		public AbstractDirectory GetDirectory(string aTypeStr) 
		{
            Type aType = Type.GetType(aTypeStr);
			if (!Type.GetType("com.drew.metadata.AbstractDirectory").IsAssignableFrom(aType)) 
			{
                throw new ArgumentException("Class type passed to GetDirectory must be an implementation of com.drew.metadata.AbstractDirectory");
			}

			// check if we've already issued this type of directory
            if (this.ContainsDirectory(aType)) 
			{
				return directoryMap[aType];
			}
            AbstractDirectory lcDirectory = null;
			try 
			{
				ConstructorInfo[] lcConstructor = aType.GetConstructors();
				lcDirectory = (AbstractDirectory) lcConstructor[0].Invoke(null);
			} 
			catch (Exception e) 
			{
				throw new SystemException(
					"Cannot instantiate provided Directory type: "
					+ aType, e);
			}
			// store the directory in case it'str requested later
			this.directoryMap.Add(aType, lcDirectory);
		
			return lcDirectory;
		}

		/// <summary>
		/// Indicates whether a given directory type has been created in this aMetadata repository.
		/// Directories are created by calling getDirectory(Class).
		/// </summary>
		/// <param name="aType">the Directory type</param>
		/// <returns>true if the aMetadata directory has been created</returns>
		public bool ContainsDirectory(Type aType) 
		{
			return this.directoryMap.ContainsKey(aType);
		}
	}
}