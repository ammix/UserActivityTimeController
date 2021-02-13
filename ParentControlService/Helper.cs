using System.Net;

using RestSharp;

namespace ParentControlService
{
	static class Helper
	{
		public static string ServerUri = "http://bookkeeping.somee.com/parentcontrolsettings";

		public static UserStatus GetUserStatus(string windowsUser) //, int secondsBetweenCalls
		{
			var restClient = new RestClient(ServerUri);
			var request = new RestRequest(Method.GET).AddQueryParameter("user", windowsUser);
			var response = restClient.Get<UserStatus>(request);

			return response.StatusCode == HttpStatusCode.OK
				? response.Data
				: new UserStatus {Message = "Відсутній інтернет. Комп'ютер не може працювати без інтернету"};



			//var request = WebRequest.Create($"http://bookkeeping.somee.com/parentcontrolsettings?user={windowsUser}");
			//try
			//{
			//	var response = request.GetResponse().GetResponseStream();
			//	using (var streamReader = new StreamReader(response ?? throw new InvalidOperationException()))
			//		return JsonConvert.DeserializeObject<UserStatus>(streamReader.ReadToEnd());
			//}
			//catch (WebException)
			//{
			//	return new UserStatus { Action = WhatDoWithUser.ShowMessage, Message = "Відсутній інтернет. Комп'ютер не може працювати без інтернету" };
			//}
		}
	}

	public struct UserStatus
	{
		public WhatDoWithUser Action;
		public string Message;
	}

	public enum WhatDoWithUser
	{
		ShowMessage,
		LockScreen,
		LogOffUser
	}
}
