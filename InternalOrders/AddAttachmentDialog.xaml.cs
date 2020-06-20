using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace InternalOrders {
    /// <summary>
    /// Logika interakcji dla klasy AddAttachmentDialog.xaml
    /// </summary>
    public partial class AddAttachmentDialog : Window, INotifyPropertyChanged {
        private string _fileName;
        private string _description;
        public event PropertyChangedEventHandler PropertyChanged;
        public string FileName {
            get { return _fileName; }
            set {
                _fileName = value;
                NotifyPropertyChanged();
            }
        }
        public string Description {
            get { return _description; }
            set {
                _description = value;
                NotifyPropertyChanged();
            }
        }

        public AddAttachmentDialog() : this("null") {
		}

        public AddAttachmentDialog(string fileName) {
            DataContext = this;
            FileName = fileName;
            InitializeComponent();
        }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnDialogAdd_Click(object sender, RoutedEventArgs e) {
			DialogResult = true;
		}
	}
}
