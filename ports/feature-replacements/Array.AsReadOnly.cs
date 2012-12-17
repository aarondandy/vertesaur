using System.Collections.ObjectModel;

namespace Vertesaur
{
	
	public static class Array
	{
		public static ReadOnlyCollection<T> AsReadOnly<T>(T[] data){
			return new ReadOnlyCollection<T>(data);
		}
	}

}