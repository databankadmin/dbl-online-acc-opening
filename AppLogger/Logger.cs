using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;

namespace AppLogger
{
	public class Logger
	{
		private static readonly object Locker;

		public static readonly Logger Instance;

		private static string _infoLogDirectoryStatic;

		private static string _errorLogDirectoryStatic;

		private static string _warningLogDirectoryStatic;

		private static string _appName;

		public static bool VerboseLogging
		{
			get;
			set;
		}

		static Logger()
		{
			Logger.Locker = new object();
			Logger.Instance = new Logger();
			Logger._errorLogDirectoryStatic = ConfigurationManager.AppSettings["ERROR_LOG_DIRECTORY"];
			Logger._infoLogDirectoryStatic = ConfigurationManager.AppSettings["INFO_LOG_DIRECTORY"];
			Logger._warningLogDirectoryStatic = ConfigurationManager.AppSettings["WARNING_LOG_DIRECTORY"];
			Logger._appName = ConfigurationManager.AppSettings["APP_NAME"];
		}

		public Logger()
		{
		}

		private void CheckDir(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		private static void CheckDirStatic(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		public void logError(Exception x)
		{
			try
			{
				string str = Logger._errorLogDirectoryStatic;
				DateTime now = DateTime.Now;
				string dir = Path.Combine(str, now.ToString("yyyy_MM_dd"));
				this.CheckDir(dir);
				now = DateTime.Now;
				dir = Path.Combine(dir, now.ToString("HH"));
				this.CheckDir(dir);
				string message = string.Concat(x.ToString(), "\r\n", x.StackTrace, "\r\n----------END----------");
				string str1 = Logger._appName;
				now = DateTime.Now;
				using (StreamWriter sw = new StreamWriter(Path.Combine(dir, string.Format("{0}_error_{1}_{2}.log", str1, now.ToString("HHmmss"), (new Random()).Next(100000, 999999))), true))
				{
					sw.WriteLine(message);
				}
			}
			catch
			{
			}
		}

		public void logError(string msisdn, Exception x)
		{
			try
			{
				string str = Logger._errorLogDirectoryStatic;
				DateTime now = DateTime.Now;
				string dir = Path.Combine(str, now.ToString("yyyy_MM_dd"));
				this.CheckDir(dir);
				now = DateTime.Now;
				dir = Path.Combine(dir, now.ToString("HH"));
				this.CheckDir(dir);
				string message = string.Concat(x.ToString(), "\r\n", x.StackTrace, "\r\n----------END----------");
				object[] objArray = new object[] { msisdn, Logger._appName, null, null };
				now = DateTime.Now;
				objArray[2] = now.ToString("HHmmss");
				objArray[3] = (new Random()).Next(100000, 999999);
				using (StreamWriter sw = new StreamWriter(Path.Combine(dir, string.Format("{0}_{1}_error_{2}_{3}.log", objArray)), true))
				{
					sw.WriteLine(message);
				}
			}
			catch
			{
			}
		}

		public void logError(string msisdn, string message)
		{
			try
			{
				string str = Logger._errorLogDirectoryStatic;
				DateTime now = DateTime.Now;
				string dir = Path.Combine(str, now.ToString("yyyy_MM_dd"));
				this.CheckDir(dir);
				now = DateTime.Now;
				dir = Path.Combine(dir, now.ToString("HH"));
				this.CheckDir(dir);
				message = string.Concat(message, "\r\n--------------------");
				object[] objArray = new object[] { msisdn, Logger._appName, null, null };
				now = DateTime.Now;
				objArray[2] = now.ToString("HHmmss");
				objArray[3] = (new Random()).Next(100000, 999999);
				using (StreamWriter sw = new StreamWriter(Path.Combine(dir, string.Format("{0}_{1}_Error_{2}_{3}.log", objArray)), true))
				{
					sw.WriteLine(message);
				}
			}
			catch
			{
			}
		}

		public void logInfo(string message)
		{
			try
			{
				string str = Logger._infoLogDirectoryStatic;
				DateTime now = DateTime.Now;
				string dir = Path.Combine(str, now.ToString("yyyy_MM_dd"));
				this.CheckDir(dir);
				now = DateTime.Now;
				dir = Path.Combine(dir, now.ToString("HH"));
				this.CheckDir(dir);
				message = string.Concat(message, "\r\n--------------------");
				now = DateTime.Now;
				using (StreamWriter sw = new StreamWriter(Path.Combine(dir, string.Format("CLF_Info_{0}_{1}.log", now.ToString("HHmmss"), (new Random()).Next(100000, 999999))), true))
				{
					sw.WriteLine(message);
				}
			}
			catch
			{
			}
		}

		public void logInfo(string msisdn, string message)
		{
			try
			{
				string str = Logger._infoLogDirectoryStatic;
				DateTime now = DateTime.Now;
				string dir = Path.Combine(str, now.ToString("yyyy_MM_dd"));
				this.CheckDir(dir);
				now = DateTime.Now;
				dir = Path.Combine(dir, now.ToString("HH"));
				this.CheckDir(dir);
				message = string.Concat(message, "\r\n--------------------");
				now = DateTime.Now;
				using (StreamWriter sw = new StreamWriter(Path.Combine(dir, string.Format("{0}_CLF_Info_{1}_{2}.log", msisdn, now.ToString("HHmmss"), (new Random()).Next(100000, 999999))), true))
				{
					sw.WriteLine(message);
				}
			}
			catch
			{
			}
		}

		public void logWarning(string message)
		{
			lock (Logger.Locker)
			{
				try
				{
					string str = Logger._warningLogDirectoryStatic;
					DateTime now = DateTime.Now;
					string dir = Path.Combine(str, now.ToString("yyyy_MM_dd"));
					Logger.CheckDirStatic(dir);
					now = DateTime.Now;
					dir = Path.Combine(dir, now.ToString("HH"));
					Logger.CheckDirStatic(dir);
					message = string.Concat(message, "\r\n--------------------");
					string str1 = DateTime.Now.ToString("HHmmssfff");
					object obj = (new Random()).Next(100000, 999999);
					now = DateTime.Now;
					using (StreamWriter sw = new StreamWriter(Path.Combine(dir, string.Format("{2}_CLF_Warning_{0}_{1}.log", str1, obj, now.ToString("HHmmssfff"))), true))
					{
						sw.WriteLineAsync(message);
					}
				}
				catch
				{
				}
			}
		}
	}
}