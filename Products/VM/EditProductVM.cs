using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Products.DB;
using Products.Model;
using Products.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
       
        public ObservableCollection<Status> Statuses { get; set; }
        public Status SelectStatus
        {
            get => selectStatus;
            set
            {
                selectStatus = value;
                Signal();
                
            }
        }
        DbProductContext db = new DbProductContext();
        public EditProductVM(Product product)
        {
            db.Products.Load();
            db.Statuses.Load();
            Product = product;
            Statuses = db.Statuses.Local.ToObservableCollection();
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
                    DBInstance.GetInstance().SaveChanges();
                    MessageBox.Show("OK");
                    return;
                }
                else
                {
                    Product.IdStatus = SelectStatus.Id;
                    DBInstance.GetInstance().Entry(Product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    DBInstance.GetInstance().SaveChanges();
                    MessageBox.Show("OK");
                }
                
                
            });
        }
    }
}
