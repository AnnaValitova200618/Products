﻿using System;
using System.Collections.Generic;

namespace Products.Model
{
    public partial class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? PriceStart { get; set; }

        public string? PriceEnd { get; set; }

        public byte[]? Foto { get; set; }

        public DateTime? DateOfsale { get; set; }

        public int? IdStatus { get; set; }

        public virtual Status? IdStatusNavigation { get; set; }
    }
}
