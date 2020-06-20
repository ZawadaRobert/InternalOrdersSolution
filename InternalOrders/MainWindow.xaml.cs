using InternalOrdersContext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InternalOrders {
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        private readonly InternalOrderDbContext context = new InternalOrderDbContext();
        private Order _currOrder;
        private bool _isReadOnly = true;
        public event PropertyChangedEventHandler PropertyChanged;

        public Order CurrOrder {
            get { return _currOrder; }
            set {
                _currOrder = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsReadOnly {
            get { return _isReadOnly; }
            set {
                _isReadOnly = value;
                NotifyPropertyChanged();
            }
        }

        public MainWindow() {
            DataContext = this;
            InitializeComponent();
        }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnOrderChoose_Click(object sender, RoutedEventArgs e) {

            OrderChooseDialog inputDialog = new OrderChooseDialog();
            if (inputDialog.ShowDialog() == true) {
                CurrOrder = context.Orders.Where(o => o.OrderId == inputDialog.Result).FirstOrDefault(); ;

                //MessageBox.Show(Approvers.First().User.UserId.ToString());
            }
        }

        private void btnOrderNew_Click(object sender, RoutedEventArgs e) {

            /*
            Order newOrder = new Order() {
                Name = "NowyTest",
                Date = DateTime.Now,
                Description = "Opis tego zamówienia"
            };
            context.Orders.Add(newOrder);

            User newUser = new User() {
                Name = "Marek Juroszek",
                Password = "password",
            };
            context.Users.Add(newUser);
 
            Approver newApprover = new Approver() {
                Queue = 0,
                Status = StatusType.Canceled,
                UserId = 5,
                OrderId = 2
            };
            context.Approvers.Add(newApprover);

            Item newItem = new Item() {
                RekordIndex = "N020014",
                Name = "Forma",
                Price = 1000f,
                Quantity = 1,
                DeliveryDate = DateTime.Now,
                OrderId = 2
            };
            context.Items.Add(newItem);
             */

            try {
                context.SaveChanges();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show("Zmiany w zamówieniu wewnętrzynum spowodują anulowanie wszystkich obecnych zatwierdzeń.\nZapisać zmiany?", "Zmiana", MessageBoxButton.YesNo);
            switch (result) {
                case MessageBoxResult.Yes:
                    foreach (Approver approver in CurrOrder.Approvers) {
                        approver.Status = StatusType.Pending;
                    }
                    try {
                        context.SaveChanges();
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void gridAttachments_Drop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.
                HandleFileOpen(files);
            }
        }

        private void HandleFileOpen(string[] files) {
            foreach (string f in files) {
                try {

                    string fileName = System.IO.Path.GetFileName(f);
                    string description = "";

                    AddAttachmentDialog inputDialog = new AddAttachmentDialog(fileName);
                    if (inputDialog.ShowDialog() == true) {
                        fileName = inputDialog.FileName;
                        description = inputDialog.Description;
                        byte[] bytes;

                        // byte[] bytes = File.ReadAllBytes(f);

                        using (FileStream fsSource = new FileStream(f, FileMode.Open, FileAccess.Read)) {

                            bytes = new byte[fsSource.Length];

                            int numBytesToRead = (int)fsSource.Length;
                            int numBytesRead = 0;
                            while (numBytesToRead > 0) {
                                int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);
                                if (n == 0)
                                    break;
                                numBytesRead += n; 
                                numBytesToRead -= n;
                            }
                            numBytesToRead = bytes.Length;
                        }

                        Attachment newAttachment = new Attachment() {
                            FileName = fileName,
                            Description = description,
                            FileData = bytes
                        };
                         CurrOrder.Attachments.Add(newAttachment);

                         //Temporary solution of Notify Changes
                         CollectionViewSource.GetDefaultView(CurrOrder.Attachments).Refresh();
                    }

                    /*
                    try {
                        context.SaveChanges();
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                    */

                } catch (FileNotFoundException ioEx) {
                    MessageBox.Show(ioEx.Message);
                }
            }
        }
    }
}
