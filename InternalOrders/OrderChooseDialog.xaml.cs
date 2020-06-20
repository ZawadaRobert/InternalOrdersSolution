using InternalOrdersContext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InternalOrders {
    /// <summary>
    /// Logika interakcji dla klasy OrderChooseDialog.xaml
    /// </summary>
    public partial class OrderChooseDialog : Window {
		private readonly InternalOrderDbContext context = new InternalOrderDbContext();
		public OrderChooseDialog() {
            InitializeComponent();
			gridOrders.DataContext = Orders;
		}

		public int Result {
			get {
				Order order = (Order)gridOrders.SelectedItem;
				return order.OrderId;
			}
		}

		public ObservableCollection<Order> Orders {
			get {
				context.Orders.Load();
				return context.Orders.Local; }
		}
		private void btnDialogChoose_Click(object sender, RoutedEventArgs e) {
			DialogResult = true;
		}

		private void gridOrders_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e) {
			DialogResult = true;
		}
    }
}
