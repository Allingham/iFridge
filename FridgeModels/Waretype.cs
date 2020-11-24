using System;
using System.ComponentModel.DataAnnotations;

namespace FridgeModels
{
    public class Waretype
    {

        [Key] public int Barcode { get; set; }

        public string WareName { get; set; }
        public string Category { get; set; }
        public int Expiration { get; set; }
        public int Weight { get; set; }
        public string Picture { get; set; }


        public Waretype()
        {

        }

        public Waretype(int barcode, string wareName, string category, int expiration, int weight, string picture)
        {
            Barcode = barcode;
            WareName = wareName;
            Category = category;
            Expiration = expiration;
            Weight = weight;
            Picture = picture;
        }


        public override string ToString()
        {
            return $"Barcode: {Barcode}, Name: {WareName}, Category: {Category}, Expiration: {Expiration}, Weight: {Weight}, Picture: {Picture}";
        }
    }
}
