using MongoDB.Bson;
using Newtonsoft.Json;
using Realms;

namespace Taxi_mobile.Models.Db
{
    public class PlaceAutoCompletePredictionEntity : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("_partition")]
        public string Description { get; set; }

        [MapTo("_placeId")]
        public string PlaceId { get; set; }

        [MapTo("_googleId")]
        public string GoogleId { get; set; }

        [MapTo("_reference")]
        public string Reference { get; set; }

        [MapTo("_createdAt")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        [MapTo("_structuredFormatting")]
        public StructuredFormattingEntity StructuredFormatting { get; set; }
    }

    public class StructuredFormattingEntity : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("_mainText")]
        public string MainText { get; set; }

        [MapTo("_secondaryText")]
        public string SecondaryText { get; set; }
    }
}
