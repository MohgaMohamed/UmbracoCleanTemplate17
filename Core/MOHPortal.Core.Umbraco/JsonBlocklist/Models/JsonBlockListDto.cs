using System.Text.Json;
using System.Text.Json.Serialization;
using MOHPortal.Core.Umbraco.JsonBlocklist.Base;

namespace MOHPortal.Core.Umbraco.JsonBlocklist.Models
{
    public class JsonBlockList<TContentData> where TContentData : JsonBlockListContentData
    {
        [JsonPropertyName("layout")]
        public JsonBlockListContentsUdis Layout { get; set; } = new();

        [JsonPropertyName("contentData")]
        public List<TContentData> ContentData { get; set; } = new List<TContentData>();


        public void Add(TContentData contentData, bool forBlocklist = true)
        {
            ContentData.Add(contentData);
            JsonBlockListContentUdi contentUdiModel = new()
            {
                ContentUdi = contentData.Udi.ToString()
            };

            if (forBlocklist)
            {
                Layout.UmbracoBlockList.Add(
                    contentUdiModel
                );
            }
            else
            {
                Layout.UmbracoBlockGrid.Add(
                    contentUdiModel
                );
            }
        }

        public void Remove(TContentData? contentToRemove, bool forBlocklist = true)
        {
            if(contentToRemove is null)
            {
                return;
            }

            ContentData.Remove(contentToRemove);
            if (forBlocklist && 
                Layout.UmbracoBlockList.FirstOrDefault(a => a.ContentUdi == contentToRemove.Udi.ToString()) is JsonBlockListContentUdi udi)
            {
                Layout.UmbracoBlockList.Remove(udi);
            }
            else
            {
                if (Layout.UmbracoBlockGrid.FirstOrDefault(a => a.ContentUdi == contentToRemove.Udi.ToString()) is JsonBlockListContentUdi blockGridUdi)
                {
                    Layout.UmbracoBlockGrid.Remove(blockGridUdi);
                }
            }
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
