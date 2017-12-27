using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
    public class LanguageGridViewModel
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(15)]
        public string LanguageCulture { get; set; }

        [MaxLength(2)]
        public string UniqueSeoCode { get; set; }

        [MaxLength(20)]
        public string FlagImageFileName { get; set; }

        public bool Rtl { get; set; } = false;

        public int? DefaultCurrencyId { get; set; }

        public bool IsEnabled { get; set; }

        public int? DisplayOrder { get; set; }
    }
}
