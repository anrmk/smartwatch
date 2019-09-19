using System;

namespace Core.Dto {
    public class ProfileCardGeneralDto {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Middlename { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
