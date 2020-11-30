using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeModels{
    public class ProductInstance{
        [Key] public int ProductInstanceId{ get; set; }
        public int OwnerId{ get; set; }
        [ForeignKey("Product")] public int Barcode{ get; set; }
        public Product Product{ get; set; }
        public DateTime DateAdded{ get; set; }

        public ProductInstance(){
            
        }

        public ProductInstance(int productInstanceId, int ownerId, int barcode, Product product, DateTime dateAdded){
            ProductInstanceId = productInstanceId;
            OwnerId = ownerId;
            Barcode = barcode;
            Product = product;
            DateAdded = dateAdded;
        }

        public override string ToString(){
            return $"ProductInstanceId: {ProductInstanceId}, OwnerId: {OwnerId}, Product: {Product}, DateAdded: {DateAdded}";
        }
    }
}