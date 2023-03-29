using Microsoft.Win32;
using Products.DB;
using Products.Model;
using Products.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Products.VM
{
    public class EditProductVM : BaseVM
    {
        private Status selectStatus;
 

        public CustomCommand Save { get; set; }
        public CustomCommand LoadImage { get; set; }
        public Product Product { get; set; }
       
        public List<Status> Statuses { get; set; }
        public Status SelectStatus
        {
            get => selectStatus;
            set
            {
                selectStatus = value;
                Signal();
                
            }
        }
        public EditProductVM(Product product)
        {
            Product = product;
            Statuses = DBInstance.GetInstance().Statuses.ToList();
            SelectStatus = Product.IdStatusNavigation;

            

            LoadImage = new CustomCommand(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    Product.Foto = File.ReadAllBytes(openFileDialog.FileName);
                    Signal(nameof(Product));
                }
            });
            Save = new CustomCommand(() =>
            {
                if (Product.Id == 0)
                {
                    DBInstance.GetInstance().Products.Add(Product);
                }
                Product.IdStatus = SelectStatus.Id;
                DBInstance.GetInstance().SaveChanges();
                MessageBox.Show("OK");
            });
        }
    }
}
