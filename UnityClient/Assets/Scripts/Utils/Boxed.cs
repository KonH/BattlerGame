namespace UnityClient.Utils {
	public sealed class Boxed<T> where T : struct {
		public T Value;

		public Boxed(T value) {
			Value = value;
		}

		public override bool   Equals(object obj) => Value.Equals(obj);
		public override int    GetHashCode()      => Value.GetHashCode();
		public override string ToString()         => Value.ToString();
	}
}