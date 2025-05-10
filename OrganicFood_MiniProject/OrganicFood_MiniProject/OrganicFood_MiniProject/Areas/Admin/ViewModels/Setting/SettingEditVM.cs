using System.ComponentModel.DataAnnotations;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Setting
{
    public class SettingEditVM
    {
        public int Id { get; set; }


        public string Key { get; set; }

        public string Value { get; set; }
    }
}  
