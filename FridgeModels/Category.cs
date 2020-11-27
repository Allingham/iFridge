using System.ComponentModel.DataAnnotations;

namespace FridgeModels{
    public class Category{
        [Key] public int CategoryId{ get; set; }
        public string CategoryName{ get; set; }

        public Category(){
            
        }

        public Category(int categoryId, string categoryName){
            CategoryId = categoryId;
            CategoryName = categoryName;
        }

        public override string ToString(){
            return $"CategoryId: {CategoryId}, CategoryName: {CategoryName}";
        }
    }
}