using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace EditClipboardText
{
	class Program
	{
		enum ExitCode { Success = 0, ShowSyntax = 1 };
		
		[STAThread]
		static int Main(string[] args)
		{ return (int)RunMain(args); }

		static ExitCode RunMain(string[] args)
		{
			// Verify command line arguments
			if (args.Length > 0 && !args[0].Equals("/editor", StringComparison.OrdinalIgnoreCase)) return ShowSyntax();

			// Save text and open it
			string tmptxt = Path.ChangeExtension(Path.GetTempFileName(), ".txt");
			File.WriteAllText(tmptxt, Clipboard.GetText());

			// If an editor was specified use it, otherwise use default
			string editor = null;
			if (args.Length > 1 && args[0].Equals("/editor", StringComparison.InvariantCultureIgnoreCase) && File.Exists(args[1])) editor = args[1];
			//else { editor = Environment.ExpandEnvironmentVariables("%edit%"); if (!File.Exists(editor)) editor = null; } 
			//if (String.IsNullOrEmpty(editor)) editor = Path.Combine(Environment.ExpandEnvironmentVariables("%system%"), "notepad.exe");
			if (editor != null) Process.Start(editor, tmptxt);
			else Process.Start(tmptxt);

			return ExitCode.Success;
		}

		static ExitCode ShowSyntax()
		{
			// Display the usage syntax
			MessageBox.Show(AppSyntax, "EditClipboardText");
			return ExitCode.ShowSyntax;
		}

		static string AppSyntax
		{
			get
			{
				Assembly a = Assembly.GetExecutingAssembly();
				return new StreamReader(a.GetManifestResourceStream(a.GetName().Name + ".Syntax.txt")).ReadToEnd();
			}
		}
	}
}
