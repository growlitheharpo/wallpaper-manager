using System;

namespace WallpaperManager
{
	internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var mode = -1;
	        Console.WriteLine("MENU\n");
	        Console.WriteLine("\t 1. Create a new database");
            Console.WriteLine("\t 2. Run query and copy files");
	        Console.WriteLine("\t 3. Run query and copy files (specify database)");
            Console.Write("\nYour choice: ");
	        
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
                    FindFiles.DoFindFiles(true);
                    break;
	            case 3:
		            FindFiles.DoFindFiles(false);
		            break;
            }
        }
    }
}
