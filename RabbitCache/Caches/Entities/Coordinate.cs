namespace RabbitCache.Caches.Entities
{
    public class Coordinate
    {
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }

        public Coordinate()
        {

        }
        public Coordinate(double _latitude, double _longitude)
            : this()
        {
            this.Latitude = _latitude;
            this.Longitude = _longitude;
        }
    }
}