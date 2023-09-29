using System;
using static System.Console;
using System.Drawing;
using GoldBadgeChallenge.Repository.DeliveryRepository;
using GoldBadgeChallenge.Data;
using GoldBadgeChallenge.Repository.ItemRepository;
using GoldBadgeChallenge.Repository.CustomerRepository;
using GoldBadgeChallenge.Data.Enums;
using System.Runtime.InteropServices;
public class ProgramUI
{
    private readonly DeliveryRepo _deliveryRepo = new DeliveryRepo();
    private readonly ItemRepo _itemRepo = new ItemRepo();
    private readonly CustomerRepo _customerRepo = new CustomerRepo();
    private bool IsRunning = true;
    public void RunApplication()
    {
        Seed();
        Run();
    }

    public void Run()
    {
        while (IsRunning)
        {
            Clear();
            WriteLine(
                        "================================================\n" +
                        "=== Warner Transit Federal Delivery Tracking ===\n" +
                        "================================================\n" +
                        "    -----------------\n" +
                        "    Delivery Options\n" +
                        "    -----------------\n" +
                        "1.  Create New Delivery\n" +
                        "2.  See all Available Deliveries\n" +
                        "3.  Find Delivery by ID\n" +
                        "4.  Find Deliveries by Order Status\n" +
                        "5.  Edit Existing Delivery\n" +
                        "6.  Delete A Delivery\n" +
                        "    ------------------\n" +
                        "    Item and Customer Options\n" +
                        "    ------------------\n" +
                        "7.  Add Items to Existing Delivery\n" +
                        "8.  See all Items\n" +
                        "9.  Find item by ID\n" +
                        "10. Remove Items from Delivery\n" +
                        "11. Find Customer by ID\n" +
                        "12. See all Customers\n" +
                        "=================================================\n" +
                        "0.  Close App\n" +
                        "=================================================\n"
                        );

            var userSelection = int.Parse(ReadLine()!);
            switch (userSelection)
            {
                case 1:
                    CreateDelivery();
                    break;
                case 2:
                    GetAllDelivery();
                    break;
                case 3:
                    GetDeliveryById();
                    break;
                case 4:
                    GetDeliveriesByStatus();
                    break;
                case 5:
                    EditExistingDelivery();
                    break;
                case 6:
                    DeleteDelivery();
                    break;
                case 7:
                    AddItemsToDelivery();
                    break;
                case 8:
                    GetAllItems();
                    break;
                case 9:
                    GetItemById();
                    break;
                case 10:
                    RemoveItemsFromDelivery();
                    break;
                case 11:
                    GetCustomerbyId();
                    break;
                case 12:
                    GetAllCustomers();
                    break;
                case 0:
                    IsRunning = CloseApp();
                    break;
                default:
                    WriteLine("Invalid selection try again.");
                    PressAnyKey();
                    break;
            }
        }
    }

    private bool CloseApp()
    {
        Clear();
        return false;
    }

    #region DELIVERY CRUD

    //* CREATE
    private void CreateDelivery()
    {
        Clear();
        WriteLine("Create a new Delivery");
        Delivery delivery = FillOutDeliveryForm();

        if (_deliveryRepo.CreateDelivery(delivery))
        {
            WriteLine("Success!");
        }
        else
            WriteLine("fail");
        PressAnyKey();
    }

    private Delivery FillOutDeliveryForm()
    {

        Delivery deliveryFormData = new Delivery();

        //Show all customers
        GetAllCustomers();

        Write("Input the Customer ID delivery is for: ");
        deliveryFormData.CustomerId = int.Parse(ReadLine()!);

        WriteLine("Enter today's date (Year, Month, Day):");
        var orderDate = DateOnly.Parse(ReadLine()!);
        deliveryFormData.OrderDate = orderDate;

        //delivery date plus 5 days
        var deliveryDate = orderDate.AddDays(5);
        deliveryFormData.DeliveryDate = deliveryDate;

        //order status
        WriteLine("Enter the order status ( 1. Scheduled, 2. EnRoute, 3. Complete, or 4. Canceled)");

        var userStatusInput = int.Parse(ReadLine()!);
        switch (userStatusInput)
        {
            case 1:
                deliveryFormData.OrderStatus = Status.Scheduled;
                break;

            case 2:
                deliveryFormData.OrderStatus = Status.EnRoute;
                break;

            case 3:
                deliveryFormData.OrderStatus = Status.Complete;
                break;

            case 4:
                deliveryFormData.OrderStatus = Status.Canceled;
                break;
            default:
                WriteLine("invalid entry try again");
                PressAnyKey();
                break;
        }

        return deliveryFormData;
    }

