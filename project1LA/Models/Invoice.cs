using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace project1LA.Models
{
    [Table("Invoices")]
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column("Invoice")]
        public string InvoiceNumber { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Customer { get; set; }
        [Required]
        public decimal Total { get; set; }

    }
}

