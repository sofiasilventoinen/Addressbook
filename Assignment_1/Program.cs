using Assignment_1.Models;
using Assignment_1.Service;

IMenu menu = new MenuService();

do
{
    Console.Clear();
    menu.MenuOptions();
    Console.ReadKey();

} while (true);