    //* UPDATE
    private bool AddItemsToDelivery()
    {
        //need a loop that keeps asking if the user wants to add more items

        WriteLine("Here are all the available items you can add to your order.");
        GetAllItems();

        //a while loop that checks if the user said they are done adding items?
        bool doneAddingItems = false;
        while (doneAddingItems == false)
        {
            Write("Please enter the Id of the Delivery you'd like to add item(s) to.");
            int userInputDeliveryId = int.Parse(ReadLine()!);
            var selectedDelivery = _deliveryRepo.GetDeliveryById(userInputDeliveryId);

            GetAllItems();
            Write("Please enter the Id of the Item you'd like to add to that Delivery.");
            int userInputItemId = Convert.ToInt32(ReadLine()!);
            var selectedItem = _itemRepo.GetItemById(userInputItemId);

            Write($"Please enter the number of {selectedItem.Name} you'd like to add:");
            int userItemQuantity = int.Parse(ReadLine()!);

            Item item = RetriveItemData(userInputItemId);

            if (item == null)
                DataValidationCheck(userInputItemId);
            else
            {
                Clear();

                for (int i = 0; i < userItemQuantity; i++)
                {
                    _deliveryRepo.AddItemToDeliveryList(userInputDeliveryId, selectedItem);

                }

                WriteLine($"{userItemQuantity} {selectedItem.Name} has been added to Delivery ID: {selectedDelivery.Id}.");
                WriteLine("Do you want to add another item? Y/N");

                string userInputYesorNo = ReadLine()!;

                if (userInputYesorNo.ToLower() == "Y".ToLower())
                {
                    continue;
                }
                else
                {
                    WriteLine("Items have been added to the delivery.");
                    doneAddingItems = true;
                }
            }
        }
        return true;
    }
    private bool RemoveItemsFromDelivery()
    {
        //need a loop that keeps asking if the user wants to add more items
        Clear();
        WriteLine("Remove Items from Delivery");
        //a while loop that checks if the user said they are done adding items?
        bool doneAddingItems = false;
        while (doneAddingItems == false)
        {
            GetAllDelivery();
            Write("Please enter the Id of the Delivery you'd like to remove item(s) from: ");
            int userInputDeliveryId = int.Parse(ReadLine()!);
            var selectedDelivery = _deliveryRepo.GetDeliveryById(userInputDeliveryId);
            DisplayDeliveryInfo(selectedDelivery);

            GetAllItems();
            Write("Please enter the Id of the Item you'd like to remove from that Delivery: ");
            int userInputItemId = Convert.ToInt32(ReadLine()!);
            var selectedItem = _itemRepo.GetItemById(userInputItemId);

            Write($"Please enter the number of {selectedItem.Name} you'd like to remove: ");
            int userItemQuantity = int.Parse(ReadLine()!);

            Item item = RetriveItemData(userInputItemId);
            Delivery delivery = RetriveDeliveryData(userInputDeliveryId);

            if (item == null)
                DataValidationCheck(userInputItemId);
            else
            {
                Clear();

                if (_deliveryRepo.CountItemInDelivery(delivery, item) >= userItemQuantity)
                {
                    for (int i = 0; i < userItemQuantity; i++)
                    {
                        _deliveryRepo.RemoveItemFromDeliveryList(userInputDeliveryId, selectedItem);
                    }
                    WriteLine($"{userItemQuantity} {selectedItem.Name} has been removed from Delivery ID: {selectedDelivery.Id}.");
                    WriteLine("Do you want to remove another item? Y/N");

                    string userInputYesorNo = ReadLine()!;

                    if (userInputYesorNo.ToLower() == "Y".ToLower())
                    {
                        continue;
                    }
                    else
                    {
                        WriteLine("Items have been removed from the delivery.");
                        doneAddingItems = true;
                    }
                }
                else
                {
                    WriteLine("Could not remove requested amount of items. Make sure you're entering the correct quantity you want removed.");
                }
            }
        }
        return true;
    }
    private void EditExistingDelivery()
    {
        Clear();

        GetAllDelivery();

        Write("Please select a Delivery by ID: ");
        int userInputDeliveryId = int.Parse(ReadLine()!);
        Delivery delivery = RetriveDeliveryData(userInputDeliveryId);

        if (delivery == null)
        {
            WriteLine("Invalid entry, try again.");
        }
        else
        {
            Clear();
            Delivery newDeliveryData = FillOutDeliveryForm();

            if (_deliveryRepo.UpdateDeliveryInfo(delivery.Id, newDeliveryData))
                WriteLine("Successfully updated delivery info.");
            else
                WriteLine("Failed to update delivery info.");
        }
        PressAnyKey();
    }

