using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusClass
{
    public class BuildingListConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BuildingList);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // 检查是否是null引用
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            // 创建BuildingList实例
            BuildingList buildingList = new BuildingList();

            // 检查是否是数组开始
            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    Building building = serializer.Deserialize<Building>(reader);
                    buildingList.Add(building);
                }
            }

            return buildingList;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}