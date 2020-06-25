<Query Kind="Program">
  <NuGetReference>Proc</NuGetReference>
</Query>

void Main()
{
	var currentDirectory = Path.GetDirectoryName(Util.CurrentQueryPath);
	var oldProjectName = "andymac4182.Reference";
	var newProjectName = "andymac4182.NEW_PROJECT_NAME";

	var replacements = new Dictionary<string, string> {
		[oldProjectName] = newProjectName
	};
	
	replacements.Dump("Strings to replace");

	ReplaceContents(currentDirectory, replacements);
	ReplaceFileNames(currentDirectory, replacements);
	ReplaceDirectoryNames(currentDirectory, replacements);
	RunDotnetRestore(currentDirectory);
	
	Util.Highlight("DONE").Dump();
}

void ReplaceContents(string baseDirectory, Dictionary<string, string> replacements)
{
	Util.WithStyle("Replacing file contents", "font-size:150%").Dump();

	var supportedExtensions = new[] { ".cshtml", ".cs", ".csproj", ".cake", ".md", ".sln", ".json" };

 	Directory
		.GetFiles(baseDirectory, "*.*", SearchOption.AllDirectories)
		.Where(file => supportedExtensions.Any(ext =>  Path.GetExtension(file) == ext))
		.ToList()
		.ForEach(file => {
			file.Dump();
			
			File.WriteAllText(
				file,
				File.ReadAllText(file).SubstituteReplacements(replacements)
			);	
		});
}

void ReplaceFileNames(string baseDirectory, Dictionary<string, string> replacements)
{
	Util.WithStyle("Replacing filename", "font-size:150%").Dump();
	
	foreach (string file in Directory.GetFiles(baseDirectory, "*.*", SearchOption.AllDirectories))
	{
		string basePath = Path.GetDirectoryName(file);
		string originalName = Path.GetFileName(file);
		string newName = originalName.SubstituteReplacements(replacements);
		
		if (originalName != newName)
		{
			$"{originalName} => {newName}".Dump();
			File.Move(file, Path.Combine(basePath, newName));
		}
	}
}

void ReplaceDirectoryNames(string baseDirectory, Dictionary<string, string> replacements)
{
	Util.WithStyle("Replacing directory name", "font-size:150%").Dump();

	foreach (string directory in Directory.GetDirectories(baseDirectory, "*.*", SearchOption.AllDirectories))
	{
		string originalName = new DirectoryInfo(directory).Name;
		string newName = originalName.SubstituteReplacements(replacements);

		if (originalName != newName)
		{
			$"{originalName} => {newName}".Dump();
			Directory.Move(directory, Path.Combine(Directory.GetParent(directory).ToString(), newName));
		}
	}
}

void RunDotnetRestore(string baseDirectory)
{
	var execArguments = new ProcNet.ExecArguments("dotnet", "restore")
	{
		WorkingDirectory = Path.Combine(baseDirectory, "src")
	};
	ProcNet.Proc.Exec(execArguments);
}

public static class Extensions
{
	public static string SubstituteReplacements(this string source, Dictionary<string, string> replacements)
	{
		foreach (var replacement in replacements)
		{
			source = source.Replace(replacement.Key, replacement.Value);
		}

		return source;
	}
}