    //* READ
    private void GetAllDelivery()
    {
        Clear();
        WriteLine("=== All Deliveries ===");

        var deliveriesInDb = _deliveryRepo.GetDeliveries();
        if (deliveriesInDb.Count > 0)
        {
            foreach (var delivery in deliveriesInDb)
            {
                DisplayDeliveryInfo(delivery);
            }
        }
        else
        {
            WriteLine("Sorry there are no available devliveries.");
        }
        PressAnyKey();


    }
    private void GetDeliveryById()
    {
        Clear();

        //show all available items
        GetAllDelivery();

        WriteLine("Please Select Delivery by Id");
        int userInputDeliveryId = int.Parse(ReadLine()!);
        
        Clear();

        Delivery delivery = RetriveDeliveryData(userInputDeliveryId);

        if (delivery == null)
        {
            DataValidationCheck(userInputDeliveryId!);
        }
        else
        {
            DisplayDeliveryInfo(delivery);
        }
        PressAnyKey();
    }
    private void GetDeliveriesByStatus()
    {
        Clear();

        WriteLine("Select deliveries by their current status.\n" +
                    "1. Scheduled\n" +
                    "2. Enroute\n" +
                    "3. Complete\n" +
                    "4. Canceled\n");

        var userInputStatus = int.Parse(ReadLine()!);
        Clear();
        switch (userInputStatus)
        {
            case 1:
                //grab all Scheduled
                List<Delivery> deliveryScheduled = _deliveryRepo.GetDeliveryByStatus(Status.Scheduled);
                if (deliveryScheduled.Count() > 0)
                {
                    foreach (var delivery in deliveryScheduled)
                    {
                        DisplayDeliveryInfo(delivery);
                    }
                }
                else
                {
                    WriteLine("There are no Scheduled deliveries right now.");
                }
                PressAnyKey();
                break;

            case 2:
                List<Delivery> deliveryEnroute = _deliveryRepo.GetDeliveryByStatus(Status.EnRoute);
                if (deliveryEnroute.Count() > 0)
                {
                    foreach (var delivery in deliveryEnroute)
                    {
                        DisplayDeliveryInfo(delivery);
                    }
                }
                else
                {
                    WriteLine("There are no EnRoute deliveries right now.");
                }
                PressAnyKey();
                break;

            case 3:
                List<Delivery> deliveryComplete = _deliveryRepo.GetDeliveryByStatus(Status.Complete);
                if (deliveryComplete.Count() > 0)
                {
                    foreach (var delivery in deliveryComplete)
                    {
                        DisplayDeliveryInfo(delivery);
                    }
                }
                else
                {
                    WriteLine("There are no Complete deliveries right now.");
                }
                PressAnyKey();
                break;

            case 4:
                List<Delivery> deliveryCanceled = _deliveryRepo.GetDeliveryByStatus(Status.Canceled);
                if (deliveryCanceled.Count() > 0)
                {
                    foreach (var delivery in deliveryCanceled)
                    {
                        DisplayDeliveryInfo(delivery);
                    }
                }
                else
                {
                    WriteLine("There are no Canceled deliveries right now.");
                }
                PressAnyKey();
                break;

            default:
                WriteLine("invalid selection try again.");
                PressAnyKey();
                break;
        }

    }
    private Delivery RetriveDeliveryData(int userInputDeliveryId)
    {
        Delivery delivery = _deliveryRepo.GetDeliveryById(userInputDeliveryId);
        return delivery;

    }

