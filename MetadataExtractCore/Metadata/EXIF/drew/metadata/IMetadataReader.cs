namespace com.drew.metadata
{
	/// <summary>
	/// This interface represents a Metadata reader object
	/// </summary>

	public interface IMetadataReader
	{
		/// <summary>
		/// Extracts aMetadata
		/// </summary>
		/// <returns>the aMetadata found</returns>
		Metadata Extract();

		/// <summary>
		/// Extracts aMetadata
		/// </summary>
		/// <param name="aMetadata">where to add aMetadata</param>
		/// <returns>the aMetadata found</returns>
		Metadata Extract(Metadata aMetadata);
	}
}
