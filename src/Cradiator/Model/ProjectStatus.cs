namespace Cradiator.Model
{
	public class ProjectStatus
	{
		public const string BUILDING = "Building";
		public const string SUCCESS = "Success";
		public const string FAILURE = "Failure";
		public const string EXCEPTION = "Exception";

		public string Name { get; private set; }
		public string CurrentMessage { get; set; }
		public ProjectActivity ProjectActivity { private get; set; }
		public string LastBuildStatus { private get; set; }

		public ProjectStatus(string name)
		{
			Name = name;
		}

		public string CurrentState
		{
			get { return (ProjectActivity == ProjectActivity.Building) ? BUILDING : LastBuildStatus; }
		}

        public bool IsBroken
        {
            get { return LastBuildStatus == FAILURE || LastBuildStatus == EXCEPTION; }
        }

		public bool IsSuccessful
		{
			get { return LastBuildStatus == SUCCESS; }
		}

		public override bool Equals(object obj)
		{
			return ((ProjectStatus)obj).Name == Name;	// simple, but all that is needed
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}