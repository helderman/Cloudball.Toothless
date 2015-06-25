namespace CloudBall.Engines.Toothless.Roles
{
	public static class Role
	{
		public static IRole Keeper = new Keeper();
		public static IRole CatchUp = new CatchUp();
	}
}
