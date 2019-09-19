namespace Core.Dto {
    public class ProfileCardDtoList {
        public long Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Middlename { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string BirthDate { get; set; }

        public int Systolic { get; set; }
        public int Diastolic { get; set; }
    }
}
