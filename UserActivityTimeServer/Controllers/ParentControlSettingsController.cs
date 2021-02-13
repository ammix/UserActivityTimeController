using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ParentControlSettings.Controllers
{
	[ApiController]
	[Route("parentcontrolsettings")]
	public class ParentControlSettingsController: ControllerBase
	{
		Dictionary<string, string> parentSettings = new Dictionary<string, string>();

		public ParentControlSettingsController()
		{
			parentSettings.Add("ProcessesToControl", "age2_x1, dmcr, SimCity 4, chrome");
			parentSettings.Add("QuotaPerDay", "1.5h");
			parentSettings.Add("ForbidTimeFrames", "21:30-23:59, 00:00-10:00");
			parentSettings.Add("StopImmediately", "false");
		}

		[HttpGet]
		public Dictionary<string, string> Get()
		{
			return parentSettings;
		}
	}
}
