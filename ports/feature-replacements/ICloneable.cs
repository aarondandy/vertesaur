// replaces a missing ICloneable interface


namespace Vertesaur.Contracts
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