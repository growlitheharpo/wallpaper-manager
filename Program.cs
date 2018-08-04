using System;
using System.Reflection;

namespace WallpaperManager
{
	internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            int mode = -1;
	        Console.WriteLine("MENU\n");
	        Console.WriteLine("\t 1. Create a new database");
            Console.WriteLine("\t 2. Run query and copy files");
            Console.Write("\nYour choice: ");

	        Type t = typeof(WallpaperData);
	        var mems = t.GetFields(BindingFlags.Instance | BindingFlags.Public);

	        foreach (var mem in mems)
	        {
				Console.WriteLine(mem.Name);
	        }
	        
	        string input = null;
		    while (input == null ) 
			    input = Console.ReadLine();
            
	        mode = int.Parse(input);

            switch (mode)
            {
                case 1:
                    CreateDatabase.Create();
                    break;
                case 2:
                    FindFiles.DoFindFiles();
                    break;
            }
        }
    }
}
