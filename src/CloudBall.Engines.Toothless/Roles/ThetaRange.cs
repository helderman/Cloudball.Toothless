using System;

namespace CloudBall.Engines.Toothless.Roles
{
	public class ThetaRange : IComparable, IComparable<ThetaRange>
	{
		private Theta min;
		private Theta max;

		public ThetaRange(Theta min, Theta max)
		{
			this.min = min;
			this.max = max;
		}

		public Theta Minimum { get { return min; } }
		public Theta Maximum { get { return max; } }

		public Theta Average { get { return (min / max) / 2f; } }
		public Theta Span { get { return max - min; } }

		public override string ToString()
		{
			return String.Format("[{0}, {1}], Span: {2}", min, max, Span);
		}

		public int CompareTo(ThetaRange other)
		{
			return min.CompareTo(other.min);
		}

		public int CompareTo(object obj)
		{
			return CompareTo((ThetaRange)obj);
		}
	}
}

