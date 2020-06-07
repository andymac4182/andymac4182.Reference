using JetBrains.Annotations;

[assembly: AspMvcViewLocationFormat("/Features/{1}/{0}.cshtml")]
[assembly: AspMvcViewLocationFormat("/Features/Shared/{0}.cshtml")]

[assembly: AspMvcMasterLocationFormat("/Features/{1}/{0}.cshtml")]
[assembly: AspMvcMasterLocationFormat("/Features/Shared/{0}.cshtml")]

[assembly: AspMvcPartialViewLocationFormat("/Features/{1}/{0}.cshtml")]
[assembly: AspMvcPartialViewLocationFormat("/Features/Shared/{0}.cshtml")]