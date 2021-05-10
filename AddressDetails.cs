using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NLog;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookFileIO
{
    [Serializable]
    class AddressDetails 
    {
        [NonSerialized]
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        string nameOfAddressBook;

        // Constants
        const string ADD_CONTACT = "add";
        const string UPDATE_CONTACT = "update";
        const string SEARCH_CONTACT = "search";
        const string REMOVE_CONTACT = "remove";
        const string GET_ALL_CONTACTS = "view";

        // Collection Decleration
        public Dictionary<string, Address> addressBookList = new Dictionary<string, Address>();
        public static Dictionary<string, List<Person>> cityToContactMap = new Dictionary<string, List<Person>>();
        public static Dictionary<string, List<Person>> stateToContactMap = new Dictionary<string, List<Person>>();
        private Dictionary<string, List<Person>> cityToCOntactMapInstance;
        private Dictionary<string, List<Person>> stateToContactMapInstance;

        /// <summary>
        /// Gets the address book.
        /// </summary>
        /// <returns></returns>
        private Address GetAddressBook()
        {
            Console.WriteLine("\nEnter name of Address Book to be accessed or to be added");
            nameOfAddressBook = Console.ReadLine();

            // search for address book in dictionary
            if (addressBookList.ContainsKey(nameOfAddressBook))
            {
                Console.WriteLine("\nAddressBook Identified");
                logger.Info("Address book " + nameOfAddressBook + " is accessed by user");
                return addressBookList[nameOfAddressBook];
            }

            // Offer to create a address book if not found in dictionary
            logger.Warn("AddressBook " + nameOfAddressBook + " not found");
            Console.WriteLine("\nAddress book not found. Type y to create a new address book or E to abort");

            // If user want to create a new address book
            if ((Console.ReadLine().ToLower()) == "y")
            {
                Address addressBook = new Address(nameOfAddressBook);
                addressBookList.Add(nameOfAddressBook, addressBook);
                Console.WriteLine("\nNew AddressBook Created");
                logger.Info("New address book created with name : " + nameOfAddressBook);
                return addressBookList[nameOfAddressBook];
            }

            // If User want to abort the operation 
            else
            {
                Console.WriteLine("\nAction Aborted");
                logger.Info("User aborted the operation to create new Address book with name : " + nameOfAddressBook);
                return null;
            }
        }

        /// <summary>
        /// Searches the in city.
        /// </summary>
        public void SearchInCity()
        {
            // Returns no record found if address book is empty
            if (addressBookList.Count == 0)
            {
                Console.WriteLine("\nNo record found");
                return;
            }

            // Get the name of city from user
            Console.WriteLine("\nEnter the city name to search for contact");
            string cityName = Console.ReadLine().ToLower();

            // If the city doesnt have any contacts
            if (!cityToContactMap.ContainsKey(cityName) || cityToContactMap[cityName].Count == 0)
            {
                Console.WriteLine("\nNo record found");
                return;
            }

            // Get the person name to be searched
            Console.WriteLine("\nEnter the person firstname to be searched");
            string firstName = Console.ReadLine().ToLower();
            Console.WriteLine("\nEnter the person lastname to be searched");
            string lastName = Console.ReadLine().ToLower();

            // Get the list of contacts whose city and name matches with search
            var searchResult = cityToContactMap[cityName].FindAll(contact => contact.firstName.ToLower() == firstName
                                                && contact.lastName.ToLower() == lastName);
            if (searchResult.Count == 0)
            {
                Console.WriteLine("\nNo record found");
                return;
            }
            Console.Write("\nThe contacts found in of given search are :");

            // print the list of contacts whose city and name matches with search
            searchResult.ForEach(contact => contact.toString());

        }

    }
}
