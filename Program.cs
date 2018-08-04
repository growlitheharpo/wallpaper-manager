using System;

namespace WallpaperManager
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            int mode = -1;
            Console.WriteLine("MENU\n\t 1. Create a new database\n");
            Console.WriteLine("MENU\n\t 2. Run query and copy files\n");
            Console.Write("Your choice: ");
            string input = Console.ReadLine();
            mode = int.Parse(input);

            switch (mode)
            {
                case 1:
                    CreateDatabase.Create();
                    break;
                case 2:
                    FindFiles.DoFindFiles();
                    break;
                default:
                    break;
            }
        }
    }
}
