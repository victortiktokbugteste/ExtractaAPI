namespace Domain.Entities
{
    public class Claim
    {
        public Claim()
        {
                
        }
        public Claim(int insuredId, int vehicleId, decimal? claimValue, int? Id)
        {
            this.Id = Id;
            this.InsuredId = insuredId;
            this.VehicleId = vehicleId;
            this.ClaimValue = claimValue;
        }
        public int? Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int InsuredId { get; set; }
        public Person? Insured { get; set; }
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public decimal? ClaimValue { get; set; }

    }
}
