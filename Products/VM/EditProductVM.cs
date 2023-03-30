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
        private bool block = false;

        public CustomCommand Save { get; set; }
        public CustomCommand LoadImage { get; set; }
        public Product Product { get; set; }
        public bool Block 
        {
            get => block;
            set
            {
                block = value;
                Signal();
            }
        }
        public ObservableCollection<Status> Statuses { get; set; }
        public Status SelectStatus
        {
            get => selectStatus;
            set
            {
                selectStatus = value;
                if (selectStatus?.Id == 1)
                    Block = true;
                else
                {
                    Block = false;
                    Product.DateOfsale = null;
                }
                    
                Signal();
                Signal(nameof(Product));
                Signal(nameof(Block));

            }
        }
        DbProductContext db = new DbProductContext();
        

        public EditProductVM(Product product, Action close)
        {
            db.Products.Load();
            db.Statuses.Load();
            Product = product;
            Statuses = db.Statuses.Local.ToObservableCollection();
            SelectStatus = Statuses.FirstOrDefault(s=>s.Id==Product.IdStatus);

            

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
                try
                {
                    if(Block == true && Product.DateOfsale == null)
                    {
                        MessageBox.Show("Необходимо выбрать дату продажи");
                        return;
                    }
                    Product.IdStatus = SelectStatus?.Id;
                    if (Product.Id == 0)
                    {
                        DBInstance.GetInstance().Products.Add(Product);
                    }
                    else
                    {
                        Product.IdStatusNavigation = SelectStatus;
                        DBInstance.GetInstance().Entry(Product).State = EntityState.Modified;
                    }
                    DBInstance.GetInstance().SaveChanges();
                    MessageBox.Show("Сохранено");
                    close();
                }
                catch
                {
                    Save.Execute(null);
                    
                }
                

            });
        }
    }
}
