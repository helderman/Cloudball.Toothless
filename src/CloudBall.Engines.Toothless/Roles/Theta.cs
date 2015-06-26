using Common;
using System;

namespace CloudBall.Engines.Toothless.Roles
{
	public struct Theta : IComparable, IComparable<Theta>
	{
		private float theta;

		public Theta(float theta)
		{
			this.theta = theta;
		}

		public Theta Abs() { return Math.Abs(theta);}

		#region IComparable

		public int CompareTo(Theta other)
		{
			return theta.CompareTo(other.theta);
		}

		public int CompareTo(object obj)
		{
			return CompareTo((ThetaRange)obj);
		}
		#endregion

		public static Theta operator -(Theta t) { return new Theta(-t.theta); }
		public static implicit operator float(Theta t) { return t.theta; }
		public static implicit operator Theta(float f) { return new Theta(f); }
		public static implicit operator Theta(double d) { return new Theta((float)d); }

		public override string ToString()
		{
			return String.Format("{0:0.0#}°", theta * 180f / (float)Math.PI);
		}

		/// <summary>Gets the angle between this and the other velocity.</summary>
		public static Theta Create(Vector v0, Vector v1)
		{
			var a = v0.X * v1.X + v0.Y * v1.Y;
			var b = v0.Length * v1.Length;
			return Math.Acos(a / b);
		}

		public static Theta Create(Vector vector)
		{
			return Math.Atan(vector.Y / vector.X);
		}
	}
}
