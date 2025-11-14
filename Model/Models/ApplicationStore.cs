namespace Model.Models
{
	public class ApplicationStore
	{
		private PartialUser loggedInEmployee;
		private static ApplicationStore ApplicationStoreInstance;

		public static ApplicationStore GetInstance()
		{
			if (ApplicationStoreInstance == null)
				ApplicationStoreInstance = new ApplicationStore();

			return ApplicationStoreInstance;
		}

		public PartialUser getLoggedInUser()
		{
			return loggedInEmployee;
		}

		public void setLoggedInUser(PartialUser employee)
		{
			loggedInEmployee = employee;
		}
	}
}
