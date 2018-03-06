using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class FlexFormGridViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public string DesignedBy { get; set; }
        public DateTime? SendCreationDate { get; set; }     // Send Date
        public DateTime  ExpiryDate{ get; set; }            //Send form Expiry Date
        public byte FormType { get; set; } = 1; // Hidden field use as flag  1-Questionnaire  2-Interview   3-Test
    }


    public class FlexFormViewModel
    {
        //FlexForm
        public int Id { get; set; }

        [MaxLength(100)]
        public string FormName { get; set; }

        [MaxLength(250)]
        public string Purpose { get; set; }
        public int DesignedBy { get; set; }
        public byte FormType { get; set; } = 1; // Hidden field use as flag  1-Questionnaire  2-Interview   3-Test
        public List<FlexFormFSViewModel> FieldSets { get; set; }
        public int SendFormId { get; set; } 
        public DateTime  ExpiryDate { get; set; } //Send Form Expiry Date
        public List<PersonFormPageVM> personForm { get; set; }
    }

    public class FlexFormFSViewModel
    {
        public int Id { get; set; }
        public int FlexformId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public byte order { get; set; } // hidden
        public List<FlexFormColumnViewModel> Columns { get; set; }
    }

    public class FlexFormColumnViewModel
    {
        public int Id { get; set; }
        public int FlexFSId { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }
        public byte ColumnOrder { get; set; }
        public InputType InputType { get; set; } // Number, Text, Radio buttons, Checkboxes
        public bool ShowTextBox { get; set; } // show Other (for Radio buttons, Checkboxes)

        [MaxLength(1000)]
        public string Selections { get; set; }
        public bool ShowHint { get; set; }

        [MaxLength(100)]
        public string Hint { get; set; }
        public string type { get; set; }


        //[MaxLength(100)]
        //public string Answer { get; set; } // Hidden
    }
}
