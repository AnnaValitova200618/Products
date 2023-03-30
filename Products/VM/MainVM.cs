﻿using Microsoft.EntityFrameworkCore;
using Products.DB;
using Products.Model;
using Products.Properties;
using Products.Tools;
using Products.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Products.VM
{
    public class MainVM : BaseVM
    {
        private Status selectStatus;
        private string search = "";
        private ObservableCollection<Product> products;
        public Product Foto { get; set; }
        public CustomCommand AddProduct { get; set; }
        public CustomCommand EditProduct { get; set; }
        public CustomCommand Cancel { get; set; }
        public CustomCommand DelProduct { get; set; }
        public ObservableCollection<Product> Products 
        { 
            get => products;
            set
            {
                products = value;
                Signal();
            }
        }
        public Product SelectProduct { get; set; }
        public ObservableCollection<Status> Statuses { get; set; }
        public Status SelectStatus
        {
            get => selectStatus;
            set
            {
                selectStatus = value;
                DoSearch();
            }
        }
        public string Search
        {
            get => search;
            set
            {
                search = value;
                DoSearch();
            }
        }

        private void DoSearch()
        {
            ObservableCollection<Product> products = 
                new ObservableCollection<Product>(db.Products.Include("IdStatusNavigation").Where(s => s.Name.Contains(Search)));
            

            if (SelectStatus != null)
            {
                products = new ObservableCollection<Product>(db.Products.Include("IdStatusNavigation").Where(s => s.IdStatus == SelectStatus.Id));
            }
           
            Products = products;
        }
        DbProductContext db;
        public MainVM()
        {
            db = DBInstance.GetInstance();
            db.Database.EnsureCreated();
            db.Products.Load();
            db.Statuses.Load();
            Products = new ObservableCollection<Product>(db.Products.Include(s => s.IdStatusNavigation).ToList());
            Statuses = db.Statuses.Local.ToObservableCollection();

            if (Statuses.Count == 0)
            {
                Statuses.Add(new Status { Title = "Продано" });
                Statuses.Add(new Status { Title = "Не продано" });
            }

            AddProduct = new CustomCommand(() =>
            {
                new EditProduct(new Product { Foto = Resources.dummy }).ShowDialog();
                Products = new ObservableCollection<Product>(db.Products.Include(s => s.IdStatusNavigation).ToList());

            });
            EditProduct = new CustomCommand(() =>
            {
                if (SelectProduct == null)
                {
                    MessageBox.Show("Необходимо выбрать продукт");
                    return;
                }
                else
                {
                    
                    new EditProduct(SelectProduct).ShowDialog();

                    Products = new ObservableCollection<Product>(db.Products.Include(s => s.IdStatusNavigation).ToList());

                }
            });
            DelProduct = new CustomCommand(() =>
            {
                if(SelectProduct != null && MessageBox.Show("Вы действительно хотите удалить выбранный продукт?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DBInstance.GetInstance().Products.Remove(SelectProduct);
                    DBInstance.GetInstance().SaveChanges();
                    Products = new ObservableCollection<Product>(db.Products.Include(s => s.IdStatusNavigation).ToList());
                }
                
            });
            Cancel = new CustomCommand(() =>
            {
                Search = "";
                SelectStatus = null;
                Signal(nameof(Search));
                Signal(nameof(SelectStatus));
            });
            
        }
        
    }
}
