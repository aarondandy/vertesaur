using System;

namespace Vertesaur
{

	public class Comparer
	{

		public static Comparer Default { get; private set; }

		static Comparer(){
			Default = new Comparer();
		}

		public Comparer() { }

		public int Compare(object a, object b){
			var aComparable = a as IComparable;
			if(null == aComparable)
				throw new NotSupportedException();

			return aComparable.CompareTo(b);
		}


	}

}