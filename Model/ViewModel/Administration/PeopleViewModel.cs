using System;

namespace Model.ViewModel
{
    public class PeopleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string GenderN { get; set; }
        public string QualificationN { get; set; }
        public DateTime? JoinDate { get; set; }
        public int Icon { get; set; }
    }

}
