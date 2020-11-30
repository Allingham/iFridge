using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeModels{
    public class SubCategory{
        [Key] public int SubCategoryId{ get; set; }
        [ForeignKey("Category")] public int CategoryId{ get; set; }
        public Category Category{ get; set; }
        public string SubCategoryName{ get; set; }
        public bool IsFluid{ get; set; }

        public SubCategory(){
            
        }

        public SubCategory(int subCategoryId, int categoryId, Category category, string subCategoryName, bool isFluid){
            SubCategoryId = subCategoryId;
            CategoryId = categoryId;
            Category = category;
            SubCategoryName = subCategoryName;
            IsFluid = isFluid;
        }

        public override string ToString(){
            return $"SubCategoryId: {SubCategoryId}, Category: {Category}, SubCategoryName: {SubCategoryName}, IsFluid: {IsFluid}";
        }
    }
}