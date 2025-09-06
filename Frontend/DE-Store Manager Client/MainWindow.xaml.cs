//This client uses the .NET HTTP library to send various HTTP requests to a Python FastAPI implementation, communicating with microservices hosted locally using Uvicorn.


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DE_Store_Manager_Client
{

    public partial class MainWindow : Window
    {
        //Setting up some global-ish variables to make the client work a bit more easily. The HTTP client should only be initialised once. The other variables are here to make it easier to track.
        private static readonly HttpClient client = new HttpClient();
        List<string> orderedItems = new List<string>();
        DateTime orderTime = new DateTime();

        public MainWindow()
        {

            InitializeComponent();
            retrieveItems();
        }

        //Method for hiding all of the components if microservices are unavailable
        public void hideComponents()
        {
            txtDropdown.Visibility = Visibility.Hidden;
            txtID.Visibility = Visibility.Hidden;
            txtItems.Visibility = Visibility.Hidden;
            txtName.Visibility = Visibility.Hidden;
            txtPrice.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Hidden;
            lblID.Visibility = Visibility.Hidden;
            lblPrice.Visibility = Visibility.Hidden;
            lblName.Visibility = Visibility.Hidden;
            lblSale.Visibility = Visibility.Hidden;
            lblPriceError.Visibility = Visibility.Visible;
            lblPriceControl.Visibility = Visibility.Hidden;
            txtStock.Visibility = Visibility.Hidden;
            btnOrder.Visibility = Visibility.Hidden;
            btnExport.Visibility = Visibility.Hidden;
            btnEmail.Visibility = Visibility.Hidden;
            lblInventoryControl.Visibility = Visibility.Hidden;
            lblStockError.Visibility = Visibility.Visible;
        }


        //Method for retrieving all items that need listing on the GUI (Price Control and Inventory Control microservices)
        public async void retrieveItems()
        {
            try //Try statement to catch exceptions in case the microservices are down
            {
                //This code block will send GET requests to /Item/List and /Stock/ListLow and deserialize them into an array of objects
                var shopItems = await client.GetStringAsync("http://127.0.0.1:8000/Item/List");
                var shopStock = await client.GetStringAsync("http://127.0.0.1:8000/Stock/ListLow");
                var itemList = JsonConvert.DeserializeObject<productItem[]>(shopItems);
                var stockList = JsonConvert.DeserializeObject<stockItem[]>(shopStock);


                //Display item objects for the Price Control microservice section
                for (int i = 0; i < itemList.Length; i++)
                {
                    txtItems.Items.Add(itemList[i].id + " " + itemList[i].name + " " + itemList[i].price + " " + itemList[i].itemSale);
                }

                //Input validation for stock, as the GET request returns "[]" even if the low-stock item database is empty.
                if(shopStock == "[]")
                {
                    txtNoLow.Visibility = Visibility.Visible;
                } 
                else {
                    //Display stock item objects for the Inventory Control microservice section
                    for (int i = 0; i < stockList.Length; i++)
                    {
                        txtStock.Items.Add(stockList[i].name + " " + stockList[i].stock);
                    }
                }

                //Add "enum" values to dropdown for Price Control
                txtDropdown.Items.Add("None");
                txtDropdown.Items.Add("HalfOff");
                txtDropdown.Items.Add("TwoForOne");
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException is SocketException socketException)
                {   //Display error if Microservices are down
                    if (socketException.SocketErrorCode == SocketError.ConnectionRefused)
                    {
                        hideComponents();
                        MessageBox.Show("Services unavailable, exception: " + ex.Message);
                    }
                }
            }
        }

        //Simple method for clearing various fields after inputs are performed
        public void clearFields()
        {
            txtID.Clear();
            txtName.Clear();
            txtPrice.Clear();
            txtDropdown.Items.Clear();
            txtItems.Items.Clear();
            txtStock.Items.Clear();
        }



        //Check if the selection on the ListBox has changed and display individual components of the selection in specific fields.
        private void txtItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (txtItems.SelectedItem != null)
            {
                string text = txtItems.SelectedItem.ToString();
                text = text.Replace(" ", ",");
                List<string> itemComponents = text.Split(',').ToList<string>();

                txtID.Text = itemComponents[0];
                txtName.Text = itemComponents[1];
                txtPrice.Text = itemComponents[2];

                txtDropdown.SelectedItem = itemComponents[3];


            }
        }


        //This async method will send a PUT request to the Price Control microservice, updating an item in the database with a new price and/or sale.
        private async void updateItem(object sender, RoutedEventArgs e)
        {
            //Initialise a URL for sending a PUT request to
            string putURL = "http://127.0.0.1:8000/Item/Update/";
            //Initialise a list of components for ease in making a productItem object to serialize
            List<string> components = new List<string>();


            //This large block of code will populate the components list and use that to create a productItem object. This object will then be serialized and put into StringContent so that we can use it in a PUT request.
            if(txtID.Text != string.Empty)
            {
                float x;
                if (float.TryParse(txtPrice.Text, out x))
                {
                    components.Add(txtID.Text);
                    components.Add(txtName.Text);
                    components.Add(txtPrice.Text);
                    components.Add(txtDropdown.SelectedValue.ToString());

                    putURL = putURL + components[0];
                    productItem PUTitem = new productItem(Int32.Parse(components[0]), components[1], float.Parse(components[2]), components[3]);


                    //Serialize object since API only accepts JSONs
                    var jsonItem = JsonConvert.SerializeObject(PUTitem, Formatting.Indented);
                    //Wrap it in StringContent for a HTTP request
                    var stringContent = new StringContent(jsonItem, Encoding.UTF8, "application/json");
                    //Send PUT request to API
                    HttpResponseMessage response = await client.PutAsync(putURL, stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        txtItems.SelectedIndex = -1;
                        clearFields();
                        retrieveItems();
                        MessageBox.Show("Item updated!", caption: "DE-Store");

                    } else
                    {
                        MessageBox.Show($"{response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    }
                } else
                {
                    MessageBox.Show("Invalid price!", caption: "Error");
                }
            } else
            {
                MessageBox.Show("No item selected!", caption: "Error");
            }


        }

        //This method will order more stock based on the items that are displayed on the client. The microservice will only return items that are low on stock (<100).
        private async void btnOrderStock(object sender, RoutedEventArgs e)
        {
            //Initialise URL for PUT request and other variables for easier processing
            string putURL = "http://127.0.0.1:8000/Stock/Update";
            string[] text = txtStock.Items.OfType<string>().ToArray();
            List<stockItem> lowStockList = new List<stockItem>();


            //This large block of code will extract all of the text data from the listbox and turn them into objects for later serialization and HTTP request.
            if (txtStock.Items.Count > 0)
            {
                //Extract data into smaller components so that we can make objects
                foreach (var item in text)
                {
                    string[] components = item.Split(' ');
                    lowStockList.Add(new stockItem(components[0], int.Parse(components[1])));
                }

                //For each object, add 100 to the stock (emulate ordering more items)
                foreach (stockItem stockItem in lowStockList)
                {
                    stockItem.stock += 100;
                    //Track items that were ordered for the Order report
                    orderedItems.Add(stockItem.name);
                }

                //Track order time for the Order report
                orderTime = DateTime.Now;
                //Serialize our list of objects so that we can perform a PUT request
                string jsonString = JsonConvert.SerializeObject(lowStockList, Formatting.Indented);
                //Wrap JSON in StringContent for HTTP request
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                //Send HTTP PUT request to API
                HttpResponseMessage response = await client.PutAsync(putURL, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    clearFields();
                    retrieveItems();
                    MessageBox.Show("Low stock items ordered!", caption: "DE-Store");
                }
                else
                {
                    MessageBox.Show($"{response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            } else
            {
                MessageBox.Show("No low stock items to order!", caption: "Error");
            }
        }

        //Simple method for exporting all of the items that were ordered into a report file.
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            string txtDir = (@"C:\DEStoreReports\testItem.txt");

            if(orderedItems.Count > 0)
            {
                using (StreamWriter sw = new StreamWriter(txtDir))
                {
                    sw.Write("The following items were ordered on " + orderTime + "\n");
                    foreach(var order in orderedItems)
                    {
                        sw.Write(order);
                        sw.Write("\n");
                    }
                }
            }
            MessageBox.Show("Items ordered: " + orderedItems.Count + ", Time ordered:  " + orderTime);
        }

        //Simple method for generating a template for customer reports
        private void btnGenerateCustomer(object sender, RoutedEventArgs e)
        {
            string CustomerReportForm = "Customer Analysis Report\n\nOur customer activity shows that customers have bought (sales) items over the last (months) months.";

            txtReport.Text = CustomerReportForm;


        }
        //Simple method for generating a template for shop reports
        private void btnGenerateStore(object sender, RoutedEventArgs e)
        {
            string StoreReportForm = "Store Analysis Report\n\nOur store has made (amount) of profit over the last (time period). We can gladly report that the store is showing growth";
            txtReport.Text = StoreReportForm;
        }


        //This method will write the report template to a report file.
        private void btnExportCustomer_Click(object sender, RoutedEventArgs e)
        {
            string txtDir = (@"C:\DEStoreReports\CustomerReport.txt");
            if(txtReport.Text.Length > 0)
            {
                using (StreamWriter sw = new StreamWriter(txtDir))
                {
                    sw.Write(txtReport.Text);
                    txtReport.Text = null;
                    MessageBox.Show("Report saved!", caption: "DE-Store");
                }
            } else
            {
                MessageBox.Show("No text in report section", caption: "Error");
            }
        }

        //This method will write the report template to a report file.
        private void btnExportStore_Click(object sender, RoutedEventArgs e)
        {
            string txtDir = (@"C:\DEStoreReports\StoreReport.txt");
            if(txtReport.Text.Length > 0)
            {
                using (StreamWriter sw = new StreamWriter(txtDir))
                {
                    sw.Write(txtReport.Text);
                    txtReport.Text = null;
                    MessageBox.Show("Report saved!", caption: "DE-Store");
                }
            } else
            {
                MessageBox.Show("No text in report section", caption: "Error");
            }
        }
    }
}
