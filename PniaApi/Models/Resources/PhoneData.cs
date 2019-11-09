namespace PniaApi.Models.Resources
{
    public class PhoneData : PhoneCoreData
    {
        public string Sector { get; set; }
    }

    public class PhoneCoreData
    {
        public string Number { get; set; }
        public string Prefix { get; set; }
    }
}
