// replaces a missing ICloneable interface


namespace System
{

	/// <summary>
	/// Interface replacement for System.ICloneable
	/// </summary>
	public interface ICloneable
	{
		/// <inheritdoc/>
		object Clone();
	}

}