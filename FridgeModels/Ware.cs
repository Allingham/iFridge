using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FridgeModels
{
    public class Ware
    {

        [Key] public int Id { get; set; }
        [ForeignKey("Waretype")] public int Barcode { get; set; }
        public Waretype Waretype { get; set; }

        public DateTime DateAdded { get; set; }
        public int OwnerId { get; set; }


        public Ware()
        {

        }


        public Ware(int id, int barcode, Waretype waretype, DateTime dateAdded, int ownerId)
        {
            Id = id;
            Barcode = barcode;
            Waretype = waretype;
            DateAdded = dateAdded;
            OwnerId = ownerId;
        }


        public override string ToString()
        {
            return $"Id: {Id}, Barcode: {Barcode}, Waretype: {Waretype}, DateAdded: {DateAdded}, OwnerId: {OwnerId}";
        }
    }
}
