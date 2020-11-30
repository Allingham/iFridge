using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeModels{
    public class Product{
        [Key] public int Barcode{ get; set; }
        [ForeignKey("SubCategory")] public int SubCategoryId{ get; set; }
        public SubCategory SubCategory{ get; set; }
        public string ProductName{ get; set; }
        public int Expiration{ get; set; }
        public int Weight{ get; set; }
        public string Picture{ get; set; }

        public Product(){
            
        }

        public Product(int barcode, int subCategoryId, SubCategory subCategory, string productName, int expiration, int weight, string picture){
            Barcode = barcode;
            SubCategoryId = subCategoryId;
            SubCategory = subCategory;
            ProductName = productName;
            Expiration = expiration;
            Weight = weight;
            Picture = picture;
        }

        public override string ToString(){
            return $"Barcode: {Barcode}, SubCategory: {SubCategory}, ProductName: {ProductName}, Expiration: {Expiration}, Weight: {Weight}, Picture: {Picture}";
        }
    }
}