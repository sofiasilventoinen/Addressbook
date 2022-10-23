using Assignment_1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Assignment_1.Service
{
    internal interface IMenu //interface som innehåller de metoder jag använder i mitt program
    {
        public void MenuOptions();

        public void CreateNewContact();

        public void ViewContactList();

        public void ViewContactDetails(Guid id);

        public void ViewUpdatedContactList(Contact contact);

        public void RemoveContact(Guid id);
    }

    internal class MenuService : IMenu
    {
        private IFileService _fileService = new FileService();
        private List<Contact> _contacts = new();
        private string _filepath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\file.json"; //Standardsökväg


        public void MenuOptions()
        {
            //Min meny som beroende på användarens val skickar denna vidare till olika case med metoder
            Console.Clear();
            Console.WriteLine("------ Contact List ------");
            Console.WriteLine("1. View Contact List");
            Console.WriteLine("2. Create New Contact");
            Console.WriteLine("3. Exit");
            Console.Write("Choose one option: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ViewContactList();
                    break;

                case "2":
                    CreateNewContact();
                    break;

                case "3":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid option selected");
                    break;
            }
        }

        //Finns det kontakter i listan kommer _contacts-listan att visas
        public void ViewContactList()
        {

            try { _contacts = JsonConvert.DeserializeObject<List<Contact>>(_fileService.Read(_filepath)); }
            catch { }

            Console.Clear();
            Console.WriteLine(" ---------- View Contact List ---------- ");

            foreach (var contact in _contacts)
                Console.WriteLine($"{contact.Id} \t {contact.FirstName} {contact.LastName}");

            //Om det finns fler produkter än 0 körs if-satsen
            if (_contacts.Count > 0)
            {
                Console.WriteLine();
                Console.Write("View contact details?  (y/n) ");
                var option = Console.ReadLine();

                if (option?.ToLower() == "y")
                {
                    Console.Write("Enter contact id: ");
                    var id = Guid.Parse(Console.ReadLine());
                    ViewContactDetails(id);
                }
            }
        }

        public void ViewContactDetails(Guid id)
        {
            //Här visas kontaktdetaljer där användaren kan välja två st alternativ som jag placerat i en switch med mina metoder
            var contact = _contacts.FirstOrDefault(x => x.Id == id);

            Console.Clear();
            Console.WriteLine(" ------- Contact Details ------ ");
            Console.WriteLine();
            Console.WriteLine($"Id: \t\t {contact?.Id}");
            Console.WriteLine($"Firstname: \t {contact?.FirstName}");
            Console.WriteLine($"Lastname: \t {contact?.LastName}");
            Console.WriteLine($"E-mail: \t {contact?.Email}");
            Console.WriteLine();
            Console.WriteLine("1. Edit Contact");
            Console.WriteLine("2. Delete Contact");
            Console.Write("Enter Option: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ViewUpdatedContactList(contact);
                    break;

                case "2":
                    RemoveContact(contact.Id);
                    break;

                default:
                    Console.WriteLine("Invalid option selected");
                    break;
            }
        }

        public void CreateNewContact()
        {
            try
            {
                var contact = new Contact();

                Console.Clear();
                Console.WriteLine(" ---------- Create New Contact ---------- ");
                Console.WriteLine();
                Console.Write("Firstname: ");
                contact.FirstName = Console.ReadLine() ?? "";
                Console.Write("Lastname :");
                contact.LastName = Console.ReadLine() ?? "";
                Console.Write("Email: ");
                contact.Email = Console.ReadLine() ?? "";

                _contacts.Add(contact); //Lägger till kontakt i lista
                _fileService.Save(_filepath, JsonConvert.SerializeObject(_contacts)); //Sparar kontakt i lista
            }
            catch
            {
                Console.WriteLine("Invalid values added");
            }
        }

        public void ViewUpdatedContactList(Contact contact)
        {
            //Vill användaren updatera kontaktuppgifterna görs det här och sparas i listan
            var index = _contacts.IndexOf(contact);

            Console.Clear();
            Console.WriteLine(" -------Update Contact ------- ");
            Console.WriteLine();

            Console.WriteLine($"Id: \t\t {contact?.Id}");
            Console.WriteLine($"Firstname: \t {contact?.FirstName}");
            Console.WriteLine($"Lastname: \t {contact?.LastName}");
            Console.WriteLine($"Email: \t\t {contact?.Email}");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("Firstname: ");
            contact.FirstName = Console.ReadLine();
            Console.Write("Lastname: ");
            contact.LastName = Console.ReadLine();
            Console.Write("Email: ");
            contact.Email = Console.ReadLine();

            _contacts[index] = contact;
            _fileService.Save(_filepath, JsonConvert.SerializeObject(_contacts));
        }

        public void RemoveContact(Guid id)
        {
            //Metoden tar bort kontakt och sparar ändringen i listan
            _contacts = _contacts.Where(x => x.Id != id).ToList();
            _fileService.Save(_filepath, JsonConvert.SerializeObject(_contacts));

        }
    }
}