    private void DisplayDeliveryInfo(Delivery delivery)
    {
        var itemCounts = new Dictionary<string, int>();
        foreach (var deliveryItem in delivery.ItemsInDelivery)
        {
            if (itemCounts.ContainsKey(deliveryItem.Name))
            {
                itemCounts[deliveryItem.Name]++;
            }
            else
            {
                itemCounts[deliveryItem.Name] = 1;
            }
        }
        string itemsInDelivery = string.Join("\n ", itemCounts.Select(item => $" * {item.Key} - Amount: {item.Value}"));
        WriteLine(
                    "--------------------------------------------------------------------------------------------------\n" +
                    $"ID: {delivery.Id} | Order Status: {delivery.OrderStatus} | Order Date: {delivery.OrderDate} | Delivery Date: {delivery.DeliveryDate} | Customer Id: {delivery.CustomerId} \n" +
                    $"Items:\n {itemsInDelivery}\n" +
                    "--------------------------------------------------------------------------------------------------");
    }

    //* DELETE
    private void DeleteDelivery()
    {
        Clear();
        //show all available items
        GetAllDelivery();

        Write("Please Select Delivery by ID: ");
        int userInputDeliveryId = int.Parse(ReadLine()!);
        //get itemData pass in userInputById;
        Delivery delivery = RetriveDeliveryData(userInputDeliveryId);

        if (_deliveryRepo.DeleteDelivery(delivery))
        {
            WriteLine("Success!");
        }
        else
            WriteLine("Fail");
        PressAnyKey();
    }
    #endregion

    #region ITEM METHODS
    private void GetItemById()
    {
        Clear();

        //show all available items
        GetAllItems();

        WriteLine("Please Select an Item by Id");
        int userInputItemId = int.Parse(ReadLine()!);
        //get itemData pass in userInputById;
        Item item = RetriveItemData(userInputItemId);

        if (item == null)
        {
            DataValidationCheck(userInputItemId!);
        }
        else
        {
            Clear();
            DisplayItemInfo(item);
        }
        PressAnyKey();
    }

    private void GetAllItems()
    {
        Clear();
        WriteLine("== All Items ==\n");
        var itemsInDb = _itemRepo.GetItems();
        if (itemsInDb.Count > 0)
        {
            foreach (var item in itemsInDb)
            {
                DisplayItemInfo(item);
            }
        }
        else
        {
            WriteLine("Sorry there are no available items.");
        }

        PressAnyKey();
    }

    private void DisplayItemInfo(Item item)
    {
        WriteLine($"ID: {item.Id} Name: {item.Name} Description: {item.Description}");
    }

    //make other display methods


    private Item RetriveItemData(int userInputItemId)
    {
        Item item = _itemRepo.GetItemById(userInputItemId);
        return item;
    }
    #endregion

    #region CUSTOMER METHODS
    private void GetCustomerbyId()
    {
        Clear();

        //show all available items
        GetAllCustomers();

        WriteLine("Please Select a Customer by Id");
        int userInputCustomerId = int.Parse(ReadLine()!);
        //get itemData pass in userInputById;
        Customer customer = RetriveCustomerData(userInputCustomerId);

        if (customer == null)
        {
            DataValidationCheck(userInputCustomerId!);
        }
        else
        {
            Clear();
            DisplayCustomerInfo(customer);
        }
        PressAnyKey();
    }

    private void GetAllCustomers()
    {
        WriteLine("======== All Customers ========");
        var customersInDb = _customerRepo.GetCustomers();
        if (customersInDb.Count > 0)
        {
            foreach (var customer in customersInDb)
            {
                DisplayCustomerInfo(customer);
            }
        }
        else
        {
            WriteLine("Sorry there are no available customers.");
        }
        WriteLine("=================================");

        PressAnyKey();
    }

