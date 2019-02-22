using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AtlasAI.AIEditor
{

    public static class GuiSerializer
    {


        public static string Serialize(AICanvas canvas)
        {
            string data;

            data = JsonConvert.SerializeObject(canvas, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            });

            return data;
        }

        public static AICanvas Deserialize(AIUI host, string data)
        {
            if (string.IsNullOrEmpty(data)) return new AICanvas();

            var canvas = JsonConvert.DeserializeObject<AICanvas>(data, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            });

            host.canvas = canvas;
            return canvas;
        }







        public static string Serialize(AICanvas canvas, bool REMOVE)
        {
            //var data = new JObject();
            //data.Add("nodes", Serialize(canvas.nodes));
            //return data.ToString();

            StringWriter sw = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(sw);
            //
            writer.WriteStartObject();
            // "nodes" : TopLevelNodes
            writer.WritePropertyName("nodes");
            writer.WriteValue(Serialize(canvas.nodes));

            writer.WriteEndObject();
            //
            return sw.ToString();
        }


        public static string Serialize(List<TopLevelNode> items)
        {
            var jsonSettings = new JsonSerializer();
            jsonSettings.Formatting = Formatting.Indented;
            jsonSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            //jsonSettings.TypeNameHandling = TypeNameHandling.Auto;
            jsonSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;


            var data = new JArray();
            for (int i = 0; i < items.Count; i++)
            {
                var properties = new JObject();
                properties.Add("name", items[i].name);
                //properties.Add("parent", JToken.FromObject(items[i].parent));
                properties.Add("viewArea", JToken.FromObject(items[i].viewArea));
                properties.Add("id", items[i].id);


                if(items[i].GetType() == typeof(SelectorNode))
                {
                    var selectorNode = (SelectorNode)items[i];

                    //  Start new Array and add all qualifier nodes to the array.
                    var qualifierNodes = new JArray();
                    for (int k = 0; k < selectorNode.qualifierNodes.Count; k++){
                        qualifierNodes.Add(Serialize(selectorNode.qualifierNodes[k]));
                    }
                    //  Add the qualifiernodes field to the properties variable.
                    properties.Add("qualifierNodes", qualifierNodes);

                    //  Add the default qualifier.
                    properties.Add("defaultQualifierNode", Serialize(selectorNode.defaultQualifierNode));
                }

                //  Add the TopLevel node to the array.
                data.Add(properties);
            }

            return data.ToString();
        }


        public static string Serialize(QualifierNode qualifier)
        {
            var data = new JObject();

            data.Add("name", qualifier.name);
            if (qualifier.actionNode != null)
                data.Add("actionNode", JToken.FromObject(qualifier.actionNode));
            else
                data.Add("actionNode", "");
            data.Add("isDefault", qualifier.isDefault);
            data.Add("isExpanded", qualifier.isExpanded);
            return data.ToString();
        }


        public static string Serialize(ActionNode action)
        {
            throw new NotImplementedException();
        }








        public static AICanvas Deserialize(AIUI host, string data, bool remove)
        {
            if (string.IsNullOrEmpty(data)) return new AICanvas();

            var json = JObject.Parse(data);
            var nodes = json.SelectToken("nodes");



            var canvas = new AICanvas();
            canvas.nodes = new List<TopLevelNode>();

            //JArray topLevelNodes = (JArray)json["nodes"];
            //Debug.Log(json["nodes"].C);

            //IList<Rect> nodes = json["nodes"].Select(node => node.SelectToken("viewArea").ToObject<Rect>() ).ToList();
            //List<JToken> results = json.SelectToken("nodes").Children().ToList();
            //List<string> topLevelNodes = new List<string>();

            //for (int i = 0; i < results.Count; i++)
            //{
            //    string node = results[i].ToObject<string>();
            //    topLevelNodes.Add(node);
            //}
            Debug.Log(nodes.Children().ToList().Count);

            //Debug.Log(json.SelectToken("nodes")[0]);



            //Debug.Log(json.SelectToken("nodes[0].viewArea"));
            //foreach (var node in json.SelectToken("nodes").Children())
            //{
            //    Type type = typeof(ScoreSelector);
            //    Rect viewArea = node.SelectToken("viewArea").ToObject<Rect>();

            //    Debug.Log(viewArea.ToString());
            //}


            //foreach (var item in topLevelNodes)
            //{
            //    Type type = typeof(ScoreSelector);
            //    Rect viewArea = item.SelectToken("viewArea").ToObject<Rect>();

            //    Debug.Log(viewArea.ToString());
            //    //var node = SelectorNode.Create(type, host, viewArea);
            //    //canvas.nodes.Add(node);
            //}


            //for (int i = 0; i < topLevelNodes.Count; i++)
            //{
            //    Type type = typeof(ScoreSelector);
            //    Rect viewArea = topLevelNodes.
            //}

            //List<TopLevelNode> nodes = json["nodes"].Select(node => (TopLevelNode)node.SelectToken())





            //JsonConvert.PopulateObject(data, host.canvas, new JsonSerializerSettings
            //{
            //    TypeNameHandling = TypeNameHandling.Auto,
            //    NullValueHandling = NullValueHandling.Ignore,
            //    PreserveReferencesHandling = PreserveReferencesHandling.All
            //});

            //if(host.canvas == null){
            //    host.canvas = new AICanvas();
            //    Debug.Log(" ** Deserialization removed AICanvas.  Created a new one.");
            //}

            return host.canvas;
        }


        private static void DeserializeSelectorNodes(AIUI targetUI, List<TopLevelNode> newNodes)
        {
            
        }

        private static void DeserializeQualifier(AIUI targetUI)
        {

        }

        private static void DeserializeAction(AIUI targetUI)
        {

        }



        internal class ElementName
        {
            public const string Canvas = "Canvas";

            public const string AILinkView = "AILinkView";

            public const string SelectorView = "SelectorView";

            public const string QualifierView = "QualifierView";

            public const string ActionView = "ActionView";

            public const string ViewArea = "ViewArea";

            public const string Name = "Name";

            public const string Description = "Description";

            public const string DefaultQualifier = "DefaultQualifier";

            public const string Offset = "offset";

            public const string Zoom = "zoom";

            public const string AIPart = "ai_part";

            public const string UIPart = "ui_part";

            public const string ViewSnippet = "viewssnippet";

            public const string QualifierSnippet = "qualifiersnippet";

            public const string ActionSnippet = "actionsnippet";

            public const string Qualifier = "Qualifier";

            public const string Action = "Action";

            public const string ReferencePosition = "ReferencePosition";


        }

    }
}
