﻿using ItaliaPizza.DataLayer;
using ItaliaPizza.UserInterfaceLayer.OrdersModule;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{
    public partial class AddressUC : UserControl
    {

        public AddressByClientView addressByClient {  get; set; }

        public AddressUC()
        {
            InitializeComponent();

        }

        public void EditDataCard(Address address)
        {

        }

        private void BtnEditClient_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