    private void DisplayCustomerInfo(Customer customer)
    {
        WriteLine($"ID: {customer.Id} First Name: {customer.FirstName} Last Name: {customer.LastName} Address: {customer.Address}\n");
    }

    private Customer RetriveCustomerData(int userInputItemId)
    {
        Customer customer = _customerRepo.GetCustomerById(userInputItemId);
        return customer;
    }


    #endregion

    #region HELPER METHODS

    private void DataValidationCheck(int userInputIdValue)
    {
        ForegroundColor = ConsoleColor.Red;
        WriteLine($"Invalid Entry: {userInputIdValue}!");
        ResetColor();
        return;
    }
    private void PressAnyKey()
    {
        WriteLine("Press any key to continue.");
        ReadKey();
    }

    #endregion

    #region SEED DATA
    private void Seed()
    {
        Item item1 = new Item(1, "Apple", "Apple fruit");
        Item item2 = new Item(2, "Banana", "Banana fruit");
        Item item3 = new Item(3, "Grape", "Grape fruit");
        Item item4 = new Item(4, "Lemon", "Lemon fruit");
        Item item5 = new Item(5, "Orange", "Orange fruit");
        Item item6 = new Item(6, "Kiwi", "Kiwi fruit");

        _itemRepo.CreateItem(item1);
        _itemRepo.CreateItem(item2);
        _itemRepo.CreateItem(item3);
        _itemRepo.CreateItem(item4);
        _itemRepo.CreateItem(item5);
        _itemRepo.CreateItem(item6);

        Customer customer1 = new Customer(1, "John", "Smith", "123 Street Ave., Chicago, IL");
        Customer customer2 = new Customer(2, "Harry", "Moore", "203 State St., Chicago, IL");
        Customer customer3 = new Customer(3, "Kenny", "Cooper", "909 Kenndy Ave., Chicago, IL");

        _customerRepo.CreateCustomer(customer1);
        _customerRepo.CreateCustomer(customer2);
        _customerRepo.CreateCustomer(customer3);

        var fillerOrderDate = new DateOnly(2023, 09, 28);
        var fillerDeliveryDate = new DateOnly(2023, 10, 28);

        Delivery delivery1 = new Delivery(1, fillerOrderDate, fillerDeliveryDate, Status.Scheduled, 1);
        Delivery delivery2 = new Delivery(2, fillerOrderDate, fillerDeliveryDate, Status.Scheduled, 2);
        Delivery delivery3 = new Delivery(3, fillerOrderDate, fillerDeliveryDate, Status.EnRoute, 2);
        Delivery delivery4 = new Delivery(4, fillerOrderDate, fillerDeliveryDate, Status.EnRoute, 1);
        Delivery delivery5 = new Delivery(5, fillerOrderDate, fillerDeliveryDate, Status.Complete, 2);
        Delivery delivery6 = new Delivery(6, fillerOrderDate, fillerDeliveryDate, Status.Complete, 1);
        Delivery delivery7 = new Delivery(7, fillerOrderDate, fillerDeliveryDate, Status.Canceled, 2);
        Delivery delivery8 = new Delivery(8, fillerOrderDate, fillerDeliveryDate, Status.Canceled, 1);

        _deliveryRepo.CreateDelivery(delivery1);
        _deliveryRepo.CreateDelivery(delivery2);
        _deliveryRepo.CreateDelivery(delivery3);
        _deliveryRepo.CreateDelivery(delivery4);
        _deliveryRepo.CreateDelivery(delivery5);
        _deliveryRepo.CreateDelivery(delivery6);
        _deliveryRepo.CreateDelivery(delivery7);
        _deliveryRepo.CreateDelivery(delivery8);

        //add some itesm to delivery1
        _deliveryRepo.AddItemToDeliveryList(1, item1);
        _deliveryRepo.AddItemToDeliveryList(1, item1);
        _deliveryRepo.AddItemToDeliveryList(1, item1);
        _deliveryRepo.AddItemToDeliveryList(1, item2);
        _deliveryRepo.AddItemToDeliveryList(1, item3);

    }

    #endregion

}