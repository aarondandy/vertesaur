using System;

namespace Vertesaur
{

	/// <summary>
	/// A placeholder comparer for platforms that do not have this class in the framework.
	/// </summary>
	public class Comparer
	{

		/// <summary>
		/// The default implementation.
		/// </summary>
		public static Comparer Default { get; private set; }

		static Comparer(){
			Default = new Comparer();
		}


		/// <summary>
		/// Compares two objects.
		/// </summary>
		/// <param name="a">An object.</param>
		/// <param name="b">An object.</param>
		/// <returns>The comparison result.</returns>
		public int Compare(object a, object b){
			var aComparable = a as IComparable;
			if(null == aComparable)
				throw new NotSupportedException();

			return aComparable.CompareTo(b);
		}


	}

}