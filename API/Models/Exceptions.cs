using System;
namespace API.Models
{
  public class AddMemberException: Exception
  {
    public string ResourceReferenceProperty { get; set; }
 
	  public AddMemberException(){}

	  public AddMemberException(string message)
	      : base(message){}

	  public AddMemberException(string message, Exception inner)
	      : base(message, inner){}
	}
	public class GetMemberException : Exception
	{
		public string ResourceReferenceProperty { get; set; }

		public GetMemberException() { }

		public GetMemberException(string message)
			: base(message) { }

		public GetMemberException(string message, Exception inner)
			: base(message, inner) { }
	}
	public class RemoveMemberException : Exception
	{
		public string ResourceReferenceProperty { get; set; }

		public RemoveMemberException() { }

		public RemoveMemberException(string message)
		  : base(message) { }

		public RemoveMemberException(string message, Exception inner)
		  : base(message, inner) { }
	}
	public class UpdateMemberException : Exception
	{
		public string ResourceReferenceProperty { get; set; }

		public UpdateMemberException() { }

		public UpdateMemberException(string message)
		  : base(message) { }

		public UpdateMemberException(string message, Exception inner)
		  : base(message, inner) { }
	}
